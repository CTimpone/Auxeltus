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
    public class RoleQueryTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public IRoleQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
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
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetAll()
        {
            List<Role> roles = await query.RetrieveRolesAsync(100, 0);

            Assert.AreEqual(Roles.Count, roles.Count);

            foreach (Role role in roles)
            {
                Role matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetSubset_UpToMaxReturns()
        {
            int maxReturns = 2;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, 0);

            Assert.AreEqual(maxReturns, roles.Count);

            foreach (Role role in roles)
            {
                Role matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetSubset_OnlyAfterStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 4;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(Roles.Count - (startIndex - 1), roles.Count);

            foreach (Role role in roles)
            {
                Assert.IsTrue(role.Id >= startIndex);
                Role matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetNothing_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(0, roles.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetNothing_NoDataOnTable()
        {
            context.Jobs.RemoveRange(Jobs);
            context.SaveChanges();
            context.Roles.RemoveRange(Roles);
            context.SaveChanges();

            int maxReturns = 100;
            int startIndex = 0;
            List<Role> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(0, roles.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveRolesAsync_Error_EFCoreContextThrows()
        {
            query = new RoleQuery(testLogger, null);
            await query.RetrieveRolesAsync(null, null);
        }

    }
}
