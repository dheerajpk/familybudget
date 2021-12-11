using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyBudget.UnitTest
{
    [TestClass]
    public class FacebookServiceTest
    {
        [TestMethod]
        public void GetMyFeeds()
        {
            FacebookService servceFacebookService = new FacebookService();

            servceFacebookService.GetMessages().RunSynchronously();


        }
    }
}
