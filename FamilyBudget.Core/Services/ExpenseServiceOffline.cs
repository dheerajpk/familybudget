using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Models;

namespace FamilyBudget.Core.Services
{
    public class ExpenseServiceOffline
    {
        private readonly List<Expense> _currentExpense;

        public ExpenseServiceOffline()
        {
            _currentExpense = new List<Expense>();
        }

        public Task AddVariableExpense(decimal amount, DateTime dateTime, string categoryId, string id = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.VariableExpense,
                Permission = Permission.All,
                MemberId = "",
                Name = $"{dateTime.ToLongDateString()} - {amount}"
            };

            RemoveExpense(id);

            _currentExpense.Add(expense);

            return Task.FromResult(0);
        }

        public Task AddFixedExpense(decimal amount, DateTime dateTime, string categoryId, string id = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.FixedExpense,
                Permission = Permission.All,
                MemberId = "",
                Name = $"{dateTime.ToLongDateString()} - {amount}"
            };

            RemoveExpense(id);

            _currentExpense.Add(expense);
            return Task.FromResult(0);
        }

        public Task AddIncome(decimal amount, DateTime dateTime, string categoryId, string id = null)
        {
            var expense = new Expense()
            {
                Amount = amount,
                DateTime = dateTime,
                Category = GetCategory(categoryId),
                Id = id ?? Guid.NewGuid().ToString(),
                ExpenseType = ExpenseTypes.Income,
                Permission = Permission.All,
                MemberId = "",
                Name = $"{dateTime.ToLongDateString()} - {amount}"
            };

            RemoveExpense(id);

            _currentExpense.Add(expense);

            return Task.FromResult(0);
        }

        public void RemoveExpense(string expenseId)
        {
            if (string.IsNullOrEmpty(expenseId)) return;

            _currentExpense.RemoveAll(x => x.Id == expenseId);
        }

        public List<Expense> GetExpenses()
        {
            return _currentExpense;
        }

        public List<Expense> GetIncome()
        {
            return _currentExpense;
        }

        public List<Expense> GetFixedExpenses()
        {
            return _currentExpense;
        }

        public List<Expense> GetVariableExpenses()
        {
            return _currentExpense;
        }

        public List<Category> GetAllCategories()
        {
            return GetPredefinedCategories();

            //    new List<Category>()
            //{
            //    new Category(){Name = "House Rent"},
            //    new Category(){Name = "Loan EMI"},
            //    new Category(){Name = "Gas Bill"},
            //};
        }

        private Category GetCategory(string categoryId)
        {
            return null;
        }

        private List<Category> GetPredefinedCategories()
        {
            var jsonValue = GetTestJson("Categories.json");

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<FamilySchema<Categories>>(jsonValue);

            return result.Data?.CategoryList;
        }

        private string GetTestJson(string name)
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            string resourceName = $"FamilyBudget.Core.Data.{name}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                return ReadFromFile(stream);
            }
        }

        private string ReadFromFile(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
