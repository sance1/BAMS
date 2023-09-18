using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace BAMS.Models
{
    public class CreateMessageDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool AllTeacher { get; set; }
        public bool AllDistrict { get; set; }
        public bool AllProvince { get; set; }    
        public bool ToAdmin { get; set; }
        public int SenderId { get; set; }
        public List<string> Recipients { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

    public class ReadMessageDTO
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string Body { get; set; }
        public MessageAccountDTO Sender { get; set; }
        public List<MessageAccountDTO> Recipents { get; set; }
        public List<MessageAttachmentsDTO> Attachments { get; set; }
    }

    public class DataInboxDTO
    {
        public List<InboxDTO> Data { get; set; }
        public int Prev { get; set; }
        public int Next { get; set; }
    }

    public class InboxDTO
    {
        public long Uid { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public MessageAccountDTO Sender { get; set; }
        public bool IsRead { get; set; }
        public bool IsFlag { get; set; }
    }
    
    public class SugesstionDTO
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }

    public class MessageAccountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }

    public class MessageAttachmentsDTO
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public string Filename { get; set; }
    }
}
