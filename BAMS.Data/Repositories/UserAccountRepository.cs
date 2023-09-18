using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class UserAccountRepository : ExtendedRepository<UserAccount>
    {
        public UserAccountRepository(DbContext context) : base(context) { }
    }
}