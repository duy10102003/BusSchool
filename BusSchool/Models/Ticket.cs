using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Ticket
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public string? BookingTime { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int TripId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int PassengerId { get; set; }
        
        public bool IsBlocked { get; set; }

        public virtual Passenger Passenger { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual Trip Trip { get; set; } = null!;
    }
}
