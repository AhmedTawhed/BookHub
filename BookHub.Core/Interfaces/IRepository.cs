using BookHub.Core.Helpers.CustomRequests;
using System.Linq.Expressions;

namespace BookHub.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        void Update(T entity);
        Task<IEnumerable<T>> GetPage(GridRequest request, Expression<Func<T, bool>>? filter = null);
    }
}