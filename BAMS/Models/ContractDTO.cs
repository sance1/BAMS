using System;

namespace BAMS.Models
{
    

    public class ReadContractDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Uid { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Students { get; set; }
        public int ActivationCodes { get; set; }
        public int Status { get; set; }
        public int StatusUpload { get; set; }
        public string StatusText { get; set; }
        public string StatusUploadText { get; set; }
        public string Remarks { get; set; }
        public string RemarksRequest { get; set; }
        public int UploadedCode { get; set; }
        public int ActivationCodeRequestId { get; set; }
        public DateTime? CodeRequestDate { get; set; }
        public DateTime? CodeUploadDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
