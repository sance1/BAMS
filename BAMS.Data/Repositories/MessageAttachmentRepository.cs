using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class MessageAttachmentRepository : ExtendedRepository<MessageAttachment>
    {
        public MessageAttachmentRepository(DbContext context) : base(context)
        {
        }
    }
}
