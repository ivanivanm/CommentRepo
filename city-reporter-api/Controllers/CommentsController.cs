using DataAccessLayer.ApiProv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using DataAccessLayer.ApiProv;
using DataAccessLayer.Models;
using System.Diagnostics.Contracts;

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
        public async Task<ActionResult> AddComment(int userId,int reportId,DateTime postedOn,string commentContent)
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
        [HttpGet]
        [Authorize(Roles ="Guest")]
        [Route("report/{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            try
            {
                Comment returned = await _commentProv.ReadComment(id);

                return Ok(new { 
                usserId = returned.UserId,
                reportId = returned.ReportId,
                postedOn = returned.PostedOn,
                commentContent = returned.CommentContent
                });
            }
            catch (NullReferenceException exc)
            {
                if (exc.Message == "Comment with such a key doesn't exist")
                {
                    return NotFound("Comment with such an Id doesn't exist");
                }
                return NotFound();
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            try
            {
                await _commentProv.DeleteComment(id);

                return Ok();
            }
            catch (NullReferenceException exc)
            {
                if (exc.Message == "Comment data doesn't exist")
                {
                    return NotFound("There aren't any comments on the server");
                }
                else if (exc.Message == "Object doesn't exists")
                {
                    return NotFound("Comment with such an Id doesn't exist");
                }
                return NotFound();
            }
        }

    }
}
