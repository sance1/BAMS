using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class CountryRepository : RepositoryBase<Country>
    {
        public CountryRepository(DbContext context) : base(context) { }
    }
}
