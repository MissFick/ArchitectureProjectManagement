using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Data
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        public string CompanyName { get; set;}

        public string CompanyEmail { get; set; }

        public string CompanyContactNo { get; set; }

        public byte[] CompanyLogo { get; set; }

        public Guid SiteId { get; set; }

    }
}
