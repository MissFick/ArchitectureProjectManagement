using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ArchitectureProjectManagement.Data
{
    public class Property
    {
        [Key]
        public int PropertyId { set; get; }

        public string PropertyName { get; set; }

        public string PropertyERF_LotNo { get; set; }

        public string PropertyAddress { get; set; }

        public string PropertySGNo { get; set; }

        public Boolean IsComplex { get; set; }

        public Boolean IsEstate { get; set; }

        public string Complex_Estate_No { get; set; }

        public string PropertyOwnerId { get; set; }

        public string SiteId { get; set; }

    }
}
