using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FamilyBudget.Core.Models;
using FamilyBudget.Core.Services;

namespace FamilyBudget.Droid.Views
{
    public class NewExpenseView : LinearLayout, Android.App.DatePickerDialog.IOnDateSetListener
    {
        private ExpenseParam _expense;
        private readonly Android.Support.V4.App.FragmentManager _fragmentManager;

        private TextView dateEditText;
        private Spinner categorySpinner;
        private EditText amountEditText;
        private EditText nameEditText;
        private List<Category> categories;
        private ImageButton _dateImageButton;

        public event EventHandler<ExpenseParam> OnAdded;
        public event EventHandler OnCancelled;

        public NewExpenseView(Context context, Android.Support.V4.App.FragmentManager fragmentManager) : base(context)
        {
            _fragmentManager = fragmentManager;

            Initialize(context);
        }
        public NewExpenseView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            _fragmentManager = ((Android.Support.V4.App.FragmentActivity)context).SupportFragmentManager;
            Initialize(context);
        }

        public NewExpenseView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            _expense.DateTime = view.DateTime;

            dateEditText.Text = view.DateTime.ToString("D", CultureInfo.InvariantCulture);
        }

        private void Initialize(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            var newview = inflater.Inflate(Resource.Layout.NewExpenseLayout, null);

            this.AddView(newview);

            var cancelButton = newview.FindViewById<Button>(Resource.Id.cancelButton);

            cancelButton.Click += delegate
            {
                HideKeyboard(context);
                this.Visibility = ViewStates.Gone;
                OnCancelled?.Invoke(this, null);
            };

            var addButton = newview.FindViewById<Button>(Resource.Id.addButton);

            addButton.Click += delegate
            {
                HideKeyboard(context);
                this.Visibility = ViewStates.Gone;
                OnAdded?.Invoke(this, GetExpenseParam());
            };


            dateEditText = newview.FindViewById<TextView>(Resource.Id.dateEditText);

            categorySpinner = newview.FindViewById<Spinner>(Resource.Id.categorySpinner);

            amountEditText = newview.FindViewById<EditText>(Resource.Id.amountEditText);

            nameEditText = newview.FindViewById<EditText>(Resource.Id.nameEditText);

            _dateImageButton = newview.FindViewById<ImageButton>(Resource.Id.dateImageButton);

            dateEditText.Click -= DateEditText_Click;

            dateEditText.Click += DateEditText_Click;

            _dateImageButton.Click -= DateEditText_Click;

            _dateImageButton.Click += DateEditText_Click;

            SetExpenseValues();

            amountEditText.SelectAll();
        }

        private void SetExpenseValues()
        {
           
            dateEditText.Text = _expense?.DateTime.ToString("D", CultureInfo.InvariantCulture) ??
                                DateTime.Now.ToString("D", CultureInfo.InvariantCulture);

            SetCategories(categorySpinner);

            amountEditText.Text = _expense?.Amount.ToString(CultureInfo.InvariantCulture) ?? string.Empty;

            nameEditText.Text = _expense?.Name ?? string.Empty;


        }

        private void DateEditText_Click(object sender, EventArgs e)
        {
            var dialog = new Views.DatePickerDialogFragment(this.Context, DateTime.Now, this);

            dialog.Show(_fragmentManager, null);
        }

        private void SetCategories(Spinner spinner)
        {
            ExpenseServiceOffline expenseService = new ExpenseServiceOffline();
            categories = expenseService.GetAllCategories();

            var adapter = new ArrayAdapter<Category>(this.Context, Resource.Layout.SpinnerItem, categories);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            if (_expense != null)
            {
                var selectedItem = categories.FirstOrDefault(x => x.Id == _expense.CategoryId);

                spinner.SetSelection(categories.IndexOf(selectedItem));
            }
        }

        private ExpenseParam GetExpenseParam()
        {
            return new ExpenseParam()
            {
                Amount = Decimal.Parse(amountEditText.Text),
                DateTime = _expense.DateTime,
                CategoryId = categories[categorySpinner.SelectedItemPosition].Id,
                Id = _expense?.Id,
                Name = nameEditText.Text,
                ExpenseType = _expense.ExpenseType
            };
        }

        public void SetExpenseParam(ExpenseParam expenseParam)
        {
            _expense = expenseParam;

            SetExpenseValues();

            switch (_expense.ExpenseType)
            {
                case ExpenseTypes.Income:

                    this.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Context, Resource.Color.ColorIncome)));
                    break;
                case ExpenseTypes.FixedExpense:

                    this.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Context, Resource.Color.ColorFixed)));
                    break;
                case ExpenseTypes.VariableExpense:

                    this.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.Context, Resource.Color.ColorVariable)));
                    break;
            }
        }

        private void HideKeyboard(Context context)
        {

            InputMethodManager inputManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);

            inputManager.HideSoftInputFromWindow(amountEditText.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}