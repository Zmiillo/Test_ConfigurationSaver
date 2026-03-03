#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CS.DataLibrary.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);

        Task<int> AddAsync(T entity);

        Task<bool> RemoveAsync(int id);

        Task<int> UpdateAsync(T entity);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }
}
