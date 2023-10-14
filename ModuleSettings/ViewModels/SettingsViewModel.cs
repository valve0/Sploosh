using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase
    {

        private IRegionManager _regionManager;

        public SettingsViewModel(IRegionManager regionManager)
        {

            _regionManager = regionManager;

            OpenSettingsDetails = new DelegateCommand<string>(Navigate);

        }

        public DelegateCommand<String> OpenSettingsDetails { get; }

        private void Navigate(string viewName)
        {
            _regionManager.RequestNavigate("SettingsDetailsRegion", viewName);
        }

    }
}