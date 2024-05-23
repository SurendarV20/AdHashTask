using EBay.Domain.Entities.Base;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace EBay.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> Get(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);

        T Insert(T entity);

        Task<T> InsertAsync(T entity);

        void InsertBulk(IEnumerable<T> entities);

        Task InsertBulkAsync(IEnumerable<T> entities);


        void Update(T entity);

        void UpdateBulk(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteBulk(IEnumerable<T> entities);
        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<SqlParameter> parameters);
        Task<int> DeleteAsync(Expression<Func<T, bool>> condition);
        IQueryable<T> GetAllQueryable();
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter);
        IEnumerable<T> FromSqlRaw(string sql, object[] parameters);
    }

}
