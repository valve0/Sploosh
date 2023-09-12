using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace Sploosh.View
{

    public partial class SplashView : UserControl
    {
        public SplashView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard translateStoryboard = (Storyboard)FindResource("TranslateStoryboard");
            translateStoryboard.Begin();
        }
    }
}
