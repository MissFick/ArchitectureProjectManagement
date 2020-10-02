using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ArchitectureProjectManagement.Models

{
    public class ProjectItemStatusViewModel
    {
        
        public int ProjectItemStatusId { get; set; }

        public ProjectViewModel Project { get; set; }

        public int ProjectId { get; set; }        

        public int ProjectItemId { get; set; }

        public ProjectItemViewModel ProjectItem { get; set; }

        public Boolean IsComplete { get; set; }

        public DateTime DateCompleted { get; set; }

        public string SiteId { get; set; }
    }

  /*  public class ProjectItemStatusesViewModel
    {
        public ProjectItemStatusViewModel ProjectItemsStatuses { get; set; }
        public Boolean IsChecked { get; set;  }
    }*/
}
