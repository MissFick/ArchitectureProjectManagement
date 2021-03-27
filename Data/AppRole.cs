using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Data
{
    public class AppRole
    {

        [Key]
        public Guid Id { get; set; }

        public string NormalizedRoleName { get; set; }

        public string RoleName { get; set; }

        public Guid SiteId { get; set; }

    }
}
