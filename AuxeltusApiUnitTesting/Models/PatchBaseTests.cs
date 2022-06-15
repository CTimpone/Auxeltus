using Auxeltus.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AuxeltusApiUnitTesting
{
    [TestClass]
    public class PatchBaseTests
    {
        [TestMethod]
        public void PatchBase_AllFieldsSpecified()
        {
            Random rnd = new Random();
            RolePatch model = new RolePatch
            {
                MaximumSalary = rnd.Next(),
                MinimumSalary = rnd.Next(),
                Title = Guid.NewGuid().ToString("N"),
                Tier = rnd.Next()
            };

            foreach (var prop in typeof(RolePatch).GetProperties())
            {
                Assert.IsTrue(model.PropertySpecified(prop.Name));
            }
        }

        [TestMethod]
        public void PatchBase_AllFieldsSpecified_ExplicitNull()
        {
            RolePatch model = new RolePatch
            {
                MaximumSalary = null,
                MinimumSalary = null,
                Title = null,
                Tier = null
            };

            foreach (var prop in typeof(RolePatch).GetProperties())
            {
                Assert.IsTrue(model.PropertySpecified(prop.Name));
                Assert.IsNull(prop.GetValue(model));
            }
        }

        [TestMethod]
        public void PatchBase_NoFieldsSpecified_ImplicitNulls()
        {
            RolePatch model = new RolePatch();

            foreach (var prop in typeof(RolePatch).GetProperties())
            {
                Assert.IsFalse(model.PropertySpecified(prop.Name));
                Assert.IsNull(prop.GetValue(model));
            }
        }

        [TestMethod]
        public void PatchBase_SomeFieldsSpecified()
        {
            RolePatch model = new RolePatch
            {
                Title = Guid.NewGuid().ToString("N"),
                MinimumSalary = null
            };

            Assert.IsTrue(model.PropertySpecified(nameof(model.Title)));
            Assert.IsNotNull(model.Title);
            Assert.IsTrue(model.PropertySpecified(nameof(model.MinimumSalary)));
            Assert.IsNull(model.MinimumSalary);
            Assert.IsFalse(model.PropertySpecified(nameof(model.MaximumSalary)));
            Assert.IsNull(model.MaximumSalary);
            Assert.IsFalse(model.PropertySpecified(nameof(model.Tier)));
            Assert.IsNull(model.Tier);
        }

    }
}
