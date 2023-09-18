using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BAMS.Data.Repositories
{
    public class AccountRepository : RepositoryBase<Models.Account>
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public string GetPreferredLanguage(int userId)
        {
            return dbSet
                .Where(a => a.Id == userId)
                .Select(a => a.PreferredLanguage)
                .SingleOrDefault();
        }

    }
}