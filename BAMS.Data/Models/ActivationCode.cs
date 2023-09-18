using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{    
    public class ActivationCode : ModelBase
    {
        
        public string Code { get; set; }
        public byte Status { get; set; }
        public DateTime ExpiryDate { get; set; }        
        public int ProjectId { get; set; }                
        public int ContractId { get; set; }
        public int DistrictId { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public int SchoolId { get; set; }
        public int UserAccountId { get; set; }
        public int ActivationCodeUploadId { get; set; }        
        public int ActivationCodeRequestId { get; set; }        
        public DateTime? RedeemDate { get; set; }
        public int? RedeemedBy { get; set; }
        public byte? RedeemMethod { get; set; }


        public Project Project { get; set; }
        public Contract Contract { get; set; }
        public ActivationCodeUpload ActivationCodeUpload { get; set; }
        public ActivationCodeRequest ActivationCodeRequest { get; set; }
        public School School { get; set; }
        public District District { get; set; }
        public AdministrativeUnit AdministrativeUnit { get; set; }
    }


    public class ActivationCodeStatus
    {
        public const byte New = 0;
        public const byte Redeemed = 1;
        public const byte Invalid = 4;
    }


    public class RedeemMethods
    {
        public const byte App = 1;
        public const byte Bams = 2;

    }
}
