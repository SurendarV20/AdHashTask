using EBay.API.Infrastructure;
using EBay.Domain.Entities.Base;
using EBay.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EBay.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext _dbContext { get; set; }
        protected DbSet<T> entities;
        public IApplicationDbContextFactory ContextFactory { get; set; }
        protected DbContext DbContext => _dbContext ??= ContextFactory.Get();

        public Repository(IApplicationDbContextFactory dbDbContextFactory)
        {
            ContextFactory = dbDbContextFactory;

            entities = DbContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return entities.AsQueryable();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter)
        {
            return entities.Where(filter);
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            return entities.Where(filter);
        }

        public Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            var result = entities.Where(filter);
            return Task.FromResult((IEnumerable<T>)result.AsAsyncEnumerable());
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return entities.Add(entity).Entity;
        }

        public async Task<T> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var result = await entities.AddAsync(entity);
            return result.Entity;
        }

        public void InsertBulk(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            this.entities.AddRange(entities);
        }

        public async Task InsertBulkAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            await this.entities.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entities.Update(entity);
        }

        public void UpdateBulk(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            this.entities.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                entities.Attach(entity);
            }
            entities.Remove(entity);
        }

        public void DeleteBulk(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            this.entities.RemoveRange(entities);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            return await DbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public IEnumerable<T> FromSqlRaw(string sql, object[] parameters)
        {
            return entities.FromSqlRaw(sql, parameters);
        }

        Task<int> IRepository<T>.DeleteAsync(Expression<Func<T, bool>> condition)
        {
            throw new NotImplementedException();
        }


    }
}