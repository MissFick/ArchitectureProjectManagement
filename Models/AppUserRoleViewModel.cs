using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Models
{
    public class AppUserRoleViewModel
    {

        [Key]
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

    }
}
