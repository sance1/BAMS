using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Models
{
    public class Text 
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Key { get; set; }
        public string LanguageCode { get; set; }
        public string Value { get; set; }
    }
}
