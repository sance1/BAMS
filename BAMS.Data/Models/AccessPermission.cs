using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    public class AccessPermission : ModelBase
    {
        [Column(TypeName = "nvarchar(50)")] 
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")] 
        public string Group { get; set; }
        public int Permission { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string MenuUrl { get; set; }
        public int MenuOrder { get; set; }
    }
}