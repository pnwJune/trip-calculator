using System;
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

        /// <summary>
        /// Always returns to 2 decimal places
        /// </summary>
        public double Total
        {
            get
            {
                double sum = 0;
                Expenses.ToList().ForEach(e => sum += e);
                return Math.Round(sum, 2);
            }
        }

        public Traveler(string name)
        {
            Expenses = new ObservableCollection<double>();
            Name = name;
        }
    }
}