using Microsoft.Extensions.DependencyInjection;
using Sploosh.View;
using Sploosh.ViewModel;
using System.Windows;

namespace Sploosh
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            //Register the services
 
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<HelpViewModel>();
            services.AddTransient<QuitViewModel>();
            services.AddTransient<RestartViewModel>();
            services.AddTransient<SoundViewModel>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();

        }
    }
}
