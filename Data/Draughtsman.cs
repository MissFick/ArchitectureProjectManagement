using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ArchitectureProjectManagement.Data
{
    public class Draughtsman
    {
        [Key]
        public int DraughtsmanId { set; get; }

        public string DraughtsmanRegNo { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public Guid SiteId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ApplicationUserId { get; set; }

      
      
       
    }
}
