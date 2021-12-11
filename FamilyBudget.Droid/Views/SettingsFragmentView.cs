using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FamilyBudget.Core.Services;

namespace FamilyBudget.Droid.Views
{
    public class SettingsFragmentView : Android.Support.V4.App.Fragment
    {
        private readonly FamilyService _familyService;

        private LinearLayout _newFamilyLayout;

        private RelativeLayout _setupFamilyRelativeLayout;

        private LinearLayout _familCodeyRelativeLayout;

        private EditText _familyEditText;

        private Button _newButton;

        private Button _joinButton;

        private Button _doneButton;

        private Button _inviteButton;

        private EditText _memberNameEditText;

        private ProgressBar _progressBar;


        public SettingsFragmentView()
        {
            _familyService = new FamilyService();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.SettingsLayout, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            _newFamilyLayout = view.FindViewById<LinearLayout>(Resource.Id.NewFamilyLayout);

            _setupFamilyRelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.SetupFamilyRelativeLayout);

            _familCodeyRelativeLayout = this.View.FindViewById<LinearLayout>(Resource.Id.familCodeRelativeLayout);

            _familyEditText = view.FindViewById<EditText>(Resource.Id.familyEditText);

            _memberNameEditText = view.FindViewById<EditText>(Resource.Id.memberNameEditText);

            _newButton = view.FindViewById<Button>(Resource.Id.newButton);

            _joinButton = view.FindViewById<Button>(Resource.Id.joinButton);

            _doneButton = view.FindViewById<Button>(Resource.Id.doneButton);

            _inviteButton = view.FindViewById<Button>(Resource.Id.inviteButton);

            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.loader);

            RegisterButtonActions();

            base.OnViewCreated(view, savedInstanceState);

            CheckFamily();
        }

        private void RegisterButtonActions()
        {
            _newButton.Click += delegate
            {
                _doneButton.Tag = "NewFamily";
                _newFamilyLayout.Visibility = ViewStates.Visible;
                _setupFamilyRelativeLayout.Visibility = ViewStates.Gone;
                _familyEditText.Visibility = ViewStates.Visible;
                //_memberNameEditText.Visibility = ViewStates.Gone;
            };

            _joinButton.Click += delegate
            {
                _doneButton.Tag = "JoinFamily";
                _familyEditText.Hint = "Family Code";
                _newFamilyLayout.Visibility = ViewStates.Visible;
                _setupFamilyRelativeLayout.Visibility = ViewStates.Gone;
                _familyEditText.Visibility = ViewStates.Visible;
                _memberNameEditText.Visibility = ViewStates.Visible;
            };

            _doneButton.Click += delegate
            {
                _newFamilyLayout.Visibility = ViewStates.Gone;
                _setupFamilyRelativeLayout.Visibility = ViewStates.Gone;
                HandleDoneButtonAction();
            };

            _inviteButton.Click += delegate { InviteFamily(); };
        }

        private void InviteFamily()
        {
            try
            {
                Intent sendIntent = new Intent();
                sendIntent.SetAction(Intent.ActionSend);
                sendIntent.PutExtra(Intent.ExtraText, $"Hey, you're invited to join Family Budget App.\nPlease copy & paste FAMILY CODE below to join your family\n{_familyService.FamilyCode}");
                sendIntent.SetType("text/plain");
                StartActivity(sendIntent);
            }
            catch
            {
                //TODO : Nothing todo for now.:)
            }
        }

        private async void CheckFamily()
        {
            var isFamilyCodeAvailable = await _familyService.IsFamilyCodeSet().ConfigureAwait(false);

            this.Activity.RunOnUiThread(() =>
            {
                _setupFamilyRelativeLayout.Visibility = isFamilyCodeAvailable ? ViewStates.Gone : ViewStates.Visible;

                if (isFamilyCodeAvailable)
                {
                    
                    var familyCodeTextView = this.View.FindViewById<TextView>(Resource.Id.familyCodeTextView);

                    _familCodeyRelativeLayout.Visibility = ViewStates.Visible;

                    familyCodeTextView.Text = _familyService.FamilyCode;
                }
            });
        }


        private async void HandleDoneButtonAction()
        {
            ShowProgress();

            HideKeyboard(Context);

            if (_doneButton.Tag.ToString() == "JoinFamily")
            {
                var familyCode = _familyEditText.Text;

                var memberName = _memberNameEditText.Text;

                await _familyService.JoinFamily(familyCode, memberName);
            }
            else
            {
                var familyName = _familyEditText.Text;

                var memberName = _memberNameEditText.Text;

                await _familyService.SetUpFamily(familyName, memberName);
            }

            CheckFamily();

            (Context as MainActivity)?.SetView(0);

            HideProgress();
        }

        private void ShowProgress()
        {
            _progressBar.Visibility = ViewStates.Visible;
        }

        private void HideProgress()
        {
            _progressBar.Visibility = ViewStates.Invisible;
        }

        private void HideKeyboard(Context context)
        {
            InputMethodManager inputManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);

            inputManager.HideSoftInputFromWindow(_familyEditText.WindowToken, HideSoftInputFlags.NotAlways);
        }

        public bool OnBackeyPress()
        {
            if (_newFamilyLayout.Visibility == ViewStates.Visible )
            {
                _newFamilyLayout.Visibility = ViewStates.Gone;

                _setupFamilyRelativeLayout.Visibility = ViewStates.Visible;

                return true;
            }

            return false;
        }
    }
}