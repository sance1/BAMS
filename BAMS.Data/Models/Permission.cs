using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class Permission : ModelBase
    {
        public int RoleId { get; set; }

        [Column(TypeName = "nvarchar(50)")] public string Group { get; set; }
        public int Access { get; set; }
    }
}