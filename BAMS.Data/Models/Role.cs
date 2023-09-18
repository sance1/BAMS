using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAMS.Data.Models
{
    public class Role : ModelBase
    {
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public byte AccessLevel { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}