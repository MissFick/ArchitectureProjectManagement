using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ArchitectureProjectManagement.Data
{
    public class Customer
    {      
        [Key]
        public int CustomerId { set; get; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public string SiteId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ApplicationUserId { get; set; }
    
    }
}
