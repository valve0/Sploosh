using System.Threading.Tasks;

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
      SelectViewModelCommand = new DelegateCommand(SelectViewModel);
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

    public GameViewModel GameViewModel { get; }

    public SettingsViewModel SettingsViewModel { get; }

    public DelegateCommand SelectViewModelCommand { get; }

    public async override Task LoadAsync()
    {
      if(SelectedViewModel is not null)
      {
        await SelectedViewModel.LoadAsync();
      }
    }

    private async void SelectViewModel(object? parameter)
    {
      SelectedViewModel = parameter as ViewModelBase;
      await LoadAsync();
    }
  }
}
