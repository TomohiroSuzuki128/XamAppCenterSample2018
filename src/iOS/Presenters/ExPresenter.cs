using CoreAnimation;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace XamAppCenterSample2018.iOS.Presenters
{
    public class ExPresenter : MvxIosViewPresenter
    {
        public ExPresenter(IUIApplicationDelegate appDelegate, UIWindow window)
            : base(appDelegate, window)
        {
        }

        protected override MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            var navigationController = new MvxNavigationController(viewController);
            navigationController.NavigationBarHidden = true;
            return navigationController;
        }

    }
}