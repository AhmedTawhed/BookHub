using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using System.Linq.Expressions;

namespace BookHub.Core.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        void Update(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<PagedList<T>> GetPage(GridRequest request, Expression<Func<T, bool>>? filter = null);
    }
}