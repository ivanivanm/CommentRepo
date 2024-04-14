using DataBase.DbModels;
using DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;

namespace DataAccessLayer.DbProv
{
    public class CommentContext : IDb<CommentDbModel, int>
    {
        private readonly AppDbContext _appContext;
        public CommentContext(AppDbContext context)
        {
            _appContext = context;
        }
        public async Task Create(CommentDbModel entity)
        {
            if (_appContext.CommentDbModels.Contains(entity))
            {
                throw new InvalidOperationException("Object already exists");
            }
            await _appContext.CommentDbModels.AddAsync(entity);
            await _appContext.SaveChangesAsync();
        }

        public async Task Delete(int key)
        {
            if (_appContext.CommentDbModels is null)
            {
                throw new NullReferenceException("Comment data doesn't exist");
            }

            CommentDbModel comment = _appContext.CommentDbModels.Find(key);

            if (comment is null)
            {
                throw new NullReferenceException("Object doesn't exists");
            }

            _appContext.CommentDbModels.Remove(comment);
            await _appContext.SaveChangesAsync();

        }

        public async Task<CommentDbModel> Read(int entity, bool useNavigationalProperties = false, bool isReadOnlyTrue = true)
        {

            IQueryable<CommentDbModel> comments = _appContext.CommentDbModels;

            if (useNavigationalProperties)
            {
                comments = comments.Include(c => c.User).Include(c => c.Report);
            }
            if (isReadOnlyTrue)
            {
                comments.AsNoTrackingWithIdentityResolution();
            }

            CommentDbModel comment = await comments.
                FirstOrDefaultAsync(c => c.Id == entity);

            return comment;

        }

        public async Task<List<CommentDbModel>> ReadAll(bool useNavigationalProperties = false, bool isReadOnlyTrue = true)
        {

            IQueryable<CommentDbModel> comments = _appContext.CommentDbModels;

            if (useNavigationalProperties)
            {
                comments = comments.Include(c => c.User).Include(c => c.Report);
            }
            if (isReadOnlyTrue)
            {
                comments.AsNoTrackingWithIdentityResolution();
            }

            return await comments.ToListAsync();
        }

        public async Task Update(CommentDbModel entity, bool useNavigationalProperties)
        {
            CommentDbModel comment = await Read(entity.Id, true, false);

            if (comment is null)
            {
                throw new NullReferenceException("Object doesn't exists");
            }

            comment.CommentContent = entity.CommentContent;
            comment.PostedOn = entity.PostedOn;

            if (useNavigationalProperties)
            {
                UserDbModel user = _appContext.UserDbModels.Find(entity.UserId);
                ReportDbModel report = _appContext.ReportDbModels.Find(entity.ReportId);

                if (user is null)
                {
                    UserContext context = new UserContext(_appContext);
                    await context.Create(entity.User);
                }
                if (report is null)
                {
                    ReportContext context = new ReportContext(_appContext);
                    await context.Create(entity.Report);
                }

                comment.ReportId = entity.ReportId;
                comment.UserId = entity.UserId;

                comment.User = entity.User;
                comment.Report = entity.Report;
            }
            await _appContext.SaveChangesAsync();
        }
    }
}
