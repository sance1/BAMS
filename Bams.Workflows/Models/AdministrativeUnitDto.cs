using System;
using System.Collections.Generic;
using System.Text;

namespace Bams.Workflows.Models
{
    public class AdministrativeUnitDto
    {
        public long Uid { get; set; }
        public long ProjectUid { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }        
        public long ParentUid { get; set; }
        public string Remarks { get; set; }
    }
}
