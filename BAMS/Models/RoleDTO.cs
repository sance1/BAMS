namespace BAMS.Models
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccessLevel { get; set; }
    }
    
    public class RolePermissionAccessDTO
    {
        public string key { get; set; }
        public int value { get; set; }
    }
}