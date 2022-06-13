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
        public ILogger<RoleQuery> testLogger;
        public IRoleQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
            testLogger = NullLogger<RoleQuery>.Instance;
            query = new RoleQuery(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLE_CATEGORY)]
        public async Task RetrieveRoleAsync_Success()
        {
            int id = 1;
            RoleEntity role = await query.RetrieveRoleAsync(id);

            Assert.IsNotNull(role);

            RoleEntity matchingRole = Roles.First(r => r.Id == id);

            CompareRoles(matchingRole, role);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLE_CATEGORY)]
        public async Task RetrieveRoleAsync_NoMatch()
        {
            int id = 1000;
            RoleEntity role = await query.RetrieveRoleAsync(id);

            Assert.IsNull(role);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLE_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveRoleAsync_Error_EFCoreContextThrows()
        {
            int id = 1;

            query = new RoleQuery(testLogger, null);
            await query.RetrieveRoleAsync(id);
        }


        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetAll()
        {
            List<RoleEntity> roles = await query.RetrieveRolesAsync(100, 0);

            Assert.AreEqual(Roles.Count, roles.Count);

            foreach (RoleEntity role in roles)
            {
                RoleEntity matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetSubset_UpToMaxReturns()
        {
            int maxReturns = 2;
            List<RoleEntity> roles = await query.RetrieveRolesAsync(maxReturns, 0);

            Assert.AreEqual(maxReturns, roles.Count);

            foreach (RoleEntity role in roles)
            {
                RoleEntity matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetSubset_OnlyAfterStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 4;
            List<RoleEntity> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

            Assert.AreEqual(Roles.Count - (startIndex - 1), roles.Count);

            foreach (RoleEntity role in roles)
            {
                Assert.IsTrue(role.Id >= startIndex);
                RoleEntity matchingRole = Roles.First(r => r.Id == role.Id);

                CompareRoles(matchingRole, role);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_ROLES_CATEGORY)]
        public async Task RetrieveRolesAsync_GetNothing_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<RoleEntity> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

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
            List<RoleEntity> roles = await query.RetrieveRolesAsync(maxReturns, startIndex);

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
