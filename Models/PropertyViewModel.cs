using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ArchitectureProjectManagement.Models;
using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitectureProjectManagement.Models
{
    public class PropertyViewModel
    {
        public int PropertyId { set; get; }

        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }

        [Display(Name = "Property ERF / Lot Number")]
        public string PropertyERF_LotNo { get; set; }

        [Display(Name = "Property Address")]
        public string PropertyAddress { get; set; }

        [Display(Name = "Property SG Number")]
        public string PropertySGNo { get; set; }

        [Display(Name = "Is Complex")]
        public Boolean IsComplex { get; set; }

        [Display(Name = "Is Estate")]
        public Boolean IsEstate { get; set; }

        [Display(Name = "Number of Complex / Estate")]
        public string Complex_Estate_No { get; set; }

        //[Required]
        public string PropertyOwnerId { get; set; }

        public string PropertyOwnerFirstName { get; set; }

        public string PropertyOwnerLastName { get; set; }

        public string PropertyOwnerEmailAddress { get; set; }

        public string PropertyOwnerContactNo { get; set; }



    }

    public class PropertyOwnerDDListViewModel
    {
        public string PropertyOwnerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }


    public class CreatePropertyViewModel
    {
        public int PropertyId { set; get; }

        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }

        [Display(Name = "Property ERF / Lot Number")]
        public string PropertyERF_LotNo { get; set; }

        [Display(Name = "Property Address")]
        public string PropertyAddress { get; set; }

        [Display(Name = "Property SG Number")]
        public string PropertySGNo { get; set; }

        [Display(Name = "Is Complex")]
        public Boolean IsComplex { get; set; }

        [Display(Name = "Is Estate")]
        public Boolean IsEstate { get; set; }

        [Display(Name = "Number of Complex / Estate")]
        public string Complex_Estate_No { get; set; }

        public IEnumerable<SelectListItem> PropertyOwners { get; set; }

        public string PropertyOwnerId { get; set; }

        public string SiteId { get; set; }

    }


    public class PropertyDetailsViewModel
    {
        public int PropertyId { set; get; }

        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }

        [Display(Name = "Property ERF / Lot Number")]
        public string PropertyERF_LotNo { get; set; }

        [Display(Name = "Property Address")]
        public string PropertyAddress { get; set; }

        [Display(Name = "Property SG Number")]
        public string PropertySGNo { get; set; }

        [Display(Name = "Is Complex")]
        public Boolean IsComplex { get; set; }

        [Display(Name = "Is Estate")]
        public Boolean IsEstate { get; set; }

        [Display(Name = "Number of Complex / Estate")]
        public string Complex_Estate_No { get; set; }

        public string PropertyOwnerId { get; set; }

        public string PropertyOwnerFirstName { get; set; }

        public string PropertyOwnerLastName { get; set; }

        public string PropertyOwnerEmailAddress { get; set; }

        public string PropertyOwnerContactNo { get; set; }
    }

    public class PropertiesViewModel
    {
        public List<PropertyViewModel> properties { get; set; }
    }

    public class PropertyOwnerPropertyViewModel
    {
        public string PropertyOwnerId { get; set; }
        public string PropertyOwnerEmail { get; set; }
        public string PropertyOwnerDisplayName { get; set; }
        public Guid PropertyownerSiteId { get; set; }
        public List<PropertyViewModel> Properties { get; set; }
}
}
