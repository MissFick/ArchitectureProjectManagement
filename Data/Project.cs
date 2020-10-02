using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArchitectureProjectManagement.Data
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string MunicipalRefNo { get; set; }

        public DateTime DateofSubmission { get; set; }

        public string MunicipalAssessmentOfficer { get; set; }

        public string AssessmentOfficerContactNo { get; set; }

        public string AssessmentOfficerEmail { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [ForeignKey("PropertyId")]
        public Property Property { get; set; }

        public int PropertyId { get; set; }

        public string PropertyOwnerId { get; set; }

        public string  DraughtsmanId { get; set; }

        public string SiteId { get; set; }

        public int ProjectStateId { get; set; }
    }
}

