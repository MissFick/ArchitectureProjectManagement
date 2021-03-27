using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArchitectureProjectManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitectureProjectManagement.Models
{
    public class ProjectStateProjectVM
    {
        public int ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        [Display(Name = "Current Project State")]
        public string ProjectStateName { get; set; }

        [Display(Name = "Project State")]
        public IEnumerable<SelectListItem> StateSelectList { get; set; }

    }

   
}
    

