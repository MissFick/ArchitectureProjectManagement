using System;
using System.ComponentModel.DataAnnotations;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Models
{
    public class DraughtsmanViewModel
    {
        
        public int DraughtsmanId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public ApplicationUserViewModel ApplicationUser { get; set; }

        //public int ApplicationUserId { get; set; }

    }

    public class DraughtsmanDDListViewModel
    {

        public string DraughtsmanId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public ApplicationUserViewModel ApplicationUser { get; set; }

        //public int ApplicationUserId { get; set; }

    }

    public class CreateDraughtsmanViewModel
    {
        public int DraughtsmanId { set; get; }

        public ApplicationUserViewModel ApplicationUser { get; set; }


        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Please Confirm Password")]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage = "Password & Confirm Password  do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Draughtsman Regitration Number")]
        public string DraughtsmanRegNo { get; set; }

        public CompanyViewModel Company { get; set; }

        public int CompanyId { get; set; }
    }


}
