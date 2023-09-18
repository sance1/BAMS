using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("AdministrativeLevel")]
    public class AdministrativeLevel
    {        
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
    }
}
