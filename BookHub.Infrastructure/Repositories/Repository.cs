using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
