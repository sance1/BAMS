using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data.Repositories
{
    public class MessageRepository : ExtendedRepository<Message>
    {
        public MessageRepository(DbContext context) : base(context) { }

    }
}
