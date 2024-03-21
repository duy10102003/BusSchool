using System;
using System.Collections.Generic;

namespace BusSchool.Models
{
    public partial class ContactUsForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string TimeStamp { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsReaded { get; set; }
    }
}
