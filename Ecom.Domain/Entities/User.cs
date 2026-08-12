using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
