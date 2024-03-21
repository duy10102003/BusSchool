using System;
using System.Collections.Generic;

namespace BusSchool.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Method { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
