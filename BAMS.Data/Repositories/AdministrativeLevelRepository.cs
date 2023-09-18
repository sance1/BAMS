using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class AdministrativeLevelRepository : RepositoryBase<Models.AdministrativeLevel>
    {
        public AdministrativeLevelRepository(DbContext dbContext) : base(dbContext) { }
    }
}
