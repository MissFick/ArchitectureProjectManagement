using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class DraughtsmanRepository : IDraughtsmanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DraughtsmanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Draughtsman entity)
        {
            await _dbContext.tb_Draughtsman.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Draughtsman entity)
        {
            _dbContext.tb_Draughtsman.Remove(entity);
            return await Save();
        }

        public async Task<Draughtsman> Get(int id)
        {
            var draughtsman = _dbContext.tb_Draughtsman
                .Include(q => q.ApplicationUser)
                .Include(q => q.ApplicationUser.IdentityUser)
                .Where(q => q.DraughtsmanId == id)
                .FirstOrDefaultAsync();
            return await draughtsman;
        }

        public async Task<ICollection<Draughtsman>> GetAll()
        {
            var draughtsmen = _dbContext.tb_Draughtsman
                .Include(q => q.ApplicationUser)
                .Include(q => q.ApplicationUser.IdentityUser)
                .ToListAsync();
            return await draughtsmen;
        }

        public async Task<bool> DoesExist(string draughtsmanregno)
        {
            bool exists = await _dbContext.tb_Draughtsman
                            .Include(q => q.ApplicationUser)
                            .Include(q => q.ApplicationUser.IdentityUser)
                            .Where(n => n.DraughtsmanRegNo == draughtsmanregno)
                            .AnyAsync();
            return exists;
        }


        public async Task<int> FindIdByDetails(string draughtsmanregno)
        {
            var companies =  await _dbContext.tb_Draughtsman
                            .Include(q => q.ApplicationUser)
                            .Include(q => q.ApplicationUser.IdentityUser)
                            .Where(n => n.DraughtsmanRegNo == draughtsmanregno)
                            .FirstOrDefaultAsync();
            var draftidentity = companies.DraughtsmanId;
            return draftidentity;
        }

        public async Task<int> FindDraughtsmanId(string id)
        {
            var draftman = await _dbContext.tb_Draughtsman
                .Where(n => n.ApplicationUser.Id == id)
                .FirstOrDefaultAsync();
               var Draftid = draftman.DraughtsmanId;
            return Draftid;
        }

        public async Task<bool> Update(Draughtsman entity)
        {
            _dbContext.tb_Draughtsman.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
