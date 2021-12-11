using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyBudget.UnitTest
{
    [TestClass]
    public class FamilyServiceTest
    {
        [TestMethod]
        public async Task SetUpFamilyTestAsync()
        {
            FamilyService familyService = new FamilyService();

            var result = await familyService.SetUpFamily("Katilot","Dheeraj");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task JoinFamilyTestAsync()
        {
            FamilyService familyService = new FamilyService();

            var result = await familyService.JoinFamily("XNHJKD", "Vaishna");

            Assert.IsTrue(result);
        }
    }
}
