using DataAccessLayer.DbProv;
using DataAccessLayer.Models;
using DataBase.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ApiProv
{
    public class CommentProv
    {
        private readonly CommentContext _context;
        private readonly UserContext _userContext;
        private readonly ReportContext _reportContext;

        public CommentProv(CommentContext context, UserContext userContext, ReportContext reportContext)
        {
            _context = context;
            _userContext = userContext;
            _reportContext = reportContext;
        }
        public async Task CreateComment(Comment comment)
        {
            if (comment == null)
            {
                throw new NullReferenceException("comment is null");
            }

            UserDbModel commentUser = await _userContext.Read(comment.UserId);
            ReportDbModel commentReport = await _reportContext.Read(comment.ReportId);

            if (commentUser is null)
            {
                throw new NullReferenceException("user is null");
            }
            if (commentReport is null)
            {
                throw new ArgumentNullException("comment is null");
            }

            CommentDbModel newComment = new CommentDbModel(commentUser,comment.UserId,commentReport,comment.ReportId,comment.PostedOn,comment.CommentContent);

            await _context.Create(newComment);
        }
        public async Task<Comment> ReadComment(int key)
        {
            CommentDbModel commentFromDb = await _context.Read(key, true, false);

            if (commentFromDb is null)
            {
                throw new NullReferenceException("Comment with such a key doesn't exist");
            }

            Comment comment = new Comment(commentFromDb.Id, commentFromDb.UserId, commentFromDb.ReportId, commentFromDb.PostedOn, commentFromDb.CommentContent);

            return comment;
        }
        public async Task<List<Comment>> ReadAllComments()
        {
            List<CommentDbModel> commentsFromDb = await _context.ReadAll(true, false);

            if (commentsFromDb is null)
            {
                throw new NullReferenceException("Our app doesn't have comments");
            }

            List<Comment> comments = commentsFromDb.
                Select(cd => new Comment(cd.Id, cd.ReportId, cd.UserId, cd.PostedOn, cd.CommentContent)).ToList();

            return comments;
        }
        public async Task UpdateComment(Comment comment)
        {
            if (comment is null)
            {
                throw new NullReferenceException("Given comment as an argument is null");
            }

            UserDbModel commentUser = await _userContext.Read(comment.UserId);
            ReportDbModel commentReport = await _reportContext.Read(comment.ReportId);

            if (commentUser is null)
            {
                throw new NullReferenceException("user is null");
            }
            if (commentReport is null)
            {
                throw new ArgumentNullException("comment is null");
            }

            CommentDbModel dbComment = new CommentDbModel
                (commentUser,comment.UserId,commentReport,comment.ReportId,comment.PostedOn,comment.CommentContent);
            dbComment.Id = comment.Id;

            await _context.Update(dbComment, true);
        }
        public async Task DeleteComment(int key)
        {
            _context.Delete(key);
        }
    }
}
