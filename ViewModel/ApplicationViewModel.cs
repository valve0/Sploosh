using Sploosh.Model;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Sploosh.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public ApplicationViewModel(GameViewModel gameViewModel,
      SettingsViewModel settingsViewModel)
        {
            
            GameViewModel = gameViewModel;
            SettingsViewModel = settingsViewModel;

            SelectedViewModel = GameViewModel;


            ShowSettingsWindowCommand = new DelegateCommand(SelectViewModel);    

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
                await LoadAsync();
            }
            else
            {
                SelectedViewModel = parameter as ViewModelBase;
                await LoadAsync();
            }

        }
    }
}
