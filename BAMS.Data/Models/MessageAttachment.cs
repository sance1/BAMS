using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Models
{
    public class MessageAttachment : ModelBase
    {
        public int MessageId { get; set; }
        public string FileName { get; set; }

        public Message Message { get; set; }
    }
}
