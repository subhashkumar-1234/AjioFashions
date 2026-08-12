using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs.AllItemDtos
{
    public class AddQuantityDto
    {
        public int Id { get; set; }
        public int AddItemId { get; set; }
        public int Quantity { get; set; }
        public int PreviousStock { get; set; }
        public int CurrentStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateQuantityDto
    {
        public int Id { get; set; }
        public int AddItemId { get; set; }
        public int Quantity { get; set; }
        public int PreviousStock { get; set; }
        public int CurrentStock { get; set; }
    }
}
