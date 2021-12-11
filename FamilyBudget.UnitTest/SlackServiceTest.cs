using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyBudget.UnitTest
{
    [TestClass]
    public class SlackServiceTest
    {
        [TestMethod]
        public async Task SetupFamilyTest()
        {
            SlackService slackService = new SlackService();

            await slackService.Setup();

            Assert.IsNotNull(slackService.FamChannel);
        }
    }
}
