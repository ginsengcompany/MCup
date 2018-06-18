using Android.App;
using Android.OS;
using Android.Content.PM;
using System;
using System.Threading;
using System.Threading.Tasks;
using MCup.Droid;

namespace QuizAmoroso.Droid
{
    [Activity(Label = "ecupt Sant'Anna e San Sebastiano Caserta", Icon = "@drawable/icon", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            await Task.Delay(1000);
            StartActivity(typeof(MainActivity));
        }
    }
}