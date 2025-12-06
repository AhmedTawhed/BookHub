using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace BookHub.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly BookHubDbContext _context;
        private readonly DbSet<T> _dbSet;


        public Repository(BookHubDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();
        public async Task<T?> GetById(int id) => await _dbSet.FindAsync(id);
        public async Task Add(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();
        public async Task<PagedList<T>> GetPage(GridRequest request, Expression<Func<T, bool>>? filter = null)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var text = request.SearchText.Trim().ToLower();

                var stringProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                if (stringProperties.Any())
                {
                    var searchQuery = string.Join(" OR ",
                        stringProperties.Select(p => $"({p.Name} != null && {p.Name}.ToLower().Contains(@0))"));
                    query = query.Where(searchQuery, text);
                }
            }

            var validColumns = typeof(T).GetProperties().Select(p => p.Name).ToList();

            if (!string.IsNullOrEmpty(request.SortColumn) &&
                validColumns.Contains(request.SortColumn))
            {
                var direction = request.SortDirection?.ToLower() == "desc" ? "desc" : "asc";
                query = query.OrderBy($"{request.SortColumn} {direction}");
            }
           
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(request.Skip < 0 ? 0 : request.Skip)
                .Take(request.Take <= 0 ? 10 : Math.Min(request.Take, 100))
                .ToListAsync();

            return new PagedList<T>
            {
                Items = items,
                TotalCount = totalCount,
                NumberOfPages = (int)Math.Ceiling((double)totalCount / request.Take)
            };
        }
    }
}
