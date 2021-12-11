using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using FamilyBudget.Core.Models;
using FamilyBudget.Core.Services;
using FamilyBudget.Droid.UIModels;
using FamilyBudget.Droid.Views;
using Object = Java.Lang.Object;

namespace FamilyBudget.Droid.Adapters
{
    public class ExpenseAdapter : BaseAdapter
    {

        //private NewExpenseView _newIncomeView;
        //private NewExpenseView _newFixedExpenseView;
        //private NewExpenseView _newVariableExpenseView;

        public event EventHandler<ExpenseTypes> OnNewExpenseRequested;

        public event EventHandler<ExpenseItem> OnDeleteRequested;


        private IList<ExpenseItem> _items;

        private Activity _activity;

        public override int Count
        {
            get { return Items?.Count ?? 0; }
        }

        protected IList<ExpenseItem> Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
            }
        }


        protected Activity Activity
        {
            get
            {
                return _activity;
            }

            set
            {
                _activity = value;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return new Java.Lang.String(Items[position].ToString());
        }

        public override long GetItemId(int position)
        {
            return Items[position].GetHashCode();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var currentItem = Items[position];

            convertView = Activity.LayoutInflater.Inflate(currentItem.IsEmpty ? Resource.Layout.ExpenseHeaderLayout : Resource.Layout.ExpenseItemLayout, parent, false);

            if (currentItem.IsEmpty)
            {
                OnRowHeaderLayoutBind(convertView, Items[position]);
            }
            else
            {
                OnRowLayoutBind(convertView, Items[position]);
            }

            return convertView;

        }


        protected ExpenseAdapter()
        {
            this._items = new List<ExpenseItem>();
        }


        public ExpenseAdapter(IList<ExpenseItem> itemsource, Activity activity)
        {
            Items = itemsource;
            Activity = activity;
        }

        private void OnRowLayoutBind(View rowView, ExpenseItem expenseItem)
        {
            rowView.FindViewById<TextView>(Resource.Id.CatShortTextView).Text = expenseItem.CategoryShortName;

            rowView.FindViewById<TextView>(Resource.Id.CategoryTextView).Text = expenseItem.Category;

            rowView.FindViewById<TextView>(Resource.Id.DateTextView).Text = expenseItem.DateTime;

            rowView.FindViewById<TextView>(Resource.Id.AmountTextView).Text = expenseItem.Amount.ToString("N", CultureInfo.InvariantCulture);

            rowView.FindViewById<TextView>(Resource.Id.SpentByTextView).Text = expenseItem.RelativeMemberName;

            var deleteimageButton = rowView.FindViewById<ImageButton>(Resource.Id.deleteimageButton);

            deleteimageButton.Tag = this.Items.IndexOf(expenseItem);

            deleteimageButton.Click += (s, e) =>
            {
                var index = int.Parse(deleteimageButton.Tag.ToString());
                OnDeleteRequested?.Invoke(this, this.Items[index]);
            };

            switch (expenseItem.ExpenseType)
            {
                case ExpenseTypes.Income:

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorIncome)));
                    break;
                case ExpenseTypes.FixedExpense:

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorFixed)));
                    break;
                case ExpenseTypes.VariableExpense:

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorVariable)));
                    break;
            }
        }

        private void OnRowHeaderLayoutBind(View rowView, ExpenseItem expenseItem)
        {
            string infotext = String.Empty;

            switch (expenseItem.ExpenseType)
            {
                case ExpenseTypes.Income:
                    //_newIncomeView = rowView.FindViewById<NewExpenseView>(Resource.Id.newExpenseView);

                    //_newIncomeView.OnAdded += (s, e) =>
                    //{
                    //    e.ExpenseType = ExpenseTypes.Income;

                    //    OnAdded?.Invoke(this, e);
                    //};
                    rowView.FindViewById<ImageButton>(Resource.Id.newExpenseAddButton).Click += delegate { OnNewExpenseRequested?.Invoke(this, ExpenseTypes.Income); };

                    infotext = Activity.GetString(Resource.String.income_hint);

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorIncome)));
                    break;
                case ExpenseTypes.FixedExpense:

                    //_newFixedExpenseView = rowView.FindViewById<NewExpenseView>(Resource.Id.newExpenseView);


                    //_newFixedExpenseView.OnAdded += (s, e) =>
                    //{
                    //    e.ExpenseType = ExpenseTypes.FixedExpense;

                    //    OnAdded?.Invoke(this, e);
                    //};

                    rowView.FindViewById<ImageButton>(Resource.Id.newExpenseAddButton).Click += delegate { OnNewExpenseRequested?.Invoke(this, ExpenseTypes.FixedExpense); };

                    infotext = Activity.GetString(Resource.String.expense_fixed_hint);

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorFixed)));
                    break;
                case ExpenseTypes.VariableExpense:
                    //_newVariableExpenseView = rowView.FindViewById<NewExpenseView>(Resource.Id.newExpenseView);

                    //_newVariableExpenseView.OnAdded += (s, e) =>
                    //{
                    //    e.ExpenseType = ExpenseTypes.VariableExpense;
                    //    OnAdded?.Invoke(this, e);
                    //};

                    rowView.FindViewById<ImageButton>(Resource.Id.newExpenseAddButton).Click += delegate { OnNewExpenseRequested?.Invoke(this, ExpenseTypes.VariableExpense); };

                    infotext = Activity.GetString(Resource.String.expense_variable_hint);

                    rowView.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Activity, Resource.Color.ColorVariable)));
                    break;
            }

            rowView.FindViewById<TextView>(Resource.Id.textViewHeader).Text = infotext;
        }
    }
}