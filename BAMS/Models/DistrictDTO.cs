
namespace BAMS.Models
{
    public class CreateDistrictDto
    {        
        public long ProjectUid { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }        
        public string Remarks { get; set; }        
    }

    public class UpdateDistrictDto
    {
        public long Uid { get; set; }  
        public long ProjectUid { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }       
        public string Remarks { get; set; }
    }
    
    public class UploadDistrictDTO
    {
        public string DistrictName { get; set; }
        public string PIC { get; set; }
    }

    public class ReadDistrictDto
    {
        public string Uid { get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string PIC { get; set; }
        public int Schools { get; set; }
        public int Students { get; set; }
        public string Remarks { get; set; }
        public int ActivationCodes { get; set; }
        public int ActivationCodesActive { get; set; }
        public bool IsCodeAvailable { get; set; } = false;
    }

}
