using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripCalculator.Models;

namespace TripCalculator.Test
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void TripCanInitialize()
        {
            var trip = new Trip();
            Assert.IsNotNull(trip);
            Assert.IsEmpty(trip.Travelers);
        }

        [Test]
        public void TripCanName()
        {
            var trip = new Trip("hello world");
            Assert.AreEqual("hello world", trip.Name);
        }

        [Test]
        public void TripCanAddTraveler()
        {
            var trip = new Trip();
            trip.AddTraveler("foo");
            Assert.AreEqual(1, trip.Travelers.Count);
            Assert.AreEqual("foo", trip.Travelers.First().Name);
            Assert.IsEmpty(trip.Travelers.First().Expenses);
            Assert.AreEqual(0, trip.Travelers.First().Total);
        }

        [Test]
        public void TripCanDeleteTraveler()
        {
            var trip = new Trip();
            trip.AddTraveler("Deleted");
            Assert.IsTrue(trip.DeleteTraveler("Deleted"));
            Assert.IsEmpty(trip.Travelers);
        }

        [Test]
        public void TripCanAddExpensesToTraveler()
        {
            var trip = new Trip();
            trip.AddTraveler("foo");
            Assert.IsTrue(trip.AddExpense("foo", 12.34));
            Assert.IsFalse(trip.AddExpense("foo", 0.00));
            Assert.IsFalse(trip.AddExpense("foo", -0.09));
        }
        
        [Test]
        public void TripCanGetTraveler()
        {
            var trip = new Trip();
            trip.AddTraveler("foo");
            trip.AddTraveler("bar");
            trip.AddTraveler("baz");

            Assert.AreEqual(trip.Travelers.Last(), trip.GetTraveler("baz"));
            Assert.AreEqual(trip.Travelers.First(), trip.GetIthTraveler(0));

        }

        [Test]
        public void TripCanCalculateAverageOfAllExpenses()
        {
            var trip = GenerateBasicTrip();
            
            Assert.AreEqual(53.54, trip.GetTraveler("foo").Total);

            Assert.AreEqual(50.23, trip.GetIthTraveler(1).Total);
            
            Assert.AreEqual(113.41, trip.GetTraveler("baz").Total);

            Assert.AreEqual(217.18, trip.GetTotalExpenses());
            Assert.AreEqual(72.39, Math.Round(trip.GetIndividualEqualShare(), 2));
        }

        [Test]
        public void TravelerCanGetReimbursementsForOneMaxPayer()
        {
            var trip = GenerateBasicTrip();

            var map = trip.GetMapTravelersToReimbursements();

            Assert.AreEqual(2, map.Count);
            Assert.IsTrue(map.ContainsKey(trip.GetTraveler("foo")));
            Assert.IsTrue(map.ContainsKey(trip.GetTraveler("bar")));
            Assert.IsFalse(map.ContainsKey(trip.GetIthTraveler(2)));

            var fooReimbursements = map[trip.GetTraveler("foo")];
            var barReimbursements = map[trip.GetTraveler("bar")];

            Assert.IsTrue(fooReimbursements.Count == 1);
            Assert.IsTrue(barReimbursements.Count == 1);
            Assert.AreEqual(18.85, fooReimbursements[0].Value);
            Assert.AreEqual("baz", fooReimbursements[0].Payee.Name);
            Assert.AreEqual(22.16, barReimbursements[0].Value);
            Assert.AreEqual("baz", barReimbursements[0].Payee.Name);
        }

        [Test]
        public void TravelerCanGetReimbursementsForMultipleMaxPayer()
        {
            var trip = GenerateBasicTrip();
            trip.AddTraveler("qux");
            trip.AddExpense("qux", 113.41);
            var map = trip.GetMapTravelersToReimbursements();

            Assert.AreEqual(2, map.Count);
            Assert.IsTrue(map.ContainsKey(trip.GetTraveler("foo")));
            Assert.IsTrue(map.ContainsKey(trip.GetTraveler("bar")));
            Assert.IsFalse(map.ContainsKey(trip.GetIthTraveler(2)));

            var fooReimbursements = map[trip.GetTraveler("foo")];
            var barReimbursements = map[trip.GetTraveler("bar")];

            Assert.IsTrue(fooReimbursements.Count == 2);
            Assert.IsTrue(barReimbursements.Count == 2);
            
            Assert.AreEqual(14.55, fooReimbursements[0].Value);
            Assert.AreEqual(14.55, fooReimbursements[1].Value);
            Assert.AreEqual("baz", fooReimbursements[0].Payee.Name);
            Assert.AreEqual("qux", fooReimbursements[1].Payee.Name);
            Assert.AreEqual(16.21, barReimbursements[0].Value);
            Assert.AreEqual(16.21, barReimbursements[1].Value);
            Assert.AreEqual("baz", barReimbursements[0].Payee.Name);
            Assert.AreEqual("qux", barReimbursements[1].Payee.Name);
        }

        private Trip GenerateBasicTrip()
        {
            var trip = new Trip();
            trip.AddTraveler("foo");
            trip.AddTraveler("bar");
            trip.AddTraveler("baz");

            // $5.75, $35.00, and $12.79
            trip.AddExpense("foo", 5.75);
            trip.AddExpense("foo", 35);
            trip.AddExpense("foo", 12.79);

            // $12.00, $15.00, and $23.23
            trip.AddExpense("bar", 12.00);
            trip.AddExpense("bar", 15);
            trip.AddExpense("bar", 23.23);

            // $10.00, $20.00, $38.41, and $45.00
            trip.AddExpense("baz", 10);
            trip.AddExpense("baz", 20);
            trip.AddExpense("baz", 38.41);
            trip.AddExpense("baz", 45);

            return trip;
        }
    }
}
