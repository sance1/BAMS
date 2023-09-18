namespace BAMS.Models
{
    public class CreateActivationCodeRequestDTO
    {
        public int ProjectId { get; set; }
        public int ContractId { get; set; }
        //public int AmountCodes { get; set; }
        //public string StatusUpload { get; set; }
        //public int AccountId { get; set; }
        public string Remarks { get; set; }
    }

    public class UpdateActivationCodeRequestDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ContractId { get; set; }
        //public int AmountCodes { get; set; }
        //public string StatusUpload { get; set; }
        //public int AccountId { get; set; }
        public string Remarks { get; set; }
    }
}
