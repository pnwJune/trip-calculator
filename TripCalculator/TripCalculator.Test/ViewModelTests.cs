using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripCalculator.Models;
using TripCalculator.ViewModels;

namespace TripCalculator.Test
{
    [TestFixture]
    public class ViewModelTests
    {
        /// <summary>
        /// Tests viewmodel initialization
        /// </summary>
        [Test]
        public void MainViewModelCanInitialize()
        {
            var vm = new MainViewModel();
            Assert.IsNotNull(vm);
            Assert.IsNull(vm.CurrentTrip);
        }

        /// <summary>
        /// Tests that the mainviewmodel can start a new trip
        /// </summary>
        [Test]
        public void MainViewModelVerifyStartNewTripCommand()
        {
            var vm = new MainViewModel();
            Assert.IsTrue(vm.StartNewTripCommand.CanExecute(null));
            vm.StartNewTripCommand.Execute(null);
            Assert.IsNotNull(vm.CurrentTrip);

            Assert.AreEqual(Trip.DefaultTripName, vm.CurrentTrip.Name);
            Assert.IsEmpty(vm.CurrentTrip.Travelers);
        }

        /// <summary>
        /// Tests all the logic the mainviewmodel has when cycling 
        /// between TabViews, including generating output when showing the
        /// output tab
        /// </summary>
        [Test]
        public void MainViewModelVerifyModifyTabViewCommand()
        {
            // verify what kinds of parameters work for the command
            var vm = new MainViewModel();
            Assert.AreEqual((int)MainViewModel.TabViews.ModeSelect, vm.CurrentTabViewIndex);
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute(null));
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute("foo"));

            // verify that the command rejects incrementing/decrementing when in "Mode Select"
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.IncrementStringCode));
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.DecrementStringCode));

            // initialize the current tab view to the starting value
            vm.CurrentTabViewIndex = (int)MainViewModel.TabViews.InitializeNewTripView;
            Assert.AreEqual((int)MainViewModel.TabViews.InitializeNewTripView, vm.CurrentTabViewIndex);

            // verify the index can increment but not decrement
            Assert.IsTrue(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.IncrementStringCode));
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.DecrementStringCode));
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.IncrementStringCode);
            Assert.AreEqual((int)MainViewModel.TabViews.TripView, vm.CurrentTabViewIndex);

            // Verify the index can both incr/decr
            Assert.IsTrue(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.IncrementStringCode));
            Assert.IsTrue(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.DecrementStringCode));
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.IncrementStringCode);
            Assert.AreEqual((int)MainViewModel.TabViews.OutputView, vm.CurrentTabViewIndex);

            // verify that no output is produced as there is no currentTrip
            Assert.IsEmpty(vm.OutputPaymentsList);

            // verify we can decrement but not increment
            Assert.IsFalse(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.IncrementStringCode));
            Assert.IsTrue(vm.ModifyTabViewIndexCommand.CanExecute(MainViewModel.DecrementStringCode));
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.DecrementStringCode);
            Assert.AreEqual((int)MainViewModel.TabViews.TripView, vm.CurrentTabViewIndex);

            // startnewtrip resets the currentTabViewIndex to TabViews.InitializeNewTripView
            vm.StartNewTripCommand.Execute(null);
            vm.CurrentTrip.AddTraveler("foo");
            vm.CurrentTrip.AddTraveler("bar");
            vm.CurrentTrip.AddExpense("foo", 10);

            // verify that incrementing to the final page generates the output list correctly
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.IncrementStringCode);
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.IncrementStringCode);
            Assert.IsNotEmpty(vm.OutputPaymentsList);

            // verify that the output is cleared and re-generated each time
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.DecrementStringCode);
            vm.CurrentTrip.AddExpense("bar", 10);
            vm.ModifyTabViewIndexCommand.Execute(MainViewModel.IncrementStringCode);
            Assert.IsEmpty(vm.OutputPaymentsList);
        }

        /// <summary>
        /// Tests the mainviewmodel can add expenses to a traveler
        /// and that it verifies different inputs before doing so
        /// </summary>
        [Test]
        public void MainViewModelVerifyAddExpenseCommand()
        {
            var vm = new MainViewModel();
            vm.StartNewTripCommand.Execute(null);
            vm.CurrentTrip.AddTraveler("foo");

            // verify the command checks the expense value when
            // a traveler is selected
            vm.TravelerToAddExpense = "foo";
            vm.ExpenseValueToAdd = null;
            Assert.IsFalse(vm.AddExpenseCommand.CanExecute(null));
            vm.ExpenseValueToAdd = "foo";
            Assert.IsFalse(vm.AddExpenseCommand.CanExecute(null));
            vm.ExpenseValueToAdd = "0";
            Assert.IsFalse(vm.AddExpenseCommand.CanExecute(null));
            vm.ExpenseValueToAdd = "-33.4";
            Assert.IsFalse(vm.AddExpenseCommand.CanExecute(null));

            vm.ExpenseValueToAdd = "10";
            Assert.IsTrue(vm.AddExpenseCommand.CanExecute(null));

            // verify the command fails when no traveler is selected
            vm.TravelerToAddExpense = null;
            Assert.IsFalse(vm.AddExpenseCommand.CanExecute(null));

            vm.TravelerToAddExpense = "foo";
            vm.AddExpenseCommand.Execute(null);

            Assert.AreEqual(10, vm.CurrentTrip.GetTraveler("foo").Total);
        }

        /// <summary>
        /// TODO: implement this functionality
        /// </summary>
        [Explicit]
        public void MainViewModelCanLoadExistingTrip()
        {
            var vm = new MainViewModel();
            vm.LoadExistingTripCommand.Execute(null); // throws exception
        }
    }
}
