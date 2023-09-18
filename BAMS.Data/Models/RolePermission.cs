using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class RolePermission : ModelBase
    {
        public int RoleId { get; set; }
        public string Group { get; set; }
        public int Access { get; set; }
    }
}
