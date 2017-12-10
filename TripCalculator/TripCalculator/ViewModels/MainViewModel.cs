using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripCalculator.Models;

namespace TripCalculator.ViewModels
{
    /// <summary>
    /// A simple contract interface used to bind the viewmodel with Unity
    /// </summary>
    public interface IMainViewModel
    {
        Command StartNewTripCommand { get; }
        Command LoadExistingTripCommand { get; }
        Command ModifyTabViewIndexCommand { get; }
        Command AddExpenseCommand { get; }
    }

    public class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        public const string IncrementStringCode = "incr";
        public const string DecrementStringCode = "decr";

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public enum TabViews
        {
            ModeSelect,
            InitializeNewTripView,
            TripView,
            OutputView
        }

        public string Title
        {
            get
            {
                var title = $"Trip Calculator";
                if (CurrentTrip != null)
                    title += $" - {CurrentTrip.Name}";

                return title;
            }
        }

        private string _expenseValueToAdd;
        public string ExpenseValueToAdd
        {
            get
            {
                return _expenseValueToAdd;
            }
            set
            {
                _expenseValueToAdd = value;
                NotifyPropertyChanged("ExpenseValueToAdd");
                AddExpenseCommand.RaiseCanExecuteChanged();
            }
        }

        private string _travelerToAddExpense;
        public string TravelerToAddExpense
        {
            get
            {
                return _travelerToAddExpense;
            }
            set
            {
                _travelerToAddExpense = value;
                NotifyPropertyChanged("TravelerToAddExpense");
            }
        }

        private int _CurrentTabViewIndex;
        public int CurrentTabViewIndex
        {
            get
            {
                return _CurrentTabViewIndex;
            }
            set
            {
                _CurrentTabViewIndex = value;
                NotifyPropertyChanged("CurrentTabViewIndex");
            }
        }

        private Trip _currentTrip;
        public Trip CurrentTrip
        {
            get
            {
                return _currentTrip;
            }
            set
            {
                _currentTrip = value;
                NotifyPropertyChanged("CurrentTrip");
            }
        }

        public ObservableCollection<string> OutputPaymentsList { get; set; }

        private Command _loadExistingTripCommand;
        public Command LoadExistingTripCommand
        {
            get
            {
                return _loadExistingTripCommand;
            }
        }

        private Command _startNewTripCommand;
        public Command StartNewTripCommand
        {
            get { return _startNewTripCommand; }
        }

        private Command _modifyTabViewIndexCommand;
        public Command ModifyTabViewIndexCommand
        {
            get { return _modifyTabViewIndexCommand; }
        }

        private Command _AddExpenseCommand;
        public Command AddExpenseCommand
        {
            get { return _AddExpenseCommand; }
        }

        public MainViewModel()
        {
            OutputPaymentsList = new ObservableCollection<string>();

            _loadExistingTripCommand = new Command(DoLoadExistingTripCommand, 
                VerifyParameterIsValidPath);
            _startNewTripCommand = new Command(DoStartNewTripCommand, 
                new Func<object, bool>((obj) => { return true; }));
            _modifyTabViewIndexCommand = new Command(DoModifyTabViewIndex, 
                VerifyParameterIsValidModifierAndCanModifyTabViewIndex);
            _AddExpenseCommand = new Command(DoAddExpenseCommand,
                VerifyExpenseToAddIsValidDouble);

            CurrentTabViewIndex = (int)TabViews.ModeSelect;
        }

        /// <summary>
        /// Checks that the expense and traveler to add the expense
        /// are valid for command execution
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool VerifyExpenseToAddIsValidDouble(object arg)
        {
            double value = 0.0;
            if(string.IsNullOrEmpty(TravelerToAddExpense) ||
                Double.TryParse(ExpenseValueToAdd, out value) == false || 
                value <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks that the parameter is valid string and that
        /// the current tab view allows for either incrementing
        /// or decrementing to a new view
        /// </summary>
        /// <param name="arg">should be a string to represent increment
        /// or decrementing the tabView index</param>
        /// <returns></returns>
        private bool VerifyParameterIsValidModifierAndCanModifyTabViewIndex(object arg)
        {
            if (CurrentTabViewIndex < (int)TabViews.InitializeNewTripView || 
                CurrentTabViewIndex > (int)TabViews.OutputView)
                return false;

            if ((arg.ToString() == IncrementStringCode ||
                arg.ToString() == DecrementStringCode) == false)
                return false;

            bool incr = arg.ToString() == IncrementStringCode;

            return incr ? CurrentTabViewIndex >= (int)TabViews.InitializeNewTripView &&
                CurrentTabViewIndex < (int) TabViews.OutputView :
                CurrentTabViewIndex <= (int)TabViews.OutputView && 
                CurrentTabViewIndex > (int)TabViews.InitializeNewTripView;
        }

        /// <summary>
        /// Creates a new trip and sets the CurrentTrip to this newly initialized trip
        /// </summary>
        /// <param name="obj"></param>
        private void DoStartNewTripCommand(object obj)
        {
            CurrentTrip = new Trip();
            CurrentTabViewIndex = (int)TabViews.InitializeNewTripView;
            NotifyPropertyChanged("Title");
            ModifyTabViewIndexCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Executes either incrementing or decrementing the current selected
        /// tab view index
        /// </summary>
        /// <param name="obj"></param>
        private void DoModifyTabViewIndex(object obj)
        {
            switch(obj.ToString())
            {
                case IncrementStringCode:
                    CurrentTabViewIndex++;
                    break;
                case
                    DecrementStringCode:
                    CurrentTabViewIndex--;
                    break;
            }
            NotifyPropertyChanged("Title");
            ModifyTabViewIndexCommand.RaiseCanExecuteChanged();

            if (CurrentTabViewIndex == (int)TabViews.OutputView)
                GenerateOutputStrings();
        }

        /// <summary>
        /// Adds an expense to the current trip
        /// </summary>
        /// <param name="obj"></param>
        private void DoAddExpenseCommand(object obj)
        {
            CurrentTrip.AddExpense(TravelerToAddExpense,
                double.Parse(ExpenseValueToAdd));
        }

        /// <summary>
        /// Populates the viewmodel with the string output of
        /// every person's reimbursement to the maximum payer(s) on
        /// the current trip
        /// </summary>
        private void GenerateOutputStrings()
        {
            OutputPaymentsList.Clear();

            if (_currentTrip == null)
                return;

            var mapping = CurrentTrip.GetMapTravelersToReimbursements();

            foreach(var kvp in mapping)
            {
                var traveler = kvp.Key;
                var payments = kvp.Value;

                foreach(var payment in payments)
                {
                    var resultString = $"{traveler.Name} owes {payment.Payee.Name} ${String.Format("{0:0.00}", payment.Value)}";
                    OutputPaymentsList.Add(resultString);
                }
            }
        }

        /// <summary>
        /// TODO: implement this for the load existing trip command
        /// </summary>
        /// <param name="obj"></param>
        private void DoLoadExistingTripCommand(object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: implement this for the load existing trip command
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool VerifyParameterIsValidPath(object arg)
        {
            return true;
        }
    }

}
