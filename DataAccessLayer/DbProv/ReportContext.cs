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

        public Task Delete(int key)
        {
            throw new NotImplementedException();
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

            ReportDbModel report = await reports.FirstOrDefaultAsync(c => c.IdReport == key);

            return report;
        }

        public Task<List<ReportDbModel>> ReadAll(bool useNavigationalProperties = false, bool isReadOnlyTrue = true)
        {
            throw new NotImplementedException();
        }

        public Task Update(ReportDbModel entity, bool useNavigationalProperties)
        {
            throw new NotImplementedException();
        }
    }
}
