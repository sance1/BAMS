using System;
using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BAMS.Data.Interface;
using EightElements.Utils;

namespace BAMS.Data.Repositories
{
    public class LogTrackingRepository : IRepositoryTracking<LogTracking>, IDisposable
    {
        internal readonly DbSet<LogTracking> dbSet;
        public DbContext Context { get; private set; }

        public LogTrackingRepository(DbContext dbContext)
        {
            Context = dbContext;
            dbSet = dbContext.Set<LogTracking>();
        }

        /// <summary>
        /// Add object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public LogTracking Add(LogTracking entity)
        {
            LogTracking result = dbSet.Add(entity).Entity;
            Context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Add object asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<LogTracking> AddAsync(LogTracking entity)
        {
            var process = await dbSet.AddAsync(entity);
            LogTracking result = process.Entity;
            Context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Add list of object asynchronously.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<LogTracking> entities)
        {
            await dbSet.AddRangeAsync(entities);
            Context.SaveChanges();
        }

        /// <summary>
        /// Edit existing object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Edit(LogTracking entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        /// <summary>
        /// Edit existing object asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(LogTracking entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public void EditSpecific(LogTracking entity)
        {
            dbSet.Attach(entity);
        }

        /// <summary>
        /// Delete object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Delete(LogTracking entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete object asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(LogTracking entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Search object by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LogTracking> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Search object by string id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LogTracking> GetByIdAsync(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<LogTracking> GetByIdAsync(long id)
        {
            return await dbSet.Where(a => a.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Search a single object by query.
        /// </summary>
        /// <param name="where">query function</param>
        /// <returns></returns>
        public async Task<LogTracking> GetSingleAsync(Expression<Func<LogTracking, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync<LogTracking>();
        }

        public async Task<List<LogTracking>> ToListAsync(Expression<Func<LogTracking, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync<LogTracking>();
        }

        /// <summary>
        /// Search object by query with selector, returned type depend on selector.
        /// </summary>
        /// <param name="selector">selector function</param>
        /// <param name="orderBy">order by function</param>
        /// <param name="predicate">query function</param>
        /// <param name="usePaging">true if using pagination</param>
        /// <param name="pageSize">default 10 items</param>
        /// <param name="pageNumber">start from 0</param>
        /// <returns></returns>
        public async Task<List<TResult>> GetAsync<TResult>(
            Expression<Func<LogTracking, TResult>> selector,
            Func<IQueryable<LogTracking>, IOrderedQueryable<LogTracking>> orderBy = null,
            Expression<Func<LogTracking, bool>> predicate = null,
            bool usePaging = false, int pageSize = 10, int pageNumber = 0, string includeProperties = "")
        {
            var query = dbSet.Where(predicate);

            if (orderBy != null)
            {
                query = orderBy(query).Select(a => a);
            }

            if (usePaging)
            {
                query = query.Skip(pageNumber * pageSize).Take(pageSize);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.Select(selector).ToListAsync();
        }

        /// <summary>
        /// Search object by query.
        /// </summary>
        /// <param name="orderBy">order by function</param>
        /// <param name="predicate">query function</param>
        /// <param name="usePaging">true if using pagination</param>
        /// <param name="pageSize">default 10 items</param>
        /// <param name="skip">offset page</param>
        /// <returns></returns>
        public async Task<List<LogTracking>> GetAsync(
            Func<IQueryable<LogTracking>, IOrderedQueryable<LogTracking>> orderBy = null,
            Expression<Func<LogTracking, bool>> predicate = null,
            bool usePaging = false, int pageSize = 10, int skip = 0, string includeProperties = "")
        {
            var query = predicate != null ? dbSet.Where(predicate) : dbSet.Where(a => true);

            if (orderBy != null)
            {
                query = orderBy(query).Select(a => a);
            }

            if (usePaging)
            {
                query = query.Skip(skip).Take(pageSize);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Count number of objects.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<LogTracking, bool>> predicate = null)
        {
            var query = predicate != null ? dbSet.Where(predicate) : dbSet.Where(a => true);

            return await query.CountAsync();
        }

        public IQueryable<LogTracking> GetAll()
        {
            return dbSet;
        }

        public IQueryable<LogTracking> GetAllIncluding(params Expression<Func<LogTracking, object>>[] includeProperties)
        {
            IQueryable<LogTracking> query = GetAll();

            foreach (Expression<Func<LogTracking, object>> includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }


        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }

        public List<LogTracking> TestGetAll()
        {
            return dbSet.ToList();
        }
    }
}