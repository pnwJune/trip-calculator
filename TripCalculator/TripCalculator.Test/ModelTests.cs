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
        }
    }
}
