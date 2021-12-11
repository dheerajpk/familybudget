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

namespace FamilyBudget.Droid.PlatformService
{
    public class MessageDialogService
    {
        private static Context _context;

        public static void ShowAlertDialog(string title, string message)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(_context);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton("Ok", (senderAlert, args) =>
            {
                
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public static void SetContext(Context context)
        {
            _context = context;
        }
    }
}