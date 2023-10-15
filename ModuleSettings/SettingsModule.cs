using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Sploosh.Modules.Settings.Views;

namespace ModuleSettings
{
    public class SettingsModule : IModule
    {
        private IRegionManager _regionManager;

        public SettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SettingsView));
        }


        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<HelpView>();
            containerRegistry.RegisterForNavigation<QuitView>();
            containerRegistry.RegisterForNavigation<RestartView>();
            containerRegistry.RegisterForNavigation<SoundView>();
        }
    }
}
