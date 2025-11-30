using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Core.Entities
{
    public class FavoriteBook
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
