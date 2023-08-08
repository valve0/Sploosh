
namespace Sploosh.ViewModel
{
    public class SoundViewModel : ViewModelBase
    {
        public SoundViewModel()
        {
            SoundEffectsCommand = new DelegateCommand(SoundEffects);
        }

        private void SoundEffects(object? parameter)
        {
            //Raise event to diable sound effects
            if(parameter == null)
            {
                EnableDisableSoundEffectsEvent?.Invoke(false);
            }
            else
                EnableDisableSoundEffectsEvent?.Invoke(true);
        }

        public DelegateCommand SoundEffectsCommand { get; }

        public delegate void SoundEffectsAction(bool state);
        public event SoundEffectsAction? EnableDisableSoundEffectsEvent;

    }
}
