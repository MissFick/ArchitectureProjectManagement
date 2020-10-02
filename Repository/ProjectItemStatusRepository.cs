using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class ProjectItemStatusRepository : IProjectItemStatusRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectItemStatusRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ProjectItemStatus entity)
        {
             await _dbContext.tb_ProjectItemStatus.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(ProjectItemStatus entity)
        {
            _dbContext.tb_ProjectItemStatus.Remove(entity);
            return await Save();
        }

        public async Task<ProjectItemStatus> Get(int id)
        {
            var projectitemstatus = await _dbContext.tb_ProjectItemStatus
                .Include(q => q.ProjectItem)
                .Include(q => q.Project)
                .FirstOrDefaultAsync(q => q.ProjectId == id);
            return projectitemstatus;
        }

        public async Task<ICollection<ProjectItemStatus>> GetAllById(int id)
        {
            var projectitemstatus = await _dbContext.tb_ProjectItemStatus
                .Include(q => q.ProjectItem)
                .Include(q => q.Project)
                .Where(q => q.ProjectId == id)
                .ToListAsync();
            return projectitemstatus;
        }

        public async Task<ICollection<ProjectItemStatus>> GetAll()
        {
            var projectitemstatuses = await _dbContext.tb_ProjectItemStatus.ToListAsync();
            return projectitemstatuses;

        }

        public async Task<ProjectItemStatus> FindByDetails(int projectId, int projectItemId)
        {
            var projectitemstatus = await _dbContext.tb_ProjectItemStatus
                .Where(n => n.ProjectId == projectId && n.ProjectItemId == projectItemId)
                .FirstOrDefaultAsync();
            return projectitemstatus; 
        }

        public async Task<bool> DoesExist(string id)
        {
            return true;
        }

        private object SomethingAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ProjectItemStatus entity)
        {
            _dbContext.tb_ProjectItemStatus.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}
