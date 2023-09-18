using System;

namespace BAMS.Models
{
    public class ReadAccountDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public string DistrictName { get; set; }
        public string Organization { get; set; }
        public string ProjectName { get; set; }
        public string RoleName { get; set; }
        public string SchoolName { get; set; }
    }
}