using System;
using System.Linq;
using FamilyBudget.Core.Models;
using FamilyBudget.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyBudget.UnitTest
{
    [TestClass]
    public class ExpenseServiceTest
    {
        private ExpenseServiceOffline _expenseService;

        [TestInitialize]
        public void Setup()
        {
            _expenseService = new ExpenseServiceOffline();
        }

        [TestMethod]
        [Priority(0)]
        public void AddFixedExpenseTest()
        {
            _expenseService.AddFixedExpense(12, DateTime.Now, "D1F0FEBC-8D7F-49BF-BB00-028AAB30DD66");

            Assert.AreEqual(1, _expenseService.GetExpenses().Count);
        }

        [TestMethod]
        [Priority(1)]
        public void UpdateFixedExpenseTest()
        {
            AddFixedExpenseTest();

            var expense = _expenseService.GetExpenses().First();

            _expenseService.AddFixedExpense(24, DateTime.Now, "D1F0FEBC-8D7F-49BF-BB00-028AAB30DD66", expense.Id);

            expense = _expenseService.GetExpenses().First(x => x.Id == expense.Id);

            Assert.AreEqual(24, expense.Amount);
        }

        [TestMethod]
        [Priority(2)]
        public void RemoveExpense()
        {
            AddFixedExpenseTest();

            var expense = _expenseService.GetExpenses().First();

            _expenseService.RemoveExpense(expense.Id);

            Assert.AreEqual(0, _expenseService.GetExpenses().Count);
        }
    }
}
