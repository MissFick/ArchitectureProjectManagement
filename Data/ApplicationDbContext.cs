using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureProjectManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Company> tb_Company { get; set; }
        public DbSet<Project> tb_Project { get; set; }
        public DbSet<ProjectItem> tb_ProjectItem { get; set; }
        public DbSet<ProjectItemStatus> tb_ProjectItemStatus { get; set; }
        public DbSet<ProjectState> tb_ProjectState { get; set; }
        public DbSet<Property> tb_Property { get; set; }
        public DbSet<AppUserRole> cs_UserRole { get; set; }
        public DbSet<AppRole> cs_Role { get; set; }

    }
}
