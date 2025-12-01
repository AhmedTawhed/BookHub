using BookHub.Core.Entities;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BookHubDbContext context) : base(context)
        {
        }
    }
}
