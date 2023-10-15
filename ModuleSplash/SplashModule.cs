using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Sploosh.Modules.Splash.Views;

namespace Sploosh.Modules.Splash
{
    public class SplashModule : IModule
    {

        public SplashModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SplashView));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
