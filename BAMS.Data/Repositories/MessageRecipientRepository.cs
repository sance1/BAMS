using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class MessageRecipientRepository : ExtendedRepository<MessageRecipient>
    {
        public MessageRecipientRepository(DbContext context) : base(context) { }
    }
}
