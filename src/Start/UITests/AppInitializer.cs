using Xamarin.UITest;

namespace XamAppCenterSample2018.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .EnableLocalScreenshots()
                    .PreferIdeSettings()
                    .InstalledApp("com.hiro128777.XamAppCenterSample2018")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .EnableLocalScreenshots()
                .PreferIdeSettings()
                .InstalledApp("com.hiro128777.XamAppCenterSample2018")
                .StartApp();
        }
    }
}
