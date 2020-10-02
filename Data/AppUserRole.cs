using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Data
{
    public class AppUserRole
    {
       
            [Key]
            public Guid UserId { get; set; }

            public Guid RoleId { get; set; }
        
    }
}
