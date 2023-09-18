namespace BAMS.Workflows.Models
{
    public class ProjectDto
    {
        public long Uid { get; set; }
        public long AppUid { get; set; } 
        public string Name { get; set; }
        public string PartnerName { get; set; }        
        public string ContactPerson { get; set; }
        public string PartnerPIC { get; set; }
        public string Remarks { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
    }


}
