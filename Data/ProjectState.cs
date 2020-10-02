using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Data
{
    public class ProjectState
    {
        [Key]
        public int ProjectStateId { get; set; }

        public string ProjectStateName { get; set; }

        public Guid SiteId { get; set; }

       // public Boolean Created { get; set; }

        // public Boolean Active { get; set; }

        // public Boolean Concluded { get; set; }

        // public Boolean Dormant_Lost { get; set; }

        // public Boolean ReOpened { get; set; }

        //public string SiteId { get; set; }

    }
}
