using Prism.Mvvm;
using Sploosh.Services;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class HelpViewModel : BindableBase
    {
        private ImageSource _help1;
        private ImageSource _help2;
        private ImageSource _help3;

        public HelpViewModel()
        {
            Help1 = new BitmapImage(EnvironmentVariables.HelpImage1);
            Help2 = new BitmapImage(EnvironmentVariables.HelpImage2);
            Help3 = new BitmapImage(EnvironmentVariables.HelpImage3);
        }

        public ImageSource Help1
        {
            get { return _help1; }
            set { SetProperty(ref _help1, value); }
        }

        public ImageSource Help2
        {
            get { return _help2; }
            set { SetProperty(ref _help2, value); }
        }

        public ImageSource Help3
        {
            get { return _help3; }
            set { SetProperty(ref _help3, value); }
        }

    }
}
