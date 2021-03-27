using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Project entity)
        {
            await _dbContext.tb_Project.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Project entity)
        {
            _dbContext.tb_Project.Remove(entity);
            return await Save();
        }

        public async Task<Project> Get(int id)
        {
            var project = await _dbContext.tb_Project.FindAsync(id);
            return project;
        }

        public async Task<ICollection<Project>> FindAllLostProjects()
        {
            var project = await _dbContext.tb_Project
                         .Where(n => n.ProjectStateId == 4)
                         //.Where(n => n.ProjectSiteId == siteid && n.ProjectStateId == 4)
                         .Include(n => n.Property)
                         .ToListAsync();
            return project;
        }


        

        public async Task<ICollection<Project>> FindAllDraughtsmanProjects(string draftId)
        {
            var projects = await _dbContext.tb_Project
                .Where(n => n.DraughtsmanId == draftId) //&& n.ProjectStateId == 1)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<ICollection<Project>> FindAllByPropertyOwnerProjects(string propOwner) {
            var projects = await _dbContext.tb_Project
                .Where(n => n.PropertyOwnerId == propOwner) //&& n.ProjectStateId == 2)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }


        public async Task<ICollection<Project>> GetAllActiveProjects(){
            var projects = await _dbContext.tb_Project
                .Where(n => n.ProjectStateId == 2)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<ICollection<Project>> GetAllArchivedProjects()
        {
            var projects = await _dbContext.tb_Project
                .Where(n => n.ProjectStateId == 6)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<ICollection<Project>> GetAllDormantProjects()
        {
            var projects = await _dbContext.tb_Project
                .Where(n => n.ProjectStateId == 3)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<ICollection<Project>> GetAllConcludedProjects()
        {
            var projects = await _dbContext.tb_Project
                .Where(n => n.ProjectStateId == 5)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<ICollection<Project>> GetAllLostProjects()
        {
            var projects = await _dbContext.tb_Project
                .Where(n => n.ProjectStateId == 4)
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }
        /*  public async Task<ICollection<Project>> FindAllByClientDetails(int clientId)
          {
              var projects = await _dbContext.tb_Project
                  .Include(n => n.Draughtsman)
                  .Include(n => n.Property)
                  .Include(n => n.Property.Client)
                  .Include(n => n.Property.Client.Company)
                  .Where(n => n.Property.ClientId == clientId)
                  .ToListAsync();
              return projects;
          }*/

        public async Task<ICollection<Project>> GetAll()
        {
            var projects = await _dbContext.tb_Project
                .Include(n => n.Property)
                .ToListAsync();
            return projects;
        }

        public async Task<bool> Update(Project entity)
        {
            _dbContext.tb_Project.Update(entity);
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
