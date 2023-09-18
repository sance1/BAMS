using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; }
        public long Uid { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
