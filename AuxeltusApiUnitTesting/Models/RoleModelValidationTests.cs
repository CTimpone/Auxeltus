using Auxeltus.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace AuxeltusApiUnitTesting
{
    [TestClass]
    public class RoleModelValidationTests : ModelValidationTestBase
    {
        [TestMethod]
        public void Role_FullyValidModel()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next(),
                Id = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void Role_ValidModel_NoIdSpecified()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void Role_InvalidModel_NullTitle()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = null,
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("The Title field is required.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Title", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_TitleEmptyString()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = string.Empty,
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("The Title field is required.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Title", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_TitleTooShort()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = "a",
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Title must be at least two (2) characters long.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Title", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_TitleTooLong()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Title must not exceed one hundred (100) characters.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Title", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NullTier()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = null
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("The Tier field is required.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Tier", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NegativeTier()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = -1
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Tier must be a non-negative integer.", validationResults[0].ErrorMessage);
            Assert.AreEqual("Tier", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NullMaximumSalary()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = null,
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("The MaximumSalary field is required.", validationResults[0].ErrorMessage);
            Assert.AreEqual("MaximumSalary", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NegativeMaximumSalary()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = -1,
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("MaximumSalary must be a non-negative integer.", validationResults[0].ErrorMessage);
            Assert.AreEqual("MaximumSalary", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NullMinimumSalary()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = null,
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("The MinimumSalary field is required.", validationResults[0].ErrorMessage);
            Assert.AreEqual("MinimumSalary", validationResults[0].MemberNames.First());
        }

        [TestMethod]
        public void Role_InvalidModel_NegativeMinimumSalary()
        {
            Random rnd = new Random();
            Role model = new Role
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = -1,
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            var validationResults = ValidateModel(model);

            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("MinimumSalary must be a non-negative integer.", validationResults[0].ErrorMessage);
            Assert.AreEqual("MinimumSalary", validationResults[0].MemberNames.First());
        }
    }
}
