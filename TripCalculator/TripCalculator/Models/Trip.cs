using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripCalculator.Models
{
    public class Trip
    {
        public List<Traveler> Travelers { get; set; }

        public Trip()
        {
            Travelers = new List<Traveler>();
        }

        public void AddTraveler(string name)
        {
            Travelers.Add(new Traveler(name));
        }

        
    }
}
