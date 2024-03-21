using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class PassengersTicketsView
    {
        public List<Passenger> Passengers { get; set; }

        public List<Ticket> Tickets { get; set; }

        public Passenger SpecificPassenger { get; set; }

        public List<bus> Buses { get; set; }

        public bus SpecificBus { get; set; }
    }
}