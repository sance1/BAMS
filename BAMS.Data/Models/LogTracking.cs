using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class LogTracking
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int EventId { get; set; }
        public string Data { get; set; }
    }
}
