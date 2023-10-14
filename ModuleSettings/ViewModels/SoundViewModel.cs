using Prism.Events;
using Prism.Mvvm;
using Sploosh.UI.Events;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class SoundViewModel : BindableBase
    {
        private bool _isToggleChecked;
        private IEventAggregator _eventAggregator;

        public SoundViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

        }

        public bool IsToggleChecked
        {
            get { return _isToggleChecked; }
            set
            {
                SetProperty(ref _isToggleChecked, value);
                _eventAggregator.GetEvent<SoundStateEvent>().Publish("SoundEffectsToggle");
            }
        }

    }
}
