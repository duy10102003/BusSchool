using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusSchool.Models
{
    public partial class Driver
    {
        public Driver()
        {
            buses = new HashSet<bus>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Driver Name")]
        [StringLength(100, MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Name in Text Pattern Only")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Identity Number")]
        [StringLength(10, MinimumLength = 10)]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Identity Number in Text Pattern Only")]
        public string Ssn { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Phone Number")]
        [StringLength(10, ErrorMessage = "Enter Valid Phone Number", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "This Field Is Required, Enter Your Phone Number in Phone Pattern Only")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        [EmailAddress(ErrorMessage = "Enter a Valid Email Address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This Field Is Required, Enter Your Email Address.")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "This Field Is Required")]
        public string? ProfilePicture { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [Display(Name = "Driver Licence")]
        [DataType(DataType.Text, ErrorMessage = "This Field Is Required, Enter Your Driving Licence Number in Text Pattern Only")]
        public string DriverLicence { get; set; } = null!;
        public bool? IsAvailable { get; set; }

        public virtual ICollection<bus> buses { get; set; }
    }
}
