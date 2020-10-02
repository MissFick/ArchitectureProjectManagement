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

        public async Task<List<AppUserRole>> GetAllDraughtsman()
        {
            const string draftId = "2ed90927-6881-449a-bd96-0b527f47d66c";
            var draughtsman = await _dbContext.cs_UserRole
                .Where(n => n.RoleId.ToString() == draftId)
                .ToListAsync();
            return draughtsman;
        }

        public async Task<List<AppUserRole>> GetAllPropertyOwners()
        {
            const string PropOwnerId = "2e5b1123-10b1-4440-92df-9b4082351cbe";
            var propOwners = await _dbContext.cs_UserRole
                .Where(n => n.RoleId.ToString() == PropOwnerId)
                .ToListAsync();
            return propOwners;
        }
    }
}
