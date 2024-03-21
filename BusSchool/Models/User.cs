using System;
using System.Collections.Generic;

namespace BusSchool.Models
{
    public partial class User
    {
        public User()
        {
            Passengers = new HashSet<Passenger>();
        }

        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; } = null!;
        public string Ssn { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Passenger> Passengers { get; set; }
    }
}
