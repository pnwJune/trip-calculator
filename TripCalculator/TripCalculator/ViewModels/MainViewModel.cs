using System;
using System.Collections.Generic;
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

    class MainViewModel : IMainViewModel
    {
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
        }

        private void DoStartNewTripCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void DoLoadExistingTripCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool VerifyParameterIsValidPath(object arg)
        {
            throw new NotImplementedException();
        }
    }

}
