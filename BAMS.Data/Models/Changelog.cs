using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Models
{
    public class Changelog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TableName { get; set; } //todo: change to a better name
        public int SourceId { get; set; }
        public int Editor { get; set; } //todo: change to a better name
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
