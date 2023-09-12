using Sploosh.Resources;
using Sploosh.UI.Commands;

namespace Sploosh.ViewModel
{
    public class SoundViewModel : ViewModelBase
    {

        private bool _isChecked;

        public SoundViewModel(UserSettings userSettings)
        {
            UserSettings = userSettings;

            SoundEffectsCommand = new DelegateCommand(SoundEffects);

        }


        public UserSettings UserSettings { get;}

        public DelegateCommand SoundEffectsCommand { get; }

        public delegate void SoundEffectsAction();
        public event SoundEffectsAction? EnableDisableSoundEffectsEvent;

        

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }


        private void SoundEffects(object? parameter)
        {
            UserSettings.UpdateSoundEffectStatus();
        }


    }
}
