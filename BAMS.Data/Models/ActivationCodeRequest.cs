using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class ActivationCodeRequest : ModelBase
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int AmountCodes { get; set; }
        public int StatusUpload { get; set; }
        public int AccountId { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Remarks { get; set; }

        public ICollection<ActivationCode> ActivationCodes { get; set; }
        public ActivationCodeUpload ActivationCodeUpload { get; set; }
    }
}
