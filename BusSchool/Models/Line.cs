using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Line
    {
        public Line()
        {
            Trips = new HashSet<Trip>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "From")]
        [StringLength(100)]
        [DataType(DataType.Text, ErrorMessage = "Should be in text format")]
        public string From { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "To")]
        [StringLength(100)]
        [DataType(DataType.Text, ErrorMessage = "Should be in text format")]
        public string To { get; set; } = null!;

        [Required]
        [Display(Name = "Line Price")]
        [DataType(DataType.Currency, ErrorMessage = "Should be in Currency format")]
        public double Price { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "To")]
        [StringLength(100)]
        [DataType(DataType.Text, ErrorMessage = "Should be in text format")]
        public string? LinePicture { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}
