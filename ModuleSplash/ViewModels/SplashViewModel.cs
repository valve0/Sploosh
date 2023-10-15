using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sploosh.UI.Events;
using System;
using System.Windows.Threading;

namespace Sploosh.Modules.Splash.ViewModels
{
    public class SplashViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private DispatcherTimer _timer;

        public SplashViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            //Timer to control how long splash screen active
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(6); // Wait 6 seconds
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        // This code will run on the UI thread due to DispatcherTimer
        private void Timer_Tick(object timer, EventArgs e)
        {        
            _eventAggregator.GetEvent<RestartGameEvent>().Publish();

            _timer.Stop();
        }
    }
}
