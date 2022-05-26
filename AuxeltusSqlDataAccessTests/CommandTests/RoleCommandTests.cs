using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            Role newRole = new Role
            {
                Title = Guid.NewGuid().ToString("N"),
                MaximumSalary = rnd.Next(3001, 6000),
                MinimumSalary = rnd.Next(1, 3000),
                Tier = rnd.Next(1, 10)
            };

            await command.CreateRoleAsync(newRole);

            Role addedRole = context.Roles.First(r => r.Title == newRole.Title);


        }
    }
}
