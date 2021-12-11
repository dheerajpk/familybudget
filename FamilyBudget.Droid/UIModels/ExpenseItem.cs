using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using FamilyBudget.Core.Models;

namespace FamilyBudget.Droid.UIModels
{
    public class ExpenseItem
    {
        public string Id { get; }

        public string DateTime { get; }

        public string Name { get; }

        public decimal Amount { get; }

        public ExpenseTypes ExpenseType { get; }

        public string Category { get; }

        public string CategoryShortName { get; }

        public Permission Permission { get; }

        public string FamilyMemberId { get; }

        public string FamilyMemberName { get; }

        public string RelativeMemberId { get; }

        public string RelativeMemberName { get; }

        public ExpenseItem(Expense expense, List<Category> categoryList, Family familyServiceFamily)
        {
            Id = expense.Id;

            DateTime = expense.DateTime.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            Name = expense.Name;

            Amount = expense.Amount;

            var category = categoryList.FirstOrDefault(x => x.Id == expense.CategoryId);

            Category = category?.Name == "Others" ? $"{category?.Name} - {expense.Name}" : category?.Name;

            CategoryShortName = GetCategoryShortName(category?.Name);

            Permission = expense.Permission;

            ExpenseType = expense.ExpenseType;

            FamilyMemberId = expense.MemberId;

            FamilyMemberName = "Self";

            var familyMember = familyServiceFamily.FamilyMembers.FirstOrDefault(x => x.MemberId == expense.MemberId);

            if (familyMember != null && familyServiceFamily.FamilyMembers.Count > 1)
            {
                RelativeMemberId = familyServiceFamily.FamilyMembers.FirstOrDefault(x => x.MemberId == expense.MemberId)?.MemberId;

                if (!string.IsNullOrEmpty(RelativeMemberId))
                {
                    RelativeMemberName = $"Spent By {familyServiceFamily.FamilyMembers.FirstOrDefault(x => x.MemberId == expense.MemberId)?.MemberName}";
                }
            }

        }

        public bool IsEmpty { get; }
        public ExpenseItem(ExpenseTypes expenseType)
        {
            IsEmpty = true;

            ExpenseType = expenseType;
        }

        private string GetCategoryShortName(string categoryname)
        {
            var spliLetters = categoryname?.Split(' ');
            if (spliLetters != null && spliLetters.Length > 1)
                return $"{spliLetters[0][0].ToString().ToUpper()}{spliLetters[1][0].ToString().ToUpper()}";

            if (spliLetters != null && spliLetters.Length > 0)
                return $"{spliLetters[0][0].ToString().ToUpper()}";

            return string.Empty;
        }
    }
}