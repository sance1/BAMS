using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class ApplicationRepository : ExtendedRepository<Application>
    {
        public ApplicationRepository(DbContext context) : base(context) { }

    }
    
}
