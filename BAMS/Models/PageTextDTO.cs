using System;
namespace BAMS.Models
{  
    public class CreatePageTextDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }
    }

    public class UpdatePageTextDto
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }
    }

    public class ReadPageTextDto
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
