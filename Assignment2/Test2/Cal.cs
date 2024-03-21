using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    public class Cal
    {
        public double Sum { get; set; }
        public double Aver { get; set; }
        public Cal() { }

        public Cal(double sum, double aver)
        {
            Sum = sum;
            Aver = aver;
        }

        public override string? ToString()
        {
            return "SUM= " + Sum +", "+"Average= "+ Aver;
        }
    }
}
