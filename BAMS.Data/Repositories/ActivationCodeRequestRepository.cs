using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class ActivationCodeRequestRepository : RepositoryBase<ActivationCodeRequest>
    {
        public ActivationCodeRequestRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
