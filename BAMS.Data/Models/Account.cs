using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class Account :ModelBase
    {
        public int? ProjectId { get; set; }
        public int? ContractId { get; set; }
        public int? DistrictId { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public int? SchoolId { get; set; }        
        public string UserName { get; set; }        
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Organization { get; set; }  
        public string PreferredLanguage { get; set; }

        public School? School { get; set; }
        public Role Role { get; set; }
        public Project? Project { get; set; }
        public Contract? Contract { get; set; }
        public District? District { get; set; }
        public AdministrativeUnit? AdministrativeUnit { get; set; }
    }
}
