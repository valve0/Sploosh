using Sploosh.Model;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Sploosh.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ViewModelBase? _selectedViewModel;

        private DispatcherTimer _timer;


        public MainViewModel(SplashViewModel splashViewModel, ApplicationViewModel applicationViewModel)
        {
            SplashViewModel = splashViewModel; 
            ApplicationViewModel = applicationViewModel;
            
            SelectedViewModel = SplashViewModel;

            ApplicationViewModel.ScreenShakeAnimationEvent1 += ScreenShakeAnimation;

            BackgroundImagePath = new BitmapImage(GameModel.BackgroundImage);

            //Timer to control how long splash screen active
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(6); // Change to your desired interval
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
            }
        }

        public SplashViewModel SplashViewModel { get; }

        public ApplicationViewModel ApplicationViewModel { get; }

        public ImageSource BackgroundImagePath { get; private set; }

        public delegate void AttackEventAction();
        public event AttackEventAction? ScreenShakeAnimationEvent2;

        private void Timer_Tick(object sender, EventArgs e)
        {
            // This code will run on the UI thread due to DispatcherTimer
            SelectedViewModel = ApplicationViewModel;
        }

        private void ScreenShakeAnimation()
        {
            ScreenShakeAnimationEvent2?.Invoke();
        }

    }
}
