using System;
using System.Collections.Generic;
using System.Text;

namespace Bams.Workflows.Models
{
    public class ContractDto
    {
        public long Uid { get; set; }
        public long ProjectUid { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ActivationCodes { get; set; }
        public string Remarks { get; set; }
    }
}
