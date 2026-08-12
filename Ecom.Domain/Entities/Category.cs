using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        // One Category -> Many AddItems
        public ICollection<AddItem> AddItems { get; set; } = new List<AddItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
