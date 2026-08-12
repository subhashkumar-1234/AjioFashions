using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs.AllItemDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
