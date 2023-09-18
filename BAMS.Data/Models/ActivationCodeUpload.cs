using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class ActivationCodeUpload : ModelBase
    {
        public int ActivationCodeRequestId { get; set; }
        public ActivationCodeRequest ActivationCodeRequest { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string UploadedFilePath { get; set; }

    }
}
