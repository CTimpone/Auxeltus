using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            Assert.IsTrue(true);
        }
    }
}
