using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Models;

namespace FamilyBudget.Core.Services
{
    public class ExpenseServiceOnline
    {
        private List<Expense> _currentExpense;

        private readonly SlackService _slackService;

        public List<Category> CategoryList { get; private set; }

        public ExpenseServiceOnline()
        {
            _currentExpense = new List<Expense>();

            _slackService = new SlackService();
        }

        public async Task<bool> AddVariableExpense(decimal amount, DateTime dateTime, string categoryId, string memberId, string familyCode, string id = null, string externalRefernceId = null, string expenseName = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                CategoryId = categoryId,
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.VariableExpense,
                Permission = Permission.All,
                MemberId = memberId,
                FamilyCode = familyCode,
                Name = expenseName ?? $"{dateTime.ToLongDateString()} - {amount}"
            };

            var expenseSchema = new FamilySchema<Expense>()
            {
                Schema = FamilySchemaConstants.FamilyExpense,
                Data = expense,
            };

            if (externalRefernceId == null)
            {
                var messageId = await _slackService.PostMessage(expenseSchema).ConfigureAwait(false);

                expense.ExternalRefernceId = messageId;

                _currentExpense.Add(expense);
            }
            else
            {
                return await _slackService.UpdateMessage(expenseSchema, externalRefernceId).ConfigureAwait(false);
            }

            return true;
        }

        public async Task<bool> AddFixedExpense(decimal amount, DateTime dateTime, string categoryId, string memberId, string familyCode, string id = null, string externalRefernceId = null, string expenseName = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                CategoryId = categoryId,
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.FixedExpense,
                Permission = Permission.All,
                MemberId = memberId,
                FamilyCode = familyCode,
                Name = expenseName ?? $"{dateTime.ToLongDateString()} - {amount}"
            };

            var expenseSchema = new FamilySchema<Expense>()
            {
                Schema = FamilySchemaConstants.FamilyExpense,
                Data = expense
            };

            if (externalRefernceId == null)
            {
                var messageId = await _slackService.PostMessage(expenseSchema).ConfigureAwait(false);

                expense.ExternalRefernceId = messageId;

                _currentExpense.Add(expense);
            }
            else
            {
                return await _slackService.UpdateMessage(expenseSchema, externalRefernceId).ConfigureAwait(false);
            }

            return true;
        }

        public async Task<bool> AddIncome(decimal amount, DateTime dateTime, string categoryId, string memberId, string familyCode, string id = null, string externalRefernceId = null, string expenseName = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                CategoryId = categoryId,
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.Income,
                Permission = Permission.All,
                MemberId = memberId,
                FamilyCode = familyCode,
                Name = expenseName ?? $"{dateTime.ToLongDateString()} - {amount}"
            };

            var expenseSchema = new FamilySchema<Expense>()
            {
                Schema = FamilySchemaConstants.FamilyExpense,
                Data = expense
            };

            if (externalRefernceId == null)
            {
                var messageId = await _slackService.PostMessage(expenseSchema).ConfigureAwait(false);

                expense.ExternalRefernceId = messageId;

                _currentExpense.Add(expense);
            }
            else
            {
                return await _slackService.UpdateMessage(expenseSchema, externalRefernceId).ConfigureAwait(false);
            }

            return true;
        }

        public async Task RemoveExpense(string expenseId)
        {
            if (string.IsNullOrEmpty(expenseId)) return;

            var expense = _currentExpense.FirstOrDefault(x => x.Id == expenseId);

            if (expense != null)
            {
                await _slackService.DeleteMessage(expense.ExternalRefernceId).ConfigureAwait(false);
            }

            _currentExpense.RemoveAll(x => x.Id == expenseId);
        }

        public async Task<List<Expense>> GetExpenses(string familyCode)
        {
            var allmessageSchemas = await _slackService.GetMessages().ConfigureAwait(false);

            var expenses = allmessageSchemas?.Where(x => x.Text != null && x.Text.Contains("\"schema\":\"FamilyExpense\""));

            var firstDateOfCurrMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            _currentExpense = expenses.Select(x => new Expense(x)).Where(x => x.FamilyCode == familyCode && x.DateTime.Date >= firstDateOfCurrMonth).ToList();

            return _currentExpense ?? new List<Expense>();
        }

        public async Task<List<Expense>> GetIncome(string familyCode, bool ignoreCache = false)
        {
            if (ignoreCache) await GetExpenses(familyCode).ConfigureAwait(false);

            return _currentExpense?.Where(x => x.ExpenseType == ExpenseTypes.Income).ToList();
        }

        public async Task<List<Expense>> GetFixedExpenses(string familyCode, bool ignoreCache = false)
        {
            if (ignoreCache) await GetExpenses(familyCode).ConfigureAwait(false);

            return _currentExpense?.Where(x => x.ExpenseType == ExpenseTypes.FixedExpense).ToList();
        }

        public async Task<List<Expense>> GetVariableExpenses(string familyCode, bool ignoreCache = false)
        {
            if (ignoreCache) await GetExpenses(familyCode).ConfigureAwait(false);

            return _currentExpense?.Where(x => x.ExpenseType == ExpenseTypes.VariableExpense).ToList();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var allmessages = await _slackService.GetMessages().ConfigureAwait(false);

            var categoryMessage = allmessages?.FirstOrDefault(x => x.Text != null && x.Text.Contains("\"schema\":\"Categories\""));

            if (categoryMessage != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<FamilySchema<Categories>>(categoryMessage.Text);

                return CategoryList = result?.Data?.CategoryList;
            }

            return null;
        }

        private Category GetCategory(string categoryId)
        {
            return null;
        }

        public async Task<decimal> GetTotalBalance(string familyCode)
        {
            var incomeList = await GetIncome(familyCode).ConfigureAwait(false);

            var fixedList = await GetFixedExpenses(familyCode).ConfigureAwait(false);

            var variableList = await GetVariableExpenses(familyCode).ConfigureAwait(false);

            var totalincome = incomeList.Select(x => x.Amount).Sum();

            var expeneTotal = fixedList.Select(x => x.Amount).Sum() + variableList.Select(x => x.Amount).Sum();

            return totalincome - expeneTotal;
        }


    }
}
