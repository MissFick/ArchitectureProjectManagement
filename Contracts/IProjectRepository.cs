using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        public Task<ICollection<Project>> FindAllDraughtsmanProjects (string draftId);

        public Task<ICollection<Project>> FindAllByPropertyOwnerProjects(string propOwner);

        public Task<ICollection<Project>> GetAllActiveProjects();

        public Task<ICollection<Project>> GetAllArchivedProjects();

        public Task<ICollection<Project>> GetAllDormantProjects();

        public Task<ICollection<Project>> GetAllConcludedProjects();

        public Task<ICollection<Project>> GetAllLostProjects();

    }
}
