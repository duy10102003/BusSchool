using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public class User
    {
        public User()
        {
        }

        public User(string code, string name, double salary, double bonus)
        {
            Code = code;
            Name = name;
            Salary = salary;
            Bonus = bonus;
            
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public double Income() { 
            return Salary+Bonus;
        }

        public override string? ToString()
        {
            return Code + "|" + Name + "|" + Salary + "|" + Bonus+ "|"+ Income();
        }
    }
}
