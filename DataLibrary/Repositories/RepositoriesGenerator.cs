using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CS.DataLibrary.Repositories
{
    public class RepositoriesGenerator<T> : IRepository<T>
        where T : class
    {
        private readonly CSContext _databaseContext;
        private readonly DbSet<T> _dbSet;

        public RepositoriesGenerator(CSContext dbContext)
        {
            _databaseContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        private static IQueryable<T> GetQueryable(DbSet<T> dbSet)
            => dbSet.AsNoTracking();   

        public async Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter)
        {
            var query = GetQueryable(_dbSet);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);

        public async Task<int> AddAsync(T entity)
        {
            var e = await _dbSet.AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
            int newId = int.Parse(e.Entity.GetType().GetProperty("Id").GetValue(e.Entity).ToString());
            return newId;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var entity = GetEntityById(id);
                if (entity is null) return false;

                //UpdateModifyDate(entity);
                ((IDeletedEntity)entity).IsDeleted = true;
                _dbSet.Update(entity);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch
            {
            }
            return false;
        }

        private T GetEntityById(int id)
        {
            var parameters = Expression.Parameter(typeof(T));
            var key = Expression.Property(parameters, "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(key, value);
            var findId = Expression.Lambda<Func<T, bool>>(equal, parameters);

            return GetQueryable(_dbSet).FirstOrDefault(findId);
        }

        private T GetEntityById(T entity)
        {
            var parameters = Expression.Parameter(typeof(T));
            var key = Expression.Property(parameters, "Id");
            var value = Expression.Constant(((IIdEntity)entity).Id);
            var equal = Expression.Equal(key, value);
            var findId = Expression.Lambda<Func<T, bool>>(equal, parameters);

            return GetQueryable(_dbSet).FirstOrDefault(findId);
        }

        public async Task<int> UpdateAsync (T entity)
        {
            //UpdateModifyDate(entity);
            var checkEntity = GetEntityById(entity);
            if (checkEntity is null) return default;
            _dbSet.Update(entity);
            await _databaseContext.SaveChangesAsync();
            int updId = int.Parse(entity.GetType().GetProperty("Id").GetValue(entity).ToString());
            return updId;
        }

        public static IRepository<T> Create(CSContext databaseContext)
        {
            return new RepositoriesGenerator<T>(databaseContext);
        }

        //private static void UpdateModifyDate(T entity)
        //{
        //   ((IModifyDateEntity)entity).ModifyDate = DateTime.Now;
        //}
    }
}

