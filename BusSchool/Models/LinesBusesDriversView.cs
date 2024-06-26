﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class LinesBusesDriversView
    {
        public IEnumerable<Driver> Drivers { get; set; }

        public IEnumerable<Trip> Trips { get; set; }

        public IEnumerable<bus> Buses { get; set; }

        public ContactUsForm ContactUsForm { get; set; }
    }
}