using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs
{
    public class UserCreateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
       
    }



    public class UserUpdateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
      
    }
}
