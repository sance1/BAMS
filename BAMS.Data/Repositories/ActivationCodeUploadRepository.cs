using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class ActivationCodeUploadRepository : RepositoryBase<ActivationCodeUpload>
    {
        public ActivationCodeUploadRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
