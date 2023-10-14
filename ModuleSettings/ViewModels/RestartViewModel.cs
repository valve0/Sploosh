using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Sploosh.UI.Events;
using System;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class RestartViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;

        public RestartViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            
            RestartCommand = new DelegateCommand(RestartGame);

            
        }

        public DelegateCommand RestartCommand { get; private set; }

        private void RestartGame()
        {
            //Send Retart event
            _eventAggregator.GetEvent<RestartGameEvent>().Publish();



        }
    }
}
