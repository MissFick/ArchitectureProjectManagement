using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ApplicationUser entity)
        {
            _dbContext.tb_ApplicationUser.Add(entity);
            return await Save();
        }

        public async Task<bool> Delete(ApplicationUser entity)
        {
            _dbContext.tb_ApplicationUser.Remove(entity);
            return await Save();
        }

        public async Task<ApplicationUser> Get(int id)
        {
            var appuser = await _dbContext.tb_ApplicationUser.FindAsync(id);
            return appuser;
        } 

        public async Task<ICollection<ApplicationUser>> GetAll()
        {
            var appusers = await _dbContext.tb_ApplicationUser.ToListAsync();
            return appusers;
        }

        public async Task<bool> Update(ApplicationUser entity)
        {
            _dbContext.tb_ApplicationUser.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public Task<bool> DoesExist(string id)
        {
            throw new NotImplementedException();
        }
    }
}
