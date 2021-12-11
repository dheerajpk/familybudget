using FamilyBudget.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyBudget.UnitTest
{
    [TestClass]
    public class ExpenseServiceOnlineTest
    {
        private string _familyCode = "DDQHPP";

        [TestMethod]
        public async Task GetCategoryTestAsync()
        {
            ExpenseServiceOnline expenseServiceOnline = new ExpenseServiceOnline();

            var result = await expenseServiceOnline.GetAllCategories();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddIncomeAsync()
        {
            ExpenseServiceOnline expenseServiceOnline = new ExpenseServiceOnline();

            FamilyService familyService = new FamilyService();

            var familySchema = await familyService.GetFamily();

            Assert.IsNotNull(familySchema);

            var family = familySchema.Data;

            var member = family.FamilyMembers.FirstOrDefault();

            Assert.IsNotNull(member);

            var success = await expenseServiceOnline.AddIncome(25000, DateTime.Now,
                "A77E483B-35CE-42D5-B656-0D2F36A918DA", member.MemberId,
                family.FamilyCode).ConfigureAwait(false);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task AddFixedExpenseAsync()
        {
            ExpenseServiceOnline expenseServiceOnline = new ExpenseServiceOnline();

            FamilyService familyService = new FamilyService();

            var familySchema = await familyService.GetFamily();

            Assert.IsNotNull(familySchema);

            var family = familySchema.Data;

            var member = family.FamilyMembers.FirstOrDefault();

            Assert.IsNotNull(member);

            //var success = await expenseServiceOnline.AddFixedExpense(13000, DateTime.Now, "0857F97A-D78D-4940-B225-BFECBDC010B4", member.MemberId,
            //    family.FamilyCode);

            var success = await expenseServiceOnline.AddFixedExpense(13500, DateTime.Now,
                "0857F97A-D78D-4940-B225-BFECBDC010B4", member.MemberId,
                family.FamilyCode, "46d36dc9-6e37-4a15-9fd8-7a50c0ac24c8", "409621129452291_410230736057997");

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task AddVariableExpenseAsync()
        {
            ExpenseServiceOnline expenseServiceOnline = new ExpenseServiceOnline();

            FamilyService familyService = new FamilyService();

            var familySchema = await familyService.GetFamily();

            Assert.IsNotNull(familySchema);

            var family = familySchema.Data;

            var member = family.FamilyMembers.FirstOrDefault();

            Assert.IsNotNull(member);

            var success = await expenseServiceOnline.AddVariableExpense(4500, DateTime.Now,
                "B0B7F5B5-E193-4EE8-997E-88FF893090C3", member.MemberId,
                family.FamilyCode);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task GetExpenseTestAsync()
        {
            ExpenseServiceOnline expenseServiceOnline = new ExpenseServiceOnline();

            var income = await expenseServiceOnline.GetIncome(_familyCode,true);

            Assert.IsTrue(income.Count > 0);

            var fixedExpenses = await expenseServiceOnline.GetFixedExpenses(_familyCode,true);

            Assert.IsTrue(fixedExpenses.Count > 0);

            var variableExpenses = await expenseServiceOnline.GetVariableExpenses(_familyCode,true);

            Assert.IsTrue(variableExpenses.Count > 0);
        }
    }
}
