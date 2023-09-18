using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class ChangelogRepository : RepositoryBase<Changelog>
    {
        public ChangelogRepository(DbContext context) : base(context) { }
    }
}
