using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BAMS.Data.Models
{
    public class Message : ModelBase
    {
        public int AccountId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public Account Account { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<MessageRecipient> MessageRecipients { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<MessageAttachment> MessageAttachments { get; set; }

    }
}
