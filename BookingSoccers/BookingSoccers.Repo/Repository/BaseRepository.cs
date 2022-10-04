using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly BookingSoccersContext bookingSoccersContext;
public BaseRepository(BookingSoccersContext bookingSoccersContext)
        {
            this.bookingSoccersContext = bookingSoccersContext;
            this.dbSet = bookingSoccersContext.Set<T>();
        }

        public virtual void Create(T type)
        {
            dbSet.Add(type);
            
        }

        public virtual void Delete(T type)
        {
            dbSet.Remove(type);
            
        }

        public IQueryable<T> Get()
        {
            return this.dbSet;
            
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null) 
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) 
            {
                query = query.Include(includeProperty);
            }

            if (orderby != null) 
            {
                return await orderby(query).ToListAsync();
            }
            return await query.ToListAsync();
            
        }

        public virtual async Task<T> GetById<TKey>(TKey id)
        {
            return await this.dbSet.FindAsync(id);

        }

        public virtual async Task<int> SaveAsync()
        {
            return await bookingSoccersContext.SaveChangesAsync();
            
        }

        public virtual void Update(T type)
        {
            dbSet.Update(type);
            
        }
    }
}
