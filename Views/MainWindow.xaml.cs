using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Sploosh.ViewModels;
using System.IO;
using HelperClass;
using Path = System.Windows.Shapes.Path;


namespace Sploosh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        private MediaPlayer backgroundPlayer = new MediaPlayer();
        private MediaPlayer hitSoundPlayer = new MediaPlayer();
        private MediaPlayer missSoundPlayer = new MediaPlayer();
        private MediaPlayer killedSoundPlayer = new MediaPlayer();


        private string backgroundMusicPath;
        private string squidDeadSoundPath;
        private string squidHitSoundPath;
        private string squidMissSoundPath;


        public MainWindow()
        { 
            //Sounds
            backgroundMusicPath = FileRepository.AssemblyDirectory + "/Sounds/BackgroundMusic.mp3";
            squidDeadSoundPath = FileRepository.AssemblyDirectory + "/Sounds/SquidDead.mp3";
            squidHitSoundPath = FileRepository.AssemblyDirectory + "/Sounds/SquidHit.mp3";
            squidMissSoundPath = FileRepository.AssemblyDirectory + "/Sounds/SquidMiss.mp3";





            InitializeComponent();
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
            //DataContextChanged += ViewModelSaidDoSomething;
            mainViewModel.AttackEvent += AttackMethod;
            mainViewModel.SquidKilledEvent += SquidKilledMethod;


            backgroundPlayer.Open(new Uri(backgroundMusicPath, UriKind.Relative));
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();
            



        }

        private void SquidKilledMethod()
        {
            killedSoundPlayer.Open(new Uri(squidDeadSoundPath, UriKind.Relative));
            killedSoundPlayer.Play();

            ScreenShakeAnimation();
        }

        private void BackgroundMusicEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }

        private void AttackMethod(bool hit)
        {


            if (hit == true)
            {
                hitSoundPlayer.Open(new Uri(squidHitSoundPath, UriKind.Relative));
                hitSoundPlayer.Play();

                ScreenShakeAnimation();

            }
            else
            {
                //if(missSoundPlayer.CanPause) //If playing

                missSoundPlayer.Open(new Uri(squidMissSoundPath, UriKind.Relative));
                missSoundPlayer.Play();
            }



        }

        private void ScreenShakeAnimation()
        {
            
            DoubleAnimation leftAnimation = new DoubleAnimation();

            leftAnimation.From = this.Left;
            leftAnimation.To = this.Left - 30;
            leftAnimation.Duration = TimeSpan.FromSeconds(0.15);
            leftAnimation.AutoReverse = true;
            mainWindow.BeginAnimation(Window.LeftProperty, leftAnimation);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void ShowSettingsWindow(object sender, RoutedEventArgs e)
        {
            
                SettingsWindow settingsWindow = new SettingsWindow(mainViewModel);
                settingsWindow.ShowDialog();
        }


    }
}
