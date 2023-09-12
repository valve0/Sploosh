using System;
using System.Threading.Tasks;
using Sploosh.UI.Commands;

namespace Sploosh.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public SettingsViewModel(SoundViewModel soundViewModel, 
            HelpViewModel helpViewModel,
            QuitViewModel quitViewModel,
            RestartViewModel restartViewModel,
            AboutViewModel aboutViewModel)
        {

            ShowSelectedViewCommand = new DelegateCommand(SelectViewModel);

            SoundViewModel = soundViewModel;
            HelpViewModel = helpViewModel;
            QuitViewModel = quitViewModel;
            RestartViewModel = restartViewModel;
            AboutViewModel = aboutViewModel;

            SoundViewModel.EnableDisableSoundEffectsEvent += EnableDisableSoundEffectsEvent;
        }

        private void EnableDisableSoundEffectsEvent()
        {
            EnableDisableSoundEffectsEvent2?.Invoke();
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

        public SoundViewModel SoundViewModel { get; }
        public HelpViewModel HelpViewModel { get; }
        public QuitViewModel QuitViewModel { get; }
        public RestartViewModel RestartViewModel { get; }
        public AboutViewModel AboutViewModel { get; }

        public DelegateCommand ShowSelectedViewCommand { get; }

        public delegate void SoundEffectsAction();
        public event SoundEffectsAction? EnableDisableSoundEffectsEvent2;


        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
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