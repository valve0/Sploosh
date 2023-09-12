using System.Windows.Media;

namespace Sploosh.ViewModel
{
    public class HelpViewModel : ViewModelBase
    {


        private ImageSource _help1;
        private ImageSource _help2;
        private ImageSource _help3;


        public HelpViewModel()
        {
            //Help1 = new BitmapImage(GameState.HelpImage1);
            //         Help2 = new BitmapImage(GameState.HelpImage2);
            //         Help3 = new BitmapImage(GameState.HelpImage3);
        }

        public ImageSource Help1
        {
            get { return _help1; }
            set
            {
                _help1 = value;
                RaisePropertyChanged();
            }
        }



        public ImageSource Help2
        {
            get { return _help2; }
            set
            {
                _help2 = value;
                RaisePropertyChanged();
            }
        }



        public ImageSource Help3
        {
            get { return _help3; }
            set
            {
                _help3 = value;
                RaisePropertyChanged();
            }
        }



    }
}
