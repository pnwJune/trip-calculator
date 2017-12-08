using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripCalculator.Models
{
    public class Trip : INotifyPropertyChanged
    {
        public const string DefaultTripName = "My New Trip";

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public ObservableCollection<Traveler> Travelers { get; private set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public Trip()
        {
            Travelers = new ObservableCollection<Traveler>();
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
            var match = Travelers.ToList().Find(t => t.Name == name);
            if(match != null)
                Travelers.Remove(match);

            return match != null;
        }
    }
}
