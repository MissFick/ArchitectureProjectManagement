using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ArchitectureProjectManagement.Data
{
    public class ApplicationUser
    {
        [Key]
        public int ApplicationUserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [ForeignKey("Id")]
        public IdentityUser IdentityUser { get; set; }

        public Guid Id { get; set; }

    }
}
