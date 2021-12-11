using System;
using FamilyBudget.Core.Facebook.Models;
using FamilyBudget.Core.Slack.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FamilyBudget.Core.Models
{
    public class Expense
    {
        public string Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExpenseTypes ExpenseType { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        public string CategoryId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Permission Permission { get; set; }

        public string MemberId { get; set; }

        public string FamilyCode { get; set; }

        [JsonIgnore]
        public string ExternalRefernceId { get; set; }

        public Expense(FeedMessage feedmessage)
        {
            var expenseSchema = JsonConvert.DeserializeObject<FamilySchema<Expense>>(feedmessage.Message);

            var expense = expenseSchema.Data;

            this.ExternalRefernceId = feedmessage.Id;
            
            this.Amount = expense.Amount;

            this.CategoryId = expense.CategoryId;

            this.DateTime = expense.DateTime;

            this.ExpenseType = expense.ExpenseType;

            this.FamilyCode = expense.FamilyCode;

            this.Id = expense.Id;

            this.MemberId = expense.MemberId;

            this.Name = expense.Name;

            this.Permission = expense.Permission;

        }

        public Expense(Message message)
        {
            var expenseSchema = JsonConvert.DeserializeObject<FamilySchema<Expense>>(message.Text);

            var expense = expenseSchema.Data;

            this.ExternalRefernceId = message.Ts;

            this.Amount = expense.Amount;

            this.CategoryId = expense.CategoryId;

            this.DateTime = expense.DateTime;

            this.ExpenseType = expense.ExpenseType;

            this.FamilyCode = expense.FamilyCode;

            this.Id = expense.Id;

            this.MemberId = expense.MemberId;

            this.Name = expense.Name;

            this.Permission = expense.Permission;

        }

        public Expense()
        {

        }
    }
}
