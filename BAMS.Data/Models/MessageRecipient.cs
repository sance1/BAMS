using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Models
{
    public class MessageRecipient : ModelBase
    {
        public int MessageId { get; set; }
        public int AccountId { get; set; }
        public bool IsRead { get; set; }
        public bool IsFlag { get; set; }

        public Message Message { get; set; }
        public Account Account { get; set; }
    }
}
