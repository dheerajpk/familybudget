using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using FamilyBudget.Core.Models;
using FamilyBudget.Core.Services;
using FamilyBudget.Droid.Adapters;
using FamilyBudget.Droid.UIModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.Accounts;
using Android.Support.V4.App;
using Android.Runtime;
using FamilyBudget.Droid.PlatformService;

namespace FamilyBudget.Droid.Views
{
    public class ExpenseFragmentView : Android.Support.V4.App.Fragment, View.IOnKeyListener
    {

        private NewExpenseView _newExpenseView;

        private ProgressBar _progressBar;

        private TextView _familyTips;

        private RelativeLayout _helpRelativeLayout;

        private readonly ExpenseServiceOnline _expenseService = new ExpenseServiceOnline();

        private readonly FamilyService _familyService = new FamilyService();

        private readonly NetworkService _networkService = new NetworkService();

        private ListView _lstvw;


        #region Overrides
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.ExpenseLayout, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.loader);

            _newExpenseView = view.FindViewById<NewExpenseView>(Resource.Id.newExpenseView);

            _familyTips = view.FindViewById<TextView>(Resource.Id.familyTips);

            _lstvw = View.FindViewById<ListView>(Resource.Id.listViewExpenses);

            _helpRelativeLayout = View.FindViewById<RelativeLayout>(Resource.Id.helpRelativeLayout);

            _newExpenseView.OnAdded += (s, e) => HandleOnExpenseAdded(e);

            _newExpenseView.OnCancelled += (s, e) => _newExpenseView.Visibility = ViewStates.Gone;

            FillExpenses();

            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();
            this.View.SetOnKeyListener(this);

        }

        //override bacl
        #endregion


        #region Methods
        private async void HandleOnExpenseAdded(ExpenseParam expenseParam)
        {
            ShowProgress();

            switch (expenseParam.ExpenseType)
            {
                case ExpenseTypes.Income:
                    await _expenseService.AddIncome(expenseParam.Amount, expenseParam.DateTime, expenseParam.CategoryId,
                        _familyService.MemberId, _familyService.FamilyCode, expenseParam.Id,null, expenseParam.Name);
                    //FillIncomeList();
                    break;

                case ExpenseTypes.FixedExpense:
                    await _expenseService.AddFixedExpense(expenseParam.Amount, expenseParam.DateTime, expenseParam.CategoryId,
                        _familyService.MemberId, _familyService.FamilyCode, expenseParam.Id, null, expenseParam.Name).ConfigureAwait(false);
                    //FillFixedList();
                    break;

                case ExpenseTypes.VariableExpense:
                    await _expenseService.AddVariableExpense(expenseParam.Amount, expenseParam.DateTime, expenseParam.CategoryId,
                        _familyService.MemberId, _familyService.FamilyCode, expenseParam.Id, null, expenseParam.Name).ConfigureAwait(false);
                    //FillVariableList();
                    break;
            }

            //MessageDialogService.ShowAlertDialog("Family Budget", $"Expense has been added successfully.");

            await FillAllExpenses().ConfigureAwait(false);

            HideProgress();
        }

        private async void FillExpenses()
        {
            ShowProgress();
            try
            {
                if (!_networkService.IsInternetAvailable)
                    throw new NetworkErrorException("Please check your internet connectivity");

                var isFamilyCodeAvailable = await _familyService.IsFamilyCodeSet().ConfigureAwait(false);

                if (isFamilyCodeAvailable)
                {
                    await _familyService.LoadFamilyDetails(_familyService.FamilyCode);

                    await FillCategories();

                    await FillAllExpenses();
                }
            }
            catch (NetworkErrorException ex)
            {
                MessageDialogService.ShowAlertDialog("Family Budget", ex.Message);
            }
            catch (Exception ex)
            {
                MessageDialogService.ShowAlertDialog("Family Budget", $"Something went wrong...!\n{ex.Message}");
            }

            HideProgress();
        }

        private async Task FillAllExpenses()
        {
            var expensesList = (await _expenseService.GetExpenses(_familyService.FamilyCode).ConfigureAwait(false))
                .Select(x => new ExpenseItem(x, _expenseService.CategoryList, _familyService.Family)).ToList();

            var incomeList = (await _expenseService.GetIncome(_familyService.FamilyCode)).Select(x => new ExpenseItem(x, _expenseService.CategoryList, _familyService.Family))
                .ToList();

            var fixedList = (await _expenseService.GetFixedExpenses(_familyService.FamilyCode))
                .Select(x => new ExpenseItem(x, _expenseService.CategoryList, _familyService.Family)).ToList();

            var variableList = (await _expenseService.GetVariableExpenses(_familyService.FamilyCode))
                .Select(x => new ExpenseItem(x, _expenseService.CategoryList, _familyService.Family)).ToList();

            List<ExpenseItem> expenseItems = new List<ExpenseItem>
            {
                new ExpenseItem(ExpenseTypes.Income)
            };

            if (incomeList.Any()) expenseItems.AddRange(incomeList);

            expenseItems.Add(new ExpenseItem(ExpenseTypes.FixedExpense));

            if (fixedList.Any()) expenseItems.AddRange(fixedList);

            expenseItems.Add(new ExpenseItem(ExpenseTypes.VariableExpense));

            if (variableList.Any()) expenseItems.AddRange(variableList);

            this.Activity.RunOnUiThread(async () =>
            {
                _helpRelativeLayout.Visibility = expensesList.Any() ? ViewStates.Gone : ViewStates.Visible;

                var total = await _expenseService.GetTotalBalance(_familyService.FamilyCode);

                _familyTips.Text = $"{total.ToString("N", CultureInfo.InvariantCulture)} balance left";

                if (_lstvw == null) return;

                _lstvw.Adapter = new ExpenseAdapter(expenseItems, this.Activity);

                ((ExpenseAdapter)_lstvw.Adapter).OnNewExpenseRequested += (s, e) =>
                {
                    _newExpenseView.SetExpenseParam(new ExpenseParam()
                    {
                        ExpenseType = e
                    });

                    _newExpenseView.Visibility = ViewStates.Visible;
                };

                ((ExpenseAdapter)_lstvw.Adapter).OnDeleteRequested += async (s, e) =>
                   {
                       ShowProgress();

                       await _expenseService.RemoveExpense(e.Id);

                       await FillAllExpenses();

                       HideProgress();
                   };
            });
        }

        private async Task FillCategories()
        {
            await _expenseService.GetAllCategories();
        }

        private void ShowProgress()
        {
            this.Activity.RunOnUiThread(() =>
            {
                _progressBar.Visibility = ViewStates.Visible;
                _progressBar.BringToFront();
                this.View.Invalidate();
            });
        }

        private void HideProgress()
        {
            this.Activity.RunOnUiThread(() =>
            {
                _progressBar.Visibility = ViewStates.Gone;
            });
        }

        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (_newExpenseView.Visibility == ViewStates.Visible && keyCode == Keycode.Back)
            {
                _newExpenseView.Visibility = ViewStates.Gone;
                return false;
            }

            return true;
        }

        public bool OnBackeyPress()
        {
            if (_newExpenseView.Visibility == ViewStates.Visible)
            {
                _newExpenseView.Visibility = ViewStates.Gone;
                return true;
            }

            return false;
        }

        public void Refresh()
        {
            FillExpenses();
        }
        #endregion
    }
}