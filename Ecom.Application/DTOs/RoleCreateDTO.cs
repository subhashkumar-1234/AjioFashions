using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs
{
    public class RoleCreateDTO
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
       
    }

    public class RoleUpdateDTO
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
       
    }
}
