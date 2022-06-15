using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuxeltusApiUnitTesting
{
    [TestClass]
    public class RoleRepositoryTestBase
    {
        public Mock<IRoleCommand> roleCommandMock;
        public Mock<IRoleQuery> roleQueryMock;

        public RoleRepository repo;

        [TestInitialize]
        public void Initialize()
        {
            roleCommandMock = new Mock<IRoleCommand>();
            roleQueryMock = new Mock<IRoleQuery>();

            repo = new RoleRepository(roleCommandMock.Object, roleQueryMock.Object, NullLogger.Instance);
        }
    }
}
