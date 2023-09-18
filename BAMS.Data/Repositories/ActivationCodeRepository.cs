using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class ActivationCodeRepository : ExtendedRepository<ActivationCode>
    {
        public ActivationCodeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ActivationCode> GetActivationCode(int schoolId)
        {
            return await dbSet.FirstOrDefaultAsync(a => 
                a.SchoolId == schoolId && 
                a.Status == ActivationCodeStatus.New);
        }
    }
}
