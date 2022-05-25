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
    public class RoleQueryTests
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public IRoleQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = TestDataSetup.GenerateTestDataContext();
            testLogger = NullLogger.Instance;
            query = new RoleQuery(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        public async Task RetrieveRolesAsync_Success_GetAll()
        {
            List<Role> roles = await query.RetrieveRolesAsync(100, 0);

            Assert.AreEqual(TestDataSetup.Roles.Count, roles.Count);

            foreach (Role role in roles)
            {
                Role matchingRole = TestDataSetup.Roles.First(r => r.Id == role.Id);
                Assert.AreEqual(matchingRole.Title, role.Title);
                Assert.AreEqual(matchingRole.MaximumSalary, role.MaximumSalary);
                Assert.AreEqual(matchingRole.MinimumSalary, role.MinimumSalary);
                Assert.AreEqual(matchingRole.Tier, role.Tier);
            }
        }

        [TestMethod]
        public async Task RetrieveRolesAsync_Success_GetSubset_UpToMaxReturns()
        {
            int maxReturns = 2;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, 0);

            Assert.AreEqual(maxReturns, roles.Count);

            foreach (Role role in roles)
            {
                Role matchingRole = TestDataSetup.Roles.First(r => r.Id == role.Id);
                Assert.AreEqual(matchingRole.Title, role.Title);
                Assert.AreEqual(matchingRole.MaximumSalary, role.MaximumSalary);
                Assert.AreEqual(matchingRole.MinimumSalary, role.MinimumSalary);
                Assert.AreEqual(matchingRole.Tier, role.Tier);
            }
        }

        [TestMethod]
        public async Task RetrieveRolesAsync_Success_GetSubset_OnlyAfterStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 4;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(TestDataSetup.Roles.Count - (startIndex - 1), roles.Count);

            foreach (Role role in roles)
            {
                Assert.IsTrue(role.Id >= startIndex);
                Role matchingRole = TestDataSetup.Roles.First(r => r.Id == role.Id);
                Assert.AreEqual(matchingRole.Title, role.Title);
                Assert.AreEqual(matchingRole.MaximumSalary, role.MaximumSalary);
                Assert.AreEqual(matchingRole.MinimumSalary, role.MinimumSalary);
                Assert.AreEqual(matchingRole.Tier, role.Tier);
            }
        }

        [TestMethod]
        public async Task RetrieveRolesAsync_Success_GetNothing_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(0, roles.Count);
        }

        [TestMethod]
        public async Task RetrieveRolesAsync_Success_GetNothing_NoDataOnTable()
        {
            context.Jobs.RemoveRange(TestDataSetup.Jobs);
            context.SaveChanges();
            context.Roles.RemoveRange(TestDataSetup.Roles);
            context.SaveChanges();

            int maxReturns = 100;
            int startIndex = 0;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(0, roles.Count);
        }

    }
}
