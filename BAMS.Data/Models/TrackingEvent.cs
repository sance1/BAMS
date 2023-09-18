using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class TrackingEvent
    {
        public int Id { get; set; }
        public string Event { get; set; }
        public string Params { get; set; }
    }
}
