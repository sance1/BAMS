using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BAMS.Data.Models;
using System.Threading.Tasks;
using EightElements.Utils;

namespace BAMS.Data.Repositories
{
    public class DistrictRepository : ExtendedRepository<District>
    {
        public DistrictRepository(DbContext context) : base(context) { }
    }
}
