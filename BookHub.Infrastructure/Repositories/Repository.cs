using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
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
        ///////
        public async Task<IEnumerable<T>> GetPage(GridRequest request, Expression<Func<T, bool>>? filter = null)
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
                    var searchQuery = string.Join(" || ",
                        stringProperties.Select(p => $"({p.Name} != null && {p.Name}.ToLower().Contains(@0))"));
                    query = query.Where(searchQuery, text);
                }
            }
            // Sorting
            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                var direction = request.SortDirection?.ToLower() == "desc" ? "desc" : "asc";
                query = query.OrderBy($"{request.SortColumn} {direction}");
            }

            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync();

            return items;
        }
    }
}
