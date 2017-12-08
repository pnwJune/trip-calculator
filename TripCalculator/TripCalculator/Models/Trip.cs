using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripCalculator.Models
{
    public class Trip
    {
        public const string DefaultTripName = "My New Trip";

        public List<Traveler> Travelers { get; private set; }
        public string Name { get; set; }

        public Trip()
        {
            Travelers = new List<Traveler>();
            Name = DefaultTripName;
        }

        public Trip(string name) : this()
        {
            Name = name;
        }

        public void AddTraveler(string name)
        {
            Travelers.Add(new Traveler(name));
        }

        public bool DeleteTraveler(string name)
        {
            var match = Travelers.Find(t => t.Name == name);
            if(match != null)
                Travelers.Remove(match);

            return match != null;
        }
    }
}
