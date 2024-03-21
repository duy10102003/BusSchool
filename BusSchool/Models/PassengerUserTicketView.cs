using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class PassengerUserTicketView
    {
        public Passenger Passenger { get; set; }

        public List<Ticket> Tickets { get; set; }

        public List<Line> Lines { get; set; }

        public List<Trip> Trips { get; set; }

        public List<bus> Buses { get; set; }

        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}