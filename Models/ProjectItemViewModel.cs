using System;
using System.Collections.Generic;
using ArchitectureProjectManagement.Data;

namespace ArchitectureProjectManagement.Models
{
    public class ProjectItemViewModel
    {
        public int ProjectItemId { get; set; }

        public string ProjectItemName { get; set; }

        public string ProjectItemDescription { get; set; }

        public Guid SiteId { get; set; }
    }

    public class AssignProjectViewModel
    {
        public ProjectViewModel project { get; set; }

        public List<ProjectItem> ProjectItems { get; set; }

    }
}
