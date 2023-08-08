using Sploosh.Model;
using Sploosh.View;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Sploosh.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(GameViewModel gameViewModel,
      SettingsViewModel settingsViewModel)
        {
            GameViewModel = gameViewModel;
            SettingsViewModel = settingsViewModel;
            SelectedViewModel = gameViewModel;
            ShowSettingsWindowCommand = new DelegateCommand(SelectViewModel);

            BackgroundImagePath = new BitmapImage(GameModel.BackgroundImage);

            MotifImagePath = new BitmapImage(GameModel.MotifImage);

            GameViewModel.ScreenShakeAnimationEvent += ScreenShakeAnimation;

        }

        private void ScreenShakeAnimation()
        {
            ScreenShakeAnimationEvent1?.Invoke();
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

        public delegate void AttackEventAction();
        public event AttackEventAction? ScreenShakeAnimationEvent1;

        public ImageSource BackgroundImagePath { get; private set; }

        public ImageSource MotifImagePath { get; private set; }

        public GameViewModel GameViewModel { get; }

        public SettingsViewModel SettingsViewModel { get; }

        public DelegateCommand ShowSettingsWindowCommand { get; }


        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        private async void SelectViewModel(object? parameter)
        {
            //If the current selecetd view model is already the settings- select the Gameview Model
            if (SelectedViewModel == parameter as ViewModelBase)
            {
                SelectedViewModel = GameViewModel;
            }
            else
            {
                SelectedViewModel = parameter as ViewModelBase;
                await LoadAsync();
            }

        }
    }
}
