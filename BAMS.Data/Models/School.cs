using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace BAMS.Data.Models
{
    public class School : ModelBase
    {
        public int ProjectId { get; set; }
        public int DistrictId { get; set; }
        public int AdministrativeUnitId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PIC { get; set; }
        public int Students { get; set; }
        public string Remarks { get; set; }
        public string Logo { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<ActivationCode> ActivationCodes { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public District District { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public AdministrativeUnit AdministrativeUnit { get; set; }
    }
}
