using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("Project")]
    public class Project : ModelBase
    {
        public int AppId { get; set; }
        /*public string Country { get; set; }*/
        public string Name { get; set; }
        public string PartnerName { get; set; }
        public string ContactPerson { get; set; }
        public string PartnerPIC { get; set; }
        public string Remarks { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public Country Country { get; set; }
        public Province Province { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<District> Districts { get; set; }
        public ICollection<School> Schools { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
