using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Sploosh.ViewModels;



namespace Sploosh
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainViewModel;
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
            backgroundMusicPath = GameConstants.AssemblyDirectory + "/Sounds/BackgroundMusic.mp3";
            squidDeadSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidDead.mp3";
            squidHitSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidHit.mp3";
            squidMissSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidMiss.mp3";

            

            mainViewModel = new MainWindowViewModel();
            this.DataContext = mainViewModel;

            mainViewModel.AttackEvent += AttackMethod;
            mainViewModel.SquidKilledEvent += SquidKilledMethod;

            backgroundPlayer.Open(new Uri(backgroundMusicPath, UriKind.Relative));
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();

            InitializeComponent();
        }

        /// <summary>
        /// This plasy the background music on loop
        /// </summary>
        private void BackgroundMusicEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }


        /// <summary>
        /// When the attack method is called this plays the appropriate 
        /// sound effect for a hit/miss
        /// </summary>
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

        /// <summary>
        /// When the Squid Killed event occurs this plays the appropriate sound effect
        /// </summary>
        private void SquidKilledMethod()
        {
            killedSoundPlayer.Open(new Uri(squidDeadSoundPath, UriKind.Relative));
            killedSoundPlayer.Play();

            ScreenShakeAnimation();
        }

        /// <summary>
        /// This performs an anmination on the mian window- shaking it
        /// </summary>
        private void ScreenShakeAnimation()
        {
            
            DoubleAnimation leftAnimation = new DoubleAnimation();

            leftAnimation.From = this.Left;
            leftAnimation.To = this.Left - 30;
            leftAnimation.Duration = TimeSpan.FromSeconds(0.15);
            leftAnimation.AutoReverse = true;
            mainWindow.BeginAnimation(Window.LeftProperty, leftAnimation);
        }
    }
}
