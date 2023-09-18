using Microsoft.AspNetCore.Identity;

namespace BAMS.Helpers
{
    public interface IPasswordHasher
    {
        string Hash(string password);
  
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}