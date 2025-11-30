using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Core.DTOs.FavoriteBookDto
{
    public class FavoriteBookDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

}
