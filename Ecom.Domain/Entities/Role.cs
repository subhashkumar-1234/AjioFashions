using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public required string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
