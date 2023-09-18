using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BAMS.Data.Models
{
    [Table("Application")]
    public class Application : ModelBase
    {
        public string Name { get; set; }

    }
}
