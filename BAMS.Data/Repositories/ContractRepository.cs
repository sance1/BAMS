using EightElements.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Contract = BAMS.Data.Models.Contract;

namespace BAMS.Data.Repositories
{
    public class ContractRepository : ExtendedRepository<Contract>
    {
        public ContractRepository(DbContext context) : base(context) { }
    }
}
