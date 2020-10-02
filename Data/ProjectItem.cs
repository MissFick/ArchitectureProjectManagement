using System;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureProjectManagement.Data
{
    public class ProjectItem
    {
        [Key]
        public int ProjectItemId { get; set; }

        public string ProjectItemName { get; set; }

        public string ProjectItemDescription { get; set; }

        public string SiteId { get; set; }
    }
}
