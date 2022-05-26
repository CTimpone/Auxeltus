using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuxeltusSqlDataAccessTests.CommandTests
{
    [TestClass]
    public class LocationCommandTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public ILocationCommand command;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
            testLogger = NullLogger.Instance;
            command = new LocationCommand(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_LOCATION_CATEGORY)]
        public async Task CreateLocationAsync_Success()
        {
            Random rnd = new Random();
            Location newLoc = new Location
            {
                Name = Guid.NewGuid().ToString("N"),
                Latitude = rnd.NextDouble(),
                Longitude = rnd.NextDouble()
            };

            await command.CreateLocationAsync(newLoc);

            Location addedLoc = context.Locations.First(l => l.Name == newLoc.Name);

            Assert.IsNotNull(addedLoc.Id);
            CompareLocations(newLoc, addedLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateLocationAsync_Error_DuplicateCoordinates()
        {
            Random rnd = new Random();
            Location newLoc = new Location
            {
                Name = Guid.NewGuid().ToString("N"),
                Latitude = Locations[0].Latitude,
                Longitude = Locations[0].Longitude
            };

            int initialCount = Locations.Count;

            try
            {
                await command.CreateLocationAsync(newLoc);
            }
            catch (Exception)
            {
                Assert.AreEqual(initialCount, context.Locations.Count());
                throw;
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateLocationAsync_Error_DuplicateName()
        {
            Random rnd = new Random();
            Location newLoc = new Location
            {
                Name = Locations[0].Name,
                Latitude = rnd.NextDouble(),
                Longitude = rnd.NextDouble()
            };

            int initialCount = Locations.Count;

            try
            {
                await command.CreateLocationAsync(newLoc);
            }
            catch (Exception)
            {
                Assert.AreEqual(initialCount, context.Locations.Count());
                throw;
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateLocationAsync_Error_NullEFCoreContext()
        {
            Random rnd = new Random();
            Location newLoc = new Location
            {
                Name = Guid.NewGuid().ToString("N"),
                Latitude = rnd.NextDouble(),
                Longitude = rnd.NextDouble()
            };

            command = new LocationCommand(testLogger, null);
            await command.CreateLocationAsync(newLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_LOCATION_CATEGORY)]
        public async Task DeleteLocationAsync_Success()
        {
            Random rnd = new Random();

            int initialCount = Locations.Count;

            Location toBeDeleted = Locations[rnd.Next(0, initialCount - 1)];
            await command.DeleteLocationAsync(toBeDeleted.Id);

            Assert.IsFalse(context.Locations.Any(r => r.Id == toBeDeleted.Id));
            Assert.AreEqual(initialCount - 1, context.Locations.Count());
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteLocationAsync_Error_IdDoesNotExist()
        {
            int initialCount = Locations.Count;

            try
            {
                await command.DeleteLocationAsync(initialCount + 100);
            }
            catch (Exception)
            {
                Assert.AreEqual(initialCount, context.Locations.Count());
                throw;
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteLocationAsync_Error_NullEFCoreContext()
        {
            command = new LocationCommand(testLogger, null);
            await command.DeleteLocationAsync(1);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_AllFields()
        {
            Random rnd = new Random();
            Location locUpdate = new Location
            {
                Name = Guid.NewGuid().ToString("N"),
                Latitude = rnd.NextDouble(),
                Longitude = rnd.NextDouble()
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[rnd.Next(0, initialCount - 1)];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Name == locUpdate.Name);

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(locUpdate, finalLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_NameOnly()
        {
            Random rnd = new Random();
            Location locUpdate = new Location
            {
                Name = Guid.NewGuid().ToString("N")
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[rnd.Next(0, initialCount - 1)];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Name == locUpdate.Name);

            Location expected = new Location
            {
                Name = locUpdate.Name,
                Latitude = toBeUpdated.Latitude,
                Longitude = toBeUpdated.Longitude
            };

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(expected, finalLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_LatitudeOnly()
        {
            Random rnd = new Random();
            Location locUpdate = new Location
            {
                Latitude = rnd.NextDouble()
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[rnd.Next(0, initialCount - 1)];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Id == toBeUpdated.Id);

            Location expected = new Location
            {
                Name = toBeUpdated.Name,
                Latitude = locUpdate.Latitude,
                Longitude = toBeUpdated.Longitude
            };

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(expected, finalLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_LatitudeOnly_MatchesExistingLatitudeForOtherRecord()
        {
            Location locUpdate = new Location
            {
                Latitude = Locations[0].Latitude
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[1];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Id == toBeUpdated.Id);

            Location expected = new Location
            {
                Name = toBeUpdated.Name,
                Latitude = locUpdate.Latitude,
                Longitude = toBeUpdated.Longitude
            };

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(expected, finalLoc);
        }


        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_LongitudeOnly()
        {
            Random rnd = new Random();
            Location locUpdate = new Location
            {
                Longitude = rnd.NextDouble()
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[rnd.Next(0, initialCount - 1)];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Id == toBeUpdated.Id);

            Location expected = new Location
            {
                Name = toBeUpdated.Name,
                Latitude = toBeUpdated.Latitude,
                Longitude = locUpdate.Longitude
            };

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(expected, finalLoc);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        public async Task UpdateLocationAsync_LongitudeOnly_MatchesExistingLongitudeForOtherRecord()
        {
            Location locUpdate = new Location
            {
                Latitude = Locations[0].Longitude
            };

            int initialCount = Locations.Count;

            Location toBeUpdated = Locations[1];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);

            Location finalLoc = context.Locations.First(l => l.Id == toBeUpdated.Id);

            Location expected = new Location
            {
                Name = toBeUpdated.Name,
                Latitude = locUpdate.Latitude,
                Longitude = toBeUpdated.Longitude
            };

            Assert.AreEqual(toBeUpdated.Id, finalLoc.Id);
            CompareLocations(expected, finalLoc);
        }


        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateLocationAsync_Error_DuplicateName()
        {
            Location locUpdate = new Location
            {
                Name = Locations[0].Name
            };


            Location toBeUpdated = Locations[1];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateLocationAsync_Error_DuplicateCoordinates()
        {
            Location locUpdate = new Location
            {
                Latitude = Locations[0].Latitude,
                Longitude = Locations[0].Longitude
            };


            Location toBeUpdated = Locations[1];

            await command.UpdateLocationAsync(toBeUpdated.Id, locUpdate);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_LOCATION_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateLocationAsync_Error_EFCoreContextNull()
        {
            Location locUpdate = new Location
            {
                Name = Guid.NewGuid().ToString("N")
            };

            command = new LocationCommand(testLogger, null);
            await command.UpdateLocationAsync(1, locUpdate);
        }
    }
}