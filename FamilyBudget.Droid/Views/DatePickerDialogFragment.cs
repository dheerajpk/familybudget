using System;
using Android.App;
using Android.Content;
using Android.OS;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace FamilyBudget.Droid.Views
{
    internal class DatePickerDialogFragment : DialogFragment
    {
        private Context _context;
        private DateTime _date;
        private readonly Android.App.DatePickerDialog.IOnDateSetListener _listener;

        public DatePickerDialogFragment(Context context, DateTime now, Android.App.DatePickerDialog.IOnDateSetListener listener)
        {
            _context = context;
            _date = now;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new Android.App.DatePickerDialog(_context, _listener, _date.Year, _date.Month - 1, _date.Day);
            return dialog;
            
        }
    }
}