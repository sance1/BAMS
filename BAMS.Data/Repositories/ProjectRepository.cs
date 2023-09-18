using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EightElements.Utils;
using System.Collections.Generic;

namespace BAMS.Data.Repositories
{
    public class ProjectRepository : ExtendedRepository<Project>
    {
        public ProjectRepository(DbContext context) : base(context) { }
    }
}
