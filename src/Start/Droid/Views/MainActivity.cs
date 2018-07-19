using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Binding;
using XamAppCenterSample2018.ViewModels;

namespace XamAppCenterSample2018.Droid
{
    [Activity(Label = "XamAppCenterSample2018", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity :Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

    }
}

