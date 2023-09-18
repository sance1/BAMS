using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BAMS.Data.Models
{
    [Table("District")]
    public class District : ModelBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }
        public string Remarks { get; set; }
        public string Logo { get; set; }

        public ICollection<ActivationCode> ActivationCodes { get; set; }
        public ICollection<School> Schools { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; }
        [JsonIgnore] 
        [IgnoreDataMember] 
        public Project Project { get; set; }
    }
}