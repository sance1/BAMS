using System;
using System.Collections.Generic;
using System.Text;

namespace Bams.Workflows.Models
{
    public class StudentDto
    {
        public long Uid { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
