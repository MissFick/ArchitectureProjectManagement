using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArchitectureProjectManagement.Models
{
    public class ApplicationUserViewModel
    {
        public int ApplicationId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Id { get; set; }

        public IdentityUser IdentityUser { get; set; }

    }

    public class CreateApplicationUser
    {
        public int ApplicationId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Id { get; set; }

        public IdentityUser IdentityUser { get; set; }

        public string Password { get; set; }

    }
}
