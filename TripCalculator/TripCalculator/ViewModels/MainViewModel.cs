using System;
using System.Collections.Generic;
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

    }

    class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        enum TabViews
        {
            ModeSelect,
            InitializeNewTripView,
            TripView
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
            get
            {
                return _startNewTripCommand;
            }
        }

        public MainViewModel()
        {
            _loadExistingTripCommand = new Command(DoLoadExistingTripCommand, VerifyParameterIsValidPath);
            _startNewTripCommand = new Command(DoStartNewTripCommand, new Func<object, bool>((obj) => { return true; }));
            CurrentTabViewIndex = (int)TabViews.ModeSelect;
        }

        private void DoStartNewTripCommand(object obj)
        {
            CurrentTrip = new Trip();
            CurrentTabViewIndex = (int)TabViews.InitializeNewTripView;
        }

        private void DoLoadExistingTripCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool VerifyParameterIsValidPath(object arg)
        {
            return true;
        }
    }

}
