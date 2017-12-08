using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TripCalculator.Models
{
    public class Traveler
    {
        public string Name { get; set; }

        public ObservableCollection<double> Expenses { get; set; }

        public double Total
        {
            get
            {
                double sum = 0;
                Expenses.ToList().ForEach(e => sum += e);
                return sum;
            }
        }

        public Traveler(string name)
        {
            Expenses = new ObservableCollection<double>();
            Name = name;
        }
    }
}