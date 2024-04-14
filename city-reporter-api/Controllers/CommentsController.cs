using DataAccessLayer.ApiProv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using DataAccessLayer.ApiProv;
using DataAccessLayer.Models;

namespace city_reporter_api.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly UserProv _userProv;
        private readonly CommentProv _commentProv;
        public CommentsController()
        {
            _userProv = new UserProv();
            _commentProv = new CommentProv();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddComment([FromBody] int userId, [FromBody] int reportId, [FromBody] DateTime postedOn, [FromBody] string commentContent)
        {
            try
            {
                Comment comment = new Comment(0, reportId, userId, postedOn, commentContent);

                await _commentProv.CreateComment(comment);

                User commentUser = await _userProv.ReadUser(userId);

                return Ok(new {
                userName = commentUser.Name,
                postedOn = comment.PostedOn,
                commentContent = commentContent
                });
            }
            catch (NullReferenceException exc)
            {
                if (exc.Message == "user is null")
                {
                    return BadRequest("user with such an Id doesn't exist.");
                }
                else if (exc.Message == "report is null")
                {
                    return BadRequest("report with such an Id doesn't exist.");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (InvalidOperationException exc)
            {
                if (exc.Message == "Object already exists")
                {
                    return Conflict("comment with such parameters already exists");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

    }
}
