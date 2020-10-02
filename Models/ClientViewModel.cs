using System;
using System.ComponentModel.DataAnnotations;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Models
{
    public class ClientViewModel
    {
        public int ClientId { set; get; }

        public CompanyViewModel Company { get; set; }

        public int CompanyId { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public int ApplicationUserId { get; set; }

    }

    public class CreateClientViewModel
    {
        public int ClientId { set; get; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public int ApplicationUserId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /*[Required(ErrorMessage = "Please Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password & Confirm Password  do not match")]
        public string ConfirmPassword { get; set; }*/

        public CompanyViewModel Company { get; set; }

        public int CompanyId { get; set; }
    }
}
