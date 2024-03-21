using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class SeatPassengerView
    {
        public List<Passenger> Passengers { get; set; }

        public List<User> Users { get; set; }

        public Seat Seat { get; set; }
    }
}