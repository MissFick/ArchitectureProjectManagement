using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Repository
{
    public class AppRoleRepository : IAppRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<AppRole> GetDraughtsmanRoleId(Guid siteId)
        {

            //const string draftId = "2ed90927-6881-449a-bd96-0b527f47d66c";
            //const string draftId = "2ee34395-7745-4b89-acad-58ccc338ebf5";
            var draughtsmanRoleId = await _dbContext.cs_Role
                .Where(n => n.RoleName == "Draughtsman" && n.SiteId == siteId)
                .FirstOrDefaultAsync();
            return draughtsmanRoleId;
        }

        public async Task<AppRole> GetPropertyOwnerRoleId(Guid siteId)
        {
            //const string PropOwnerId = "2e5b1123-10b1-4440-92df-9b4082351cbe";
            //const string PropOwnerId = "fd9f70ab-59f5-4ad9-8939-b2fcb80b6d17";
            var propOwnerRoleId = await _dbContext.cs_Role
                .Where(n => n.RoleName == "Property Owner" && n.SiteId == siteId)
                .FirstOrDefaultAsync();
            return propOwnerRoleId;
        }
    }
}

