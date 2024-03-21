using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class bus
    {
        public bus()
        {
            Seats = new HashSet<Seat>();
            Trips = new HashSet<Trip>();
        }

        public int Id { get; set; }

        [Required] 
        public string Color { get; set; } = null!;

        [Required]
        public string BusNumber { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Maximum Seats Number")]
        [Range(10, 100)]
        public int MaximumSeats { get; set; }
        public int? DriverId { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        public string? BusPicture { get; set; }

        public virtual Driver? Driver { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}
