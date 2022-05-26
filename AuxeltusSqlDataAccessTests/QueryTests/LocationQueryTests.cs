using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuxeltusSqlDataAccessTests
{
    [TestClass]
    public class LocationQueryTests
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public ILocationQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = TestDataSetup.GenerateTestDataContext();
            testLogger = NullLogger.Instance;
            query = new LocationQuery(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        public async Task RetrieveLocationsAsync_GetAll()
        {
            List<Location> locations = await query.RetrieveLocationsAsync(100, 0);

            Assert.AreEqual(TestDataSetup.Locations.Count, locations.Count);

            foreach (Location location in locations)
            {
                Location matchedLocation = TestDataSetup.Locations.First(l => l.Id == location.Id);
                Assert.AreEqual(matchedLocation.Name, location.Name);
                Assert.AreEqual(matchedLocation.Longitude, location.Longitude);
                Assert.AreEqual(matchedLocation.Latitude, location.Latitude);
            }
        }

        [TestMethod]
        public async Task RetrieveLocationsAsync_GetSubset_UpToMaxReturns()
        {
            int maxReturns = 2;

            List<Location> locations = await query.RetrieveLocationsAsync(maxReturns, 0);

            Assert.AreEqual(maxReturns, locations.Count);

            foreach (Location location in locations)
            {
                Location matchedLocation = TestDataSetup.Locations.First(l => l.Id == location.Id);
                Assert.AreEqual(matchedLocation.Name, location.Name);
                Assert.AreEqual(matchedLocation.Longitude, location.Longitude);
                Assert.AreEqual(matchedLocation.Latitude, location.Latitude);
            }
        }

        [TestMethod]
        public async Task RetrieveLocationsAsync_GetSubset_OnlyAfterStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 2;

            List<Location> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(TestDataSetup.Locations.Count - (startIndex - 1), locations.Count);

            foreach (Location location in locations)
            {
                Location matchedLocation = TestDataSetup.Locations.First(l => l.Id == location.Id);
                Assert.AreEqual(matchedLocation.Name, location.Name);
                Assert.AreEqual(matchedLocation.Longitude, location.Longitude);
                Assert.AreEqual(matchedLocation.Latitude, location.Latitude);
            }
        }

        [TestMethod]
        public async Task RetrieveLocationsAsync_GetNothing_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<Location> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, locations.Count);
        }

        [TestMethod]
        public async Task RetrieveLocationsAsync_GetNothing_NoDataOnTable()
        {
            context.Jobs.RemoveRange(TestDataSetup.Jobs);
            context.SaveChanges();
            context.Locations.RemoveRange(TestDataSetup.Locations);
            context.SaveChanges();

            int maxReturns = 100;
            int startIndex = 0;
            List<Location> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, locations.Count);
        }

    }
}
