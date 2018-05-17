using Foundation;
using UIKit;

using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;

namespace XamAppCenterSample2018.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
    {

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            // Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            return base.FinishedLaunching(application, launchOptions);
        }

    }
}
