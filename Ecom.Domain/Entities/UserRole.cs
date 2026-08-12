using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
