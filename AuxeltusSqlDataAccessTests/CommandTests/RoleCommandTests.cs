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
    public class RoleCommandTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public IRoleCommand command;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
            testLogger = NullLogger.Instance;
            command = new RoleCommand(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_ROLE_CATEGORY)]
        public async Task CreateRoleAsync_Success()
        {
            Random rnd = new Random();
            RoleEntity newRole = new RoleEntity
            {
                Title = Guid.NewGuid().ToString("N"),
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            await command.CreateRoleAsync(newRole);

            RoleEntity addedRole = context.Roles.First(r => r.Title == newRole.Title);

            Assert.IsNotNull(addedRole.Id);
            CompareRoles(newRole, addedRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateRoleAsync_Error_DuplicateTitle()
        {
            Random rnd = new Random();
            RoleEntity newRole = new RoleEntity
            {
                Title = Roles.First().Title,
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            int initialCount = Roles.Count;

            try
            {
                await command.CreateRoleAsync(newRole);
            }
            catch (Exception)
            {
                Assert.AreEqual(initialCount, context.Roles.Count());
                throw;
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateRoleAsync_Error_NullEFCoreContext()
        {
            Random rnd = new Random();
            RoleEntity newRole = new RoleEntity
            {
                Title = Guid.NewGuid().ToString("N"),
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            command = new RoleCommand(testLogger, null);
            await command.CreateRoleAsync(newRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_ROLE_CATEGORY)]
        public async Task DeleteRoleAsync_Success()
        {
            int initialCount = Roles.Count;

            RoleEntity toBeDeleted = Roles[4];
            await command.DeleteRoleAsync(toBeDeleted.Id);

            Assert.IsFalse(context.Roles.Any(r => r.Id == toBeDeleted.Id));
            Assert.AreEqual(initialCount - 1, context.Roles.Count());
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteRoleAsync_Error_IdDoesNotExist()
        {
            int initialCount = Roles.Count;

            try
            {
                await command.DeleteRoleAsync(initialCount + 100);
            }
            catch (Exception)
            {
                Assert.AreEqual(initialCount, context.Roles.Count());
                throw;
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteRoleAsync_Error_NullEFCoreContext()
        {
            command = new RoleCommand(testLogger, null);
            await command.DeleteRoleAsync(1);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        public async Task UpdateRoleAsync_AllFields()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                Title = Guid.NewGuid().ToString("N"),
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);

            RoleEntity finalRole = context.Roles.First(r => r.Title == roleUpdate.Title);

            Assert.AreEqual(toBeUpdated.Id, finalRole.Id);
            CompareRoles(roleUpdate, finalRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        public async Task UpdateRoleAsync_TitleOnly()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                Title = Guid.NewGuid().ToString("N"),
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);

            RoleEntity finalRole = context.Roles.First(r => r.Title == roleUpdate.Title);

            RoleEntity expected = new RoleEntity
            {
                Title = roleUpdate.Title,
                Tier = toBeUpdated.Tier,
                MaximumSalary = toBeUpdated.MaximumSalary,
                MinimumSalary = toBeUpdated.MinimumSalary
            };

            Assert.AreEqual(toBeUpdated.Id, finalRole.Id);
            CompareRoles(expected, finalRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        public async Task UpdateRoleAsync_TierOnly()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                Tier = rnd.Next(1, 10)
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);

            RoleEntity finalRole = context.Roles.First(r => r.Title == toBeUpdated.Title);

            RoleEntity expected = new RoleEntity
            {
                Title = toBeUpdated.Title,
                Tier = roleUpdate.Tier,
                MaximumSalary = toBeUpdated.MaximumSalary,
                MinimumSalary = toBeUpdated.MinimumSalary
            };

            Assert.AreEqual(toBeUpdated.Id, finalRole.Id);
            CompareRoles(expected, finalRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        public async Task UpdateRoleAsync_MaximumSalaryOnly()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                MaximumSalary = rnd.Next(3001, 6000),
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);

            RoleEntity finalRole = context.Roles.First(r => r.Title == toBeUpdated.Title);

            RoleEntity expected = new RoleEntity
            {
                Title = toBeUpdated.Title,
                Tier = toBeUpdated.Tier,
                MaximumSalary = roleUpdate.MaximumSalary,
                MinimumSalary = toBeUpdated.MinimumSalary
            };

            Assert.AreEqual(toBeUpdated.Id, finalRole.Id);
            CompareRoles(expected, finalRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        public async Task UpdateRoleAsync_MinimumSalaryOnly()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                MinimumSalary = rnd.Next(3001, 6000),
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);

            RoleEntity finalRole = context.Roles.First(r => r.Title == toBeUpdated.Title);

            RoleEntity expected = new RoleEntity
            {
                Title = toBeUpdated.Title,
                Tier = toBeUpdated.Tier,
                MaximumSalary = toBeUpdated.MaximumSalary,
                MinimumSalary = roleUpdate.MinimumSalary
            };

            Assert.AreEqual(toBeUpdated.Id, finalRole.Id);
            CompareRoles(expected, finalRole);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateRoleAsync_Error_DuplicateTitle()
        {
            RoleEntity roleUpdate = new RoleEntity
            {
                Title = Roles[1].Title,
            };

            RoleEntity toBeUpdated = Roles[0];

            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateRoleAsync_Error_EFCoreContextNull()
        {
            Random rnd = new Random();
            RoleEntity roleUpdate = new RoleEntity
            {
                Title = Guid.NewGuid().ToString("N"),
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            int initialCount = Roles.Count;

            RoleEntity toBeUpdated = Roles[rnd.Next(0, initialCount - 1)];

            command = new RoleCommand(testLogger, null);
            await command.UpdateRoleAsync(toBeUpdated.Id, roleUpdate);
        }
    }
}