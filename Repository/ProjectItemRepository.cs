using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class ProjectItemRepository : IProjectItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ProjectItem entity)
        {
            _dbContext.tb_ProjectItem.Add(entity);
            return await Save();
        }

        public async Task<bool> Delete(ProjectItem entity)
        {
            _dbContext.tb_ProjectItem.Remove(entity);
            return await Save();
        }

        public async Task<ProjectItem> Get(int id)
        {
            var projectitem = _dbContext.tb_ProjectItem.FindAsync(id);
            return await projectitem;
        }

        public async Task<ICollection<ProjectItem>> GetAll()
        {
            var projectitems = await _dbContext.tb_ProjectItem.ToListAsync();
            return projectitems;
        }

        public async Task<bool> DoesExist(string id)
        {
            return true;
        }

        public async Task<bool> Update(ProjectItem entity)
        {
            _dbContext.tb_ProjectItem.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

    }
}
