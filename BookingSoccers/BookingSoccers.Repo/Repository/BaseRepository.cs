using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public void BulkCreate(List<T> TypeList)
        {
            dbSet.AddRange(TypeList);
        }

        public void BulkUpdate(List<T> TypeList)
        {
            dbSet.UpdateRange(TypeList);
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

        public virtual async Task<List<T>> GetPaginationAsync(int PageNum, string orderColumn,
            bool isAscending, string? IncludeProperties = null,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            int PageSize = 20;

            if(IncludeProperties != null)
            {
                foreach (var includeProperty in IncludeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null) 
            {
                query = query.Where(filter);
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression prop = Expression.Property(parameter, orderColumn);
            var lambda = Expression.Lambda(prop, parameter);

            var parameter1 = Expression.Parameter(typeof(T), "x");
            Expression prop1 = Expression.Property(parameter1, "Id");
            var lambda1 = Expression.Lambda(prop1, parameter1);

            MethodInfo? OrderMethod = null;
            MethodInfo? ThenOrderMethod = null;

            if (isAscending)
            {
                OrderMethod = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);

                if (!orderColumn.Equals("Id")) 
                {
                    ThenOrderMethod = typeof(Queryable)
                    .GetMethods()
                    .First(x => 
                    x.Name == "ThenBy" && x.GetParameters().Length == 2);
                }
            }
            else
            {
                OrderMethod = typeof(Queryable)
                .GetMethods()
                .First(x => 
                x.Name == "OrderByDescending" && x.GetParameters().Length == 2);

                if (!orderColumn.Equals("Id"))
                {
                    ThenOrderMethod = typeof(Queryable)
                    .GetMethods()
                    .First(x =>
                    x.Name == "ThenByDescending" && x.GetParameters().Length == 2);
                }
            }

            var OrderGeneric = OrderMethod.MakeGenericMethod(typeof(T), prop.Type);

            var result = (IOrderedQueryable<T>?)OrderGeneric
                .Invoke(null, new object[] { query, lambda });

            MethodInfo? ThenOrderGeneric = null;
            IOrderedQueryable<T>? ProcessedResult = null;

            if (!orderColumn.Equals("Id")) 
            {
                ThenOrderGeneric = ThenOrderMethod
                    .MakeGenericMethod(typeof(T), prop1.Type);

               var result1 = (IOrderedQueryable<T>?)ThenOrderGeneric
                .Invoke(null, new object[] { result, lambda1 });

                ProcessedResult = result1;
            }
            else 
            {
                ProcessedResult = result;
            }

            var Finalresult = await ProcessedResult
            .Skip((PageNum - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

            return Finalresult as List<T>;
            
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

        public async Task<int> GetPagingTotalElement(Expression<Func<T, bool>>? filter = null)
        {
            int queryCount = 0;

            if (filter != null) return queryCount= await Get().Where(filter).CountAsync();

            return queryCount = await Get().CountAsync();
        }
    }
}
