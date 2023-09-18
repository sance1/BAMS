using BAMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EightElements.Utils;

namespace BAMS.Data.Repositories
{
    public class SchoolRepository : ExtendedRepository<School>
    {
        public SchoolRepository(DbContext context) : base(context) { }
        
    }
}
