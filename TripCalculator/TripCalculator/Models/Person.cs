using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace TripCalculator.Models
{
    public class Traveler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public string Name { get; set; }

        public ObservableCollection<double> Expenses { get; private set; }

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