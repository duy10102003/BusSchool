using System;
using System.Collections.Generic;

namespace BusSchool.Models
{
    public partial class Passenger
    {
        public Passenger()
        {
            Feedbacks = new HashSet<Feedback>();
            Seats = new HashSet<Seat>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public bool Blocked { get; set; }

        public virtual User ApplicationUser { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
