using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository
{
    public interface IBaseRepository<T> where T : class 
    {
        Task<T> GetById<TKey>(TKey id);

        IQueryable<T> Get();

        Task<List<T>> GetPaginationAsync(int PageNum, string orderColumn, bool isAscending,
            string? IncludeProperties = null, Expression<Func<T, bool>>? filter = null);

        Task<int> GetPagingTotalElement(Expression<Func<T, bool>>? filter = null);

        void BulkCreate(List<T> TypeList);

        void Create(T type);

        void BulkUpdate(List<T> TypeList);

        void Update(T type);

        void Delete(T type);

        Task<int> SaveAsync();
    }
}
