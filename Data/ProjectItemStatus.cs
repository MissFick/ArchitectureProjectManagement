using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArchitectureProjectManagement.Data
{
    public class ProjectItemStatus
    {
        [Key]
        public int ProjectItemStatusId { get; set; }

        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [ForeignKey("ProjectItemId")]
        public int ProjectItemId { get; set; }

        public ProjectItem ProjectItem { get; set; }

        public Boolean IsComplete { get; set; }

        public DateTime DateCompleted { get; set; }

        public string SiteId { get; set; }
    }
}
