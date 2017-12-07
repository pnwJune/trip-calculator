using System.Collections.Generic;

namespace TripCalculator.Models
{
    public class Traveler
    {
        public string Name { get; set; }

        public List<double> Expenses { get; set; }

        public double Total
        {
            get
            {
                double sum = 0;
                Expenses.ForEach(e => sum += e);
                return sum;
            }
        }

        public Traveler(string name)
        {
            Expenses = new List<double>();
            Name = name;
        }
    }
}