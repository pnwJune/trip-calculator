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

        public bool AddExpense(string travelerName, double expenseValue)
        {
            if (expenseValue <= 0)
                return false;
            var traveler = Travelers.ToList().Find(t => t.Name == travelerName);
            if(traveler != null)
            {
                traveler.Expenses.Add(expenseValue);
                return true;
            }
            return false;
        }

        public Traveler GetTraveler(string name)
        {
            return Travelers.ToList().Find(t => t.Name == name);
        }

        public Traveler GetIthTraveler(int index)
        {
            return index >= 0 ? Travelers.ElementAt(index) : null;
        }

        public double GetTotalExpenses()
        {
            double sum = 0;
            Travelers.ToList().ForEach(t => sum += t.Total);
            return sum;
        }

        public double GetIndividualEqualShare()
        {
            return Travelers.Count > 0 ?
                GetTotalExpenses() / Travelers.Count : 0;
        }

        public Dictionary<Traveler, Reimbursement> GetMapTravelersToReimbursements()
        {
            var result = new Dictionary<Traveler, Reimbursement>();
            var average = GetIndividualEqualShare();
            foreach(var traveler in Travelers)
            {
                var difference = average - traveler.Total;
            }
        }
    }
}
