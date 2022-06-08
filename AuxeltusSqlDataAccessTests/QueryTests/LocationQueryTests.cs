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
    public class LocationQueryTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger<LocationQuery> testLogger;
        public ILocationQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
            testLogger = NullLogger<LocationQuery>.Instance;
            query = new LocationQuery(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        public async Task RetrieveLocationsAsync_GetAll()
        {
            List<LocationEntity> locations = await query.RetrieveLocationsAsync(100, 0);

            Assert.AreEqual(Locations.Count, locations.Count);

            foreach (LocationEntity location in locations)
            {
                LocationEntity matchedLocation = Locations.First(l => l.Id == location.Id);

                CompareLocations(matchedLocation, location);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        public async Task RetrieveLocationsAsync_GetSubset_UpToMaxReturns()
        {
            int maxReturns = 2;

            List<LocationEntity> locations = await query.RetrieveLocationsAsync(maxReturns, 0);

            Assert.AreEqual(maxReturns, locations.Count);

            foreach (LocationEntity location in locations)
            {
                LocationEntity matchedLocation = Locations.First(l => l.Id == location.Id);

                CompareLocations(matchedLocation, location);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        public async Task RetrieveLocationsAsync_GetSubset_OnlyAfterStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 2;

            List<LocationEntity> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(Locations.Count - (startIndex - 1), locations.Count);

            foreach (LocationEntity location in locations)
            {
                LocationEntity matchedLocation = Locations.First(l => l.Id == location.Id);

                CompareLocations(matchedLocation, location);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        public async Task RetrieveLocationsAsync_GetNothing_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<LocationEntity> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, locations.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        public async Task RetrieveLocationsAsync_GetNothing_NoDataOnTable()
        {
            context.Jobs.RemoveRange(Jobs);
            context.SaveChanges();
            context.Locations.RemoveRange(Locations);
            context.SaveChanges();

            int maxReturns = 100;
            int startIndex = 0;
            List<LocationEntity> locations = await query.RetrieveLocationsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, locations.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_LOCATIONS_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveRolesAsync_Error_EFCoreContextThrows()
        {
            query = new LocationQuery(testLogger, null);
            await query.RetrieveLocationsAsync(null, null);
        }
    }
}
