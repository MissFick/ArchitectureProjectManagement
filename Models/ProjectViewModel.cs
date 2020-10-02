using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArchitectureProjectManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitectureProjectManagement.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Municipal Referenence Number")]
        public string MunicipalRefNo { get; set; }

        [Display(Name = "Date Of Submission")]
        public DateTime DateofSubmission { get; set; }

        [Display(Name = "Municipal Assessment Officer")]
        public string MunicipalAssessmentOfficer { get; set; }

        [Display(Name = "Assessment Officer Contact No")]
        public string AssessmentOfficerContactNo { get; set; }

        [Display(Name = "Assessment Officer Email")]
        public string AssessmentOfficerEmail { get; set; }

        [Display(Name = "Date Create")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public PropertyViewModel Property { get; set; }

        public int PropertyId { get; set; }

        public string DraughtsmanId { get; set; }

        public string PropertyOwnerId { get; set; }

        public int ProjectStateId { get; set; }

        public string SiteId { get; set; }

    }

    public class CreateProjectViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Municipal Reference Number")]
        public string MunicipalRefNo { get; set; }

        [Display(Name = "Date Of Submission")]
        public DateTime DateofSubmission { get; set; }

        [Display(Name = "Municipal Assessment Officer")]
        public string MunicipalAssessmentOfficer { get; set; }

        [Display(Name = "Assessment Officer Contact No")]
        public string AssessmentOfficerContactNo { get; set; }

        [Display(Name = "Assessment Officer Email")]
        public string AssessmentOfficerEmail { get; set; }

        [Display(Name = "Date Create")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public IEnumerable<SelectListItem> Properties { get; set; }

        public int PropertyId { get; set; }

        public IEnumerable<SelectListItem> Draughtsmen { get; set; }

        public string DraughtsmanId { get; set; }

        public string PropertyOwnerId { get; set; }

        public ProjectState ProjectState { get; set; }

        public int ProjectStateId { get; set; }

        public string SiteId { get; set; }
    }

    public class ProjectDetailsViewModel
    { 
            public int ProjectId { get; set; }

            [Display(Name = "Project Name")]
            public string ProjectName { get; set; }

            [Display(Name = "Project Description")]
            public string ProjectDescription { get; set; }

            [Display(Name = "Municipal Reference Number")]
            public string MunicipalRefNo { get; set; }

            [Display(Name = "Date Of Submission")]
            public DateTime DateofSubmission { get; set; }

            [Display(Name = "Municipal Assessment Officer")]
            public string MunicipalAssessmentOfficer { get; set; }

            [Display(Name = "Assessment Officer Contact No")]
            public string AssessmentOfficerContactNo { get; set; }

            [Display(Name = "Assessment Officer Email")]
            public string AssessmentOfficerEmail { get; set; }

            [Display(Name = "Date Create")]
            public DateTime DateCreated { get; set; }

            [Display(Name = "Date Modified")]
            public DateTime DateModified { get; set; }            

            public int PropertyId { get; set; }

            public string PropertyName { get; set; }

            public string PropertyDescription { get; set; }

            public string PropertyAddress { get; set; }

            public string PropertyERF_LotNo { get; set; }

            public bool IsComplex { get; set; }

            public bool IsEstate { get; set; }

            public string Complex_Estate_No { get; set; }

            public string PropertySGNo { get; set; }

            public string DraughtsmanId { get; set; }

            [Display(Name = "First Name")]
            public string DraughtsmanFirstName { get; set; }

            [Display(Name = "Last Name")]
            public string DraughtsmanLastName { get; set; }

            [Display(Name = "Email")]
            public string DraughtsmanEmail { get; set; }

            [Display(Name = "Contact Number")]
            public string DraughtsmanContactNo { get; set; }

            [Display(Name = "First Name")]
            public string PropertyOwnerFirstName { get; set; }

            [Display(Name = "Last Name")]
            public string PropertyOwnerLastName { get; set; }

            [Display(Name = "Email")]
            public string PropertyOwnerEmail { get; set; }

            //[Display(Name = "Contact Number")]
            //public string PropertyOwnerContactNo { get; set; }

            public string PropertyOwnerId { get; set; }

            public ProjectState ProjectState { get; set; }

            public int ProjectStateId { get; set; }

            public string SiteId { get; set; }
    }
}
