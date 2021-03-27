using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class AppUserRoleRepository : IAppUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppUserRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AppUserRole>> GetAll() {
            var roleUsers = await _dbContext.cs_UserRole.ToListAsync();
            return roleUsers;
        }

        public async Task<List<AppUserRole>> GetAllByRole(Guid roleId)
        {

            var roleUsers = await _dbContext.cs_UserRole
                .Where(n => n.RoleId == roleId)
                .ToListAsync();
            return roleUsers;
        }

        public async Task<List<AppUserRole>> GetAllDraughtsman(Guid draughtsmanRoleId)
        {

            //const string draftId = "2ed90927-6881-449a-bd96-0b527f47d66c";
            //const string draftId = "2ee34395-7745-4b89-acad-58ccc338ebf5";
            var draughtsman = await _dbContext.cs_UserRole
                .Where(n => n.RoleId.ToString() == draughtsmanRoleId.ToString())
                .ToListAsync();
            return draughtsman;
        }

        public async Task<List<AppUserRole>> GetAllPropertyOwners(Guid propertyOwnerRoleId)
        {
            //const string PropOwnerId = "2e5b1123-10b1-4440-92df-9b4082351cbe";
            //const string PropOwnerId = "fd9f70ab-59f5-4ad9-8939-b2fcb80b6d17";
            var propOwners = await _dbContext.cs_UserRole
                .Where(n => n.RoleId.ToString() == propertyOwnerRoleId.ToString())
                .ToListAsync();
            return propOwners;
        }
    }
}
