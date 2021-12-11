
using Android.App;
using Android.Content;
using Android.Net;

namespace FamilyBudget.Droid.PlatformService
{
    public class NetworkService
    {
        private ConnectivityManager _connectivityManager;

        private static Context _context;

        public bool IsConnected()
        {
            _connectivityManager = (ConnectivityManager)_context.GetSystemService(Context.ConnectivityService);
            return _connectivityManager.ActiveNetworkInfo?.IsConnected ?? false;
        }

        public bool IsInternetAvailable => IsConnected();

        public static void SetContext(Context context)
        {
            _context = context;
        }
    }
}