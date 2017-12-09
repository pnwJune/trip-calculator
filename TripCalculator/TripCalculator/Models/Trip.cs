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

        public ObservableCollection<Traveler> Travelers { get; set; }

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

        public double MaximumPaid { get; private set; }
        private List<Traveler> _maximumPayers;

        public Trip()
        {
            Travelers = new ObservableCollection<Traveler>();
            Name = DefaultTripName;
            MaximumPaid = 0;
            _maximumPayers = new List<Traveler>();
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

                if (traveler.Total > MaximumPaid)
                    _maximumPayers.Clear();
                if(traveler.Total >= MaximumPaid)
                {
                    MaximumPaid = traveler.Total;
                    _maximumPayers.Add(traveler);
                }

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

        public Dictionary<Traveler, List<Reimbursement>> GetMapTravelersToReimbursements()
        {
            var result = new Dictionary<Traveler, List<Reimbursement>>();

            // return no reimbursements if no expenses have been logged
            if (_maximumPayers.Count > 0)
            {
                var average = GetIndividualEqualShare();

                // only iterate over travelers that are not in the maximumPayers list
                var remainingTravelers = Travelers.Where(t => 
                _maximumPayers.Any(p => p.Name == t.Name) == false);
                foreach (var traveler in remainingTravelers)
                {
                    // round this value to two decimals since reimbursement is paid as cash
                    var reimburseCost = Math.Round((average - traveler.Total) / _maximumPayers.Count, 2);
                    result.Add(traveler, new List<Reimbursement>());
                    for (int i = 0; i < _maximumPayers.Count; i++)
                    {
                        result[traveler].Add(new Reimbursement()
                        {
                            Payee = _maximumPayers[i], Value = reimburseCost
                        });
                    }
                }
            }

            return result;
        }
    }
}
