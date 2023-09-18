using System;

namespace BAMS.Models
{
    public class UserAccountDTO
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool ActivationStatus { get; set; }
        public int StudentId { get; set; }
        public int ProjectId { get; set; }
        public int DistrictId { get; set; }
        public int SchoolId { get; set; }
        public string Class { get; set; }
    }

    public class UploadUserAccountDTO
    {
        public string StudentClass { get; set; }
        public string StudentFullName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}