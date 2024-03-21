using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Seat
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Seat Number")]
        [Range(1, 100)]
        public int SeatNumber { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int PassengerId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public int? BusId { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        public string Time { get; set; } = null!;

        public virtual bus? Bus { get; set; }
        public virtual Passenger Passenger { get; set; } = null!;
    }
}
