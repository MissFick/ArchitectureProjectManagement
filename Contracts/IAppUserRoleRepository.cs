using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Contracts
{
    public interface IAppUserRoleRepository
    {
        public Task<List<AppUserRole>> GetAll();

        public Task<List<AppUserRole>> GetAllByRole(Guid roleId);
            
        public Task<List<AppUserRole>> GetAllDraughtsman();

        public Task<List<AppUserRole>> GetAllPropertyOwners();

    }
}
