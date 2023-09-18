
namespace BAMS.Models
{
    public class UploadSchoolDto
    {
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string PIC { get; set; }
        public int TotalStudent { get; set; }
    }

    public class ReadSchoolDto
    {
        public string Uid { get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int DistrictId { get; set; }        
        public string Name { get; set; }
        public string PIC { get; set; }
        public int Students { get; set; }
        public string Remarks { get; set; }
        public int ActivationCodes { get; set; }
        public string Address { get; set; }
        public string DistrictName { get; set; }
        public int AdminstrativeUnitId { get; set; }
    }
}
