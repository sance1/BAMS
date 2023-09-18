using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("AdministrativeUnit")]
    public class AdministrativeUnit : ModelBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }    
        public int AdministrativeLevel { get; set; }
        public int? ParentId { get; set; }
        public string Path { get; set; }
        public string Remarks { get; set; }


        public Project Project { get; set; }
        public ICollection<School> Schools { get; set; }
        public ICollection<ActivationCode> ActivationCodes { get; set; }
    }
}
