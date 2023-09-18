using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace BAMS.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>
    {
        public RoleRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}