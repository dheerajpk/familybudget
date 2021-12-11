using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FamilyBudget.Droid.Services;

namespace FamilyBudget.Droid
{
    [Application]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            FamilyBudget.Core.Services.StorageService.Initialise(new StoragePlatformService());



            base.OnCreate();
        }
    }
}