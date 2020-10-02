using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class ProjectStateRepository : IProjectStateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectStateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ProjectState entity)
        {
            await _dbContext.tb_ProjectState.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(ProjectState entity)
        {
            _dbContext.tb_ProjectState.Remove(entity);
            return await Save();
        }

        public async Task<ProjectState> Get(int id)
        {
            var projectitem = await _dbContext.tb_ProjectState.FindAsync(id);
            return projectitem;
        }

        public async Task<ICollection<ProjectState>> GetAll()
        {
            var projectitems = await _dbContext.tb_ProjectState.ToListAsync();
            return projectitems;
        }

        public async Task<bool> Update(ProjectState entity)
        {
            _dbContext.tb_ProjectState.Update(entity);
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
