using DataBase;
using DataBase.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DbProv
{
    public class ReportContext : IDb<ReportDbModel, int>
    {
        private readonly AppDbContext _appDbContext;

        public ReportContext(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Create(ReportDbModel entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity is null");
            }
            await _appDbContext.ReportDbModels.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(int key)
        {
            if (_appDbContext.ReportDbModels is null)
            {
                throw new NullReferenceException("Report data doesn't exist");
            }

            ReportDbModel report = _appDbContext.ReportDbModels.Find(key);

            if (report is null)
            {
                throw new NullReferenceException("Report doesn't exist");
            }

            _appDbContext.ReportDbModels.Remove(report);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ReportDbModel> Read(int entity, bool useNavigationalProperties = false, bool isReadOnlyTrue = true)
        {
            IQueryable<ReportDbModel> reports = _appDbContext.ReportDbModels;

            if (useNavigationalProperties)
            {
                reports = reports.Include(c => c.User);
            }
            if (isReadOnlyTrue)
            {
                reports.AsNoTrackingWithIdentityResolution();
            }

            ReportDbModel report = await reports.FirstOrDefaultAsync(c => c.IdReport == entity);

            return report;
        }

        public async Task<List<ReportDbModel>> ReadAll(bool useNavigationalProperties = false, bool isReadOnlyTrue = true)
        {
            IQueryable<ReportDbModel> reports = _appDbContext.ReportDbModels;

            if (useNavigationalProperties)
            {
                reports = reports.Include(c => c.User);
            }
            if (isReadOnlyTrue)
            {
                reports.AsNoTrackingWithIdentityResolution();
            }

            return await reports.ToListAsync();
        }

        public async Task Update(ReportDbModel entity, bool useNavigationalProperties)
        {
            ReportDbModel report = await Read(entity.IdReport, useNavigationalProperties, false);

            if (report is null)
            {
                throw new NullReferenceException("Report doesn't exist");
            }

            report.Title = entity.Title;
            report.Description = entity.Description;
            report.Location = entity.Location;
            report.Image = entity.Image;
            report.UserId = entity.UserId;

            if (useNavigationalProperties)
            {
                UserDbModel user = _appDbContext.UserDbModels.Find(entity.UserId);

                if (user is null)
                {
                    UserContext context = new UserContext(_appDbContext);
                    await context.Create(entity.User);
                }
                report.User = entity.User;
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}
