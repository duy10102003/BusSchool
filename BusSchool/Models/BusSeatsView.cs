using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class BusSeatsView
    {
        public bus Bus { get; set; }

        public List<Seat> Seats { get; set; }
        
    }
}