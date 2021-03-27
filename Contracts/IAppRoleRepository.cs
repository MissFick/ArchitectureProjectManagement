using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IAppRoleRepository
    {
        
        //public Task<List<AppRole>> GetAllByRole(Guid roleId);

        public Task<AppRole> GetDraughtsmanRoleId(Guid siteId);

        public Task<AppRole> GetPropertyOwnerRoleId(Guid siteId);
    }
}
