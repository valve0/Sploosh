using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class QuitViewModel : BindableBase
    {

        public QuitViewModel()
        {
            QuitCommand = new DelegateCommand(QuitGame);
        }

        public DelegateCommand QuitCommand { get; }

        private void QuitGame()
        {
            Application.Current.Shutdown();
        }

    }
}
