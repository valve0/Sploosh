using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sploosh.ViewModels
{
    class SettingsViewModel
    {
       public ICommand QuitCommand { get; set; }

       public SettingsViewModel()
       {
           QuitCommand = new RelayCommand(QuitApplication, CanQuitApplication);
       }

        private bool CanQuitApplication(object obj)
        {
            return true;
        }

        private void QuitApplication(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
