using System.Windows;

namespace Sploosh.ViewModel
{
    public class QuitViewModel : ViewModelBase
    {

        public QuitViewModel()
        {
            QuitCommand = new DelegateCommand(QuitGame);
        }

        public DelegateCommand QuitCommand { get; }

        private void QuitGame(object? obj)
        {
            Application.Current.Shutdown();
        }
    }
}
