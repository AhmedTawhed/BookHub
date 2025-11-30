using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Core.DTOs.ReviewDto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
