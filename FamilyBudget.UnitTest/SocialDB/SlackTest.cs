using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialDB.Query;
using SocialDB.Services;

namespace FamilyBudget.UnitTest.SocialDB
{
    [TestClass]
    public class SlackTest
    {
        private Connection _connection;

        private const string AccessToken = "<GET API ACCESS TOKEN>";

        [TestInitialize]
        public void Setup()
        {
            _connection = new Connection(AccessToken, ServiceConnection.Slack);
        }

        [TestMethod]
        [Priority(0)]
        public async Task InsertSlackMessageTest()
        {

            var expnse = new Expense()
            {
                Amount = 100,
                Name = "Insert Test"

            };

            var messageRow = await _connection.ContentQuery.Insert(expnse);

            Assert.IsNotNull(messageRow);

            Assert.IsNotNull(messageRow?.PrimaryId);
        }

        [TestMethod]
        [Priority(1)]
        public async Task SelectSlackMessagesTest()
        {
            var messages = await _connection.ContentQuery.Select<FamilySchema<Expense>>();

            Assert.IsNotNull(messages);

            Assert.IsTrue(messages.Any());
        }

        [TestMethod]
        [Priority(2)]
        public async Task UpdateSlackMessageTest()
        {
            var messageRows = await _connection.ContentQuery.Select<Expense>(x => x.Data?.Name == "Insert Test");

            var messageRow = messageRows.FirstOrDefault();

            Assert.IsNotNull(messageRow);

            Assert.IsNotNull(messageRow?.PrimaryId);

            messageRow.Data.Amount++;

            var isUpdateTrue = await _connection.ContentQuery.Update(messageRow.Data, messageRow.PrimaryId);

            Assert.IsTrue(isUpdateTrue);
        }

        [TestMethod]
        [Priority(3)]
        public async Task DeleteSlackMessageTest()
        {
            var messageRows = await _connection.ContentQuery.Select<Expense>(x => x.Data?.Name == "Insert Test");

            var messageRow = messageRows.FirstOrDefault();

            Assert.IsNotNull(messageRow);

            Assert.IsNotNull(messageRow?.PrimaryId);

            var isUpdateTrue = await _connection.ContentQuery.Delete(messageRow.PrimaryId);

            Assert.IsTrue(isUpdateTrue);
        }
    }
}
