using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("Contract")]
    public class Contract : ModelBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Students { get; set; }
        public int ActivationCodes { get; set; }
        public byte Status { get; set; }
        public string Remarks { get; set; }


        public Project Project { get; set; }
        public ActivationCodeRequest ActivationCodeRequest { get; set; }
    }
}
