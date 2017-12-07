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
    }
}
