using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace ArchitectureProjectManagement.Models
{
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Email")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Company Contact Number")]
        public string CompanyContactNo { get; set; }

        [Display(Name = "Company Logo")]
        public byte[] CompanyLogo { get; set; }

        public string SiteId { get; set; }
    }

    public class CreateCompanyViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Email")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Company Contact Number")]
        public string CompanyContactNo { get; set; }

        [Display(Name = "Company Logo")]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile CompanyLogo { get; set; }
    }
}
