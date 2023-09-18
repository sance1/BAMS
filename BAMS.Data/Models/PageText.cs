using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("PageText")]
    public class PageText : ModelBase
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
