using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Trip
    {
        public Trip()
        {
            Feedbacks = new HashSet<Feedback>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public string? Time { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int BusId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int LineId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public string? TripPicture { get; set; }

        public virtual bus Bus { get; set; } = null!;
        public virtual Line Line { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
