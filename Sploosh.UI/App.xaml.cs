using ModuleSettings;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Sploosh.Modules.Game;
using Sploosh.Modules.Splash;
using Sploosh.UI.Views;
using System.Windows;

namespace Sploosh.UI
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<SplashModule>();
            moduleCatalog.AddModule<GameModule>();
            moduleCatalog.AddModule<SettingsModule>();
        }
    }
}
