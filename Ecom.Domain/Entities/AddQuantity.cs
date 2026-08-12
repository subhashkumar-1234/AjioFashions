using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class AddQuantity
    {
        public int Id { get; set; }

        // FK
        public int AddItemId { get; set; }

        public AddItem AddItem { get; set; }

        // Added stock
        public int Quantity { get; set; }

        // Stock before update
        public int PreviousStock { get; set; }

        // Stock after update
        public int CurrentStock { get; set; }

        // Date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
