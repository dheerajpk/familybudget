using System;
using System.Collections.Generic;
using System.Text;
using FamilyBudget.Core.Models;

namespace FamilyBudget.Core.Services
{
    public class ExpenseParam
    {
        public string Id { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string CategoryId { get; set; }

        public ExpenseTypes ExpenseType { get; set; }
    }
}
