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
    public class RepositoryBase<T> : IRepositoryBase<T>, IDisposable 
        where T : class
    {
        internal readonly DbSet<T> dbSet;
        public DbContext Context { get; private set; }

        public DataContext BamsContext { get; private set; }

        public RepositoryBase(DbContext dbContext)
        {
            Context = dbContext;            
            dbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Add object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            T result = dbSet.Add(entity).Entity;
            Context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Add object asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            var process = await dbSet.AddAsync(entity);
            T result = process.Entity;
            Context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Add list of object asynchronously.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
            Context.SaveChanges();
        }

        /// <summary>
        /// Edit existing object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Edit(T entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Edit existing object asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public void EditSpecific(T entity)
        {
            dbSet.Attach(entity);
        }

        /// <summary>
        /// Delete object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Delete(T entity)
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
        public async Task DeleteAsync(T entity)
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
        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);            
        }

        /// <summary>
        /// Search object by string id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            return await dbSet.FindAsync(id);           
        }



        /// <summary>
        /// Search a single object by query.
        /// </summary>
        /// <param name="where">query function</param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync<T>();
        }
        
        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync<T>();
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
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, bool>> predicate = null,
            bool usePaging = false, int pageSize = 10, int skip = 0, string includeProperties = "")
        {
            var query = dbSet.Where(predicate);

            if (orderBy != null)
            {
                query = orderBy(query).Select(a => a);
            }

            if (usePaging)
            {
                query = query.Skip(skip).Take(pageSize);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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
        public async Task<List<T>> GetAsync(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, bool>> predicate = null,
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
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            var query = predicate != null ? dbSet.Where(predicate) : dbSet.Where(a => true);

            return await query.CountAsync();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = GetAll();

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
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

        public List<T> TestGetAll() 
        {
            return dbSet.ToList(); 
        }

    }
}
