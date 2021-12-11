using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using FamilyBudget.Core.Services;
using FamilyBudget.Droid.Adapters;
using FamilyBudget.Droid.Views;
using System;
using FamilyBudget.Droid.PlatformService;

namespace FamilyBudget.Droid
{
    [Activity(Label = "Family Budget", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        private TabLayout _tabLayout;

        private ViewPager _viewPager;

        private ExpenseFragmentView _expenseFragment;

        //private SummaryFragmentView _summaryFragmentView;

        private SettingsFragmentView _settingsFragmentView;

        private FamilyService _familyService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            PlatformService.MessageDialogService.SetContext(this);

            PlatformService.NetworkService.SetContext(this);

            _familyService = new FamilyService();

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            SetupViewPager(_viewPager);

            _tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            _tabLayout.SetupWithViewPager(_viewPager);
        }



        private void SetupViewPager(ViewPager viewPager1)
        {
            InitialiseFragment();
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.AddFragment(_expenseFragment, "Expense");
            //adapter.AddFragment(_summaryFragmentView, "Summary");
            adapter.AddFragment(_settingsFragmentView, "Settings");

            _viewPager.Adapter = adapter;

            CheckFamily();
        }

        private void InitialiseFragment()
        {
            _expenseFragment = new ExpenseFragmentView();
            //_summaryFragmentView = new SummaryFragmentView();
            _settingsFragmentView = new SettingsFragmentView();
        }

        private async void CheckFamily()
        {
            try
            {
                var isFamilyCodeAvailable = await _familyService.IsFamilyCodeSet().ConfigureAwait(false);

                this.RunOnUiThread(() =>
                {
                    if (!isFamilyCodeAvailable)
                    {
                        _viewPager.SetCurrentItem(1, true);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageDialogService.ShowAlertDialog("Family Budget", $"Something went wrong...!\n{ex.Message}");
            }
        }

        internal void SetView(int viewindex)
        {
            _viewPager.SetCurrentItem(viewindex, true);

            _expenseFragment.Refresh();
        }

        public override void OnBackPressed()
        {
            if (_viewPager.CurrentItem == 0 && _expenseFragment.OnBackeyPress())
            {
                return;
            }
            if (_viewPager.CurrentItem == 1 && _settingsFragmentView.OnBackeyPress())
            {
                return;
            }

            base.OnBackPressed();
        }
    }
}

