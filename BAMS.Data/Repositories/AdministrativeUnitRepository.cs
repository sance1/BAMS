using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class AdministrativeUnitRepository : ExtendedRepository<AdministrativeUnit>
    {
        public AdministrativeUnitRepository(DbContext context) : base(context)
        {
        }

        public async Task<string> GetPathByUid(long uid)
        {
            string path = await dbSet
                .Where(u => u.Uid == uid)
                .Select(u => u.Path + "/" + u.Id)
                .SingleOrDefaultAsync();
            return path != null ? path : "";
        }

        public async Task<string> GetPath(int id)
        {
            string path = await dbSet
                .Where(u => u.Id == id)
                .Select(u => u.Path + "/" + u.Id)
                .SingleOrDefaultAsync();
            return path != null ? path : "";
        }
    }
}
