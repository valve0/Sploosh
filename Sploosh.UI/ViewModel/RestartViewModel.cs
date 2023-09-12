using Sploosh.Resources;
using Sploosh.UI.Commands;
using System;

namespace Sploosh.ViewModel
{
    public class RestartViewModel : ViewModelBase
    {

        public RestartViewModel(UserSettings userSettings)
        {
            UserSettings = userSettings;

            RestartCommand = new DelegateCommand(RestartGame);
        }

        public UserSettings UserSettings { get; }

        public DelegateCommand RestartCommand { get; }

        private void RestartGame(object? obj)
        {
            UserSettings.UpdateRestartGameTrigger();
        }
    }
}
