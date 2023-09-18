using BAMS.Data.Interface;
using BAMS.Data.Models;
using EightElements.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class ExtendedRepository<T> : RepositoryBase<T> 
        where T : ModelBase
    {
        public ExtendedRepository(DbContext context) : base (context)
        { }

        public async Task<T> GetByIdAsync(long id)
        {
            return await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetByUidAsync(long uid)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.Uid == uid);
        }

        public async Task<TResult> GetByUidAsync<TResult>(
            long uid,
            Expression<Func<T, TResult>> selector)
        {
            return await dbSet
                .Where(x => x.Uid == uid)
                .Select(selector)
                .SingleOrDefaultAsync();
        }

        public async Task<int> GetIdByUid(long uid)
        {
            return await dbSet
                .Where(x => x.Uid == uid)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<long> GetUidById(int id)
        {
            return await dbSet
               .Where(x => x.Id == id)
               .Select(x => x.Uid)
               .SingleOrDefaultAsync();
        }

        public async Task<long> GenerateUid()
        {
            long uid = 0;
            while (uid == 0)
            {
                uid = RandomGenerator.GenerateRandomNumbers(15);
                int count = await dbSet.CountAsync(x => x.Uid == uid);
                if (count > 0) uid = 0;
            }
            return uid;
        }
    }
}
