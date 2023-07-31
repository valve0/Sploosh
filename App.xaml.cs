using Sploosh.View;
using Sploosh.ViewModel;
using System.Windows;

namespace Sploosh
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow(new MainViewModel(new GameViewModel(),
                new SettingsViewModel()));
            mainWindow.Show();

        }
    }
}
