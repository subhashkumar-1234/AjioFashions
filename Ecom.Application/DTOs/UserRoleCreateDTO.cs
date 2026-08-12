using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs
{
    public class UserRoleCreateDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
     

    }
    public class UserRoleUpdateDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
      
    }
}
