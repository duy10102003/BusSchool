using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusSchool.Models
{
    public class BusDriverView
    {
        public bus Bus { get; set; }

        public List<Driver> Drivers { get; set; }
            
    }
}