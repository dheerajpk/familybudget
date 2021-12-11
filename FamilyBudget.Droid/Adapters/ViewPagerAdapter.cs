using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace FamilyBudget.Droid.Adapters
{
   public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentList = new List<Android.Support.V4.App.Fragment>();

        private List<string> mFragmentTitleList = new List<string>();

        public ViewPagerAdapter(Android.Support.V4.App.FragmentManager manager) : base(manager)
        {
            //base.OnCreate(manager);
        }

        public override int Count
        {
            get
            {
                return mFragmentList.Count;
            }
        }
        public override Android.Support.V4.App.Fragment GetItem(int postion)
        {
            return mFragmentList[postion];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {

            return new Java.Lang.String(mFragmentTitleList[position].ToLower());// display the title
            //return null;// display only the icon
        }

        public void AddFragment(Android.Support.V4.App.Fragment fragment, string title)
        {
            mFragmentList.Add(fragment);
            mFragmentTitleList.Add(title);
        }
    }
}