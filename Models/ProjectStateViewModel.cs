using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Models
{
    public class ProjectStateViewModel
    {
         
        [Key]
        public int ProjectStateId { get; set; }

        public string ProjectStateName { get; set; }

        //public Guid SiteId { get; set; }
    }
}
