using System;
using System.Collections.Generic;
using System.Text;

namespace Bams.Workflows.Models
{
    public class SchoolDto
    {
        public long Uid { get; set; }
        //public long DistrictUid { get; set; }
        public long AdmUnitUid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PIC { get; set; }
        public int Students { get; set; }
        public string Remarks { get; set; }
    }
}
