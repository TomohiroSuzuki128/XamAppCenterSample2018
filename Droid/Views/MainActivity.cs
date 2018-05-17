using Android.App;
using Android.Views;
using Android.OS;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Binding;
using XamAppCenterSample2018.ViewModels;

namespace XamAppCenterSample2018.Droid
{
    [Activity(Label = "XamAppCenterSample2018", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : MvxActivity<MainViewModel>
    {
		protected override void OnCreate(Bundle bundle)
        {
			base.OnCreate(bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);          
        }

    }
}

