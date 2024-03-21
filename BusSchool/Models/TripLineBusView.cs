using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class TripLineBusView
    {
        public Trip Trip { get; set; }

        public List<bus> Buses { get; set; }

        public List<Line> Lines { get; set; }
    }
}