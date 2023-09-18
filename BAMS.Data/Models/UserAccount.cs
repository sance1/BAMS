using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class UserAccount : ModelBase
    {
        public int ProjectId { get; set; }
        public int DistrictId { get; set; }
        public int AdministrativeUnitId { get; set; }
        public int SchoolId { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }        
        public string UserName { get; set; }                
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public byte ActivationStatus { get; set; }
        

        public Project Project { get; set; }
        public District District { get; set; }
        public School School { get; set; }
        public AdministrativeUnit AdministrativeUnit { get; set; }
    }
}
