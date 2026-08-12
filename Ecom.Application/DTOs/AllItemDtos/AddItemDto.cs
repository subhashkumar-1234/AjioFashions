using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs.AllItemDtos
{
    public class AddItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ItemDescription { get; set; }

        public decimal ItemPrice { get; set; } = 0;

        public int Stock { get; set; }

        public string Size { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
    public class UpdateItemDto
    {
       
        public string ProductName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; } = 0;
        public int Stock { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
