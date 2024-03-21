using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [StringLength(500, ErrorMessage = "You Message Should Be Less Than 500 Characters")]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required")]
        public string FeedbackMessage { get; set; } = null!;
        public int? PassengerIdId { get; set; }
        public int? TripIdId { get; set; }
        public string Timestamp { get; set; } = null!;
        public bool IsReadeds { get; set; }
        [Range(0,10)]
        public int? Mark {  get; set; }

        public virtual Passenger? PassengerId { get; set; }
        public virtual Trip? TripId { get; set; }
    }
}
