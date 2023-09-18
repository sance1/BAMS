namespace BAMS.Models
{
    public class ReadProjectDto
    {
        public long Uid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PartnerName { get; set; }
        public string ContactPerson { get; set; }
        public string PartnerPIC { get; set; }
        public string Remarks { get; set; }
        public int Contracts { get; set; }
        public int Districts { get; set; }
        public int Schools { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
    }
}
