using System;
using System.Collections.Generic;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Models
{
    public class ProjectStatusViewModel
    {
        //public ProjectViewModel Project { get; set; }
        public int ProjectId { get; set; }
        public Boolean IsChecked { get; set; }
        public int ProjectItemId { get; set; }
        public ProjectItem ProjItem { get; set; }
        public string SiteId { get; set; }
        //public List<CheckedProjectItem> CheckedProjectItems { get; set; }
        //public List<ProjectItemStatusViewModel> ProjectItemStatus { get; set; }
    }

    public class ProjectStatusItemViewModel
    {
        public int ProjectId;
        public ProjectDetailsViewModel Project;
        public List<ProjectItemStatusViewModel> ProjectItemStatuses { get; set; }
    }

}
