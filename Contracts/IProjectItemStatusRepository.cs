using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IProjectItemStatusRepository : IGenericRepository<ProjectItemStatus>
    {
        public Task<ICollection<ProjectItemStatus>> GetAllById(int projectId);
        public Task<ProjectItemStatus> FindByDetails(int projectId, int projectItemId);
    }
}
