using HelperClass;
using Sploosh.Model;
using Sploosh.Models;
using Sploosh.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace Sploosh.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private MainWindow _window;

        private int _selectedIndex = -1;
        private int _shotCounter;
        private int _numberOfSquid;
        private int _highScore;
        private ObservableCollection<ImageHolder> _squidsLeftImages;
        private ObservableCollection<ImageHolder> _bombImages;
        private ObservableCollection<Square> _oneDSquares;
        private ObservableCollection<Squid> _squids;
        private string _feedback;

        private MediaPlayer backgroundPlayer = new();
        private MediaPlayer hitSoundPlayer = new();
        private MediaPlayer missSoundPlayer = new();
        private MediaPlayer killedSoundPlayer = new();


        private string backgroundMusicPath;
        private string squidDeadSoundPath;
        private string squidHitSoundPath;
        private string squidMissSoundPath;

        public GameViewModel()
        {
            //_window = window;
            GameState.SetupGame(this);

            //Remove test string
            Feedback = "";

            BackgroundImagePath = new BitmapImage(GameConstants.BackgroundImage);

            //Sounds
            backgroundMusicPath = GameConstants.AssemblyDirectory + "/Sounds/BackgroundMusic.mp3";
            squidDeadSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidDead.mp3";
            squidHitSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidHit.mp3";
            squidMissSoundPath = GameConstants.AssemblyDirectory + "/Sounds/SquidMiss.mp3";


            backgroundPlayer.Open(new Uri(backgroundMusicPath, UriKind.Relative));
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();
            //_window = window;
        }

        //public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public delegate void AttackEventAction(bool hit);
        public event AttackEventAction? AttackEvent;

        public delegate void SquidKilledEventAction();
        public event SquidKilledEventAction? SquidKilledEvent;


        public ImageSource BackgroundImagePath { get; private set; }

        public int NumberOfSquid
        {
            get { return _numberOfSquid; }
            set
            {
                _numberOfSquid = value;
                RaisePropertyChanged();
            }
        }

        public int ShotCounter
        {
            get { return _shotCounter; }
            set
            {
                _shotCounter = value;
                RaisePropertyChanged();
            }
        }

        public int Highscore
        {
            get { return _highScore; }
            set
            {
                _highScore = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Square> OneDSquares
        {
            get { return _oneDSquares; }
            set
            {
                _oneDSquares = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Squid> Squids
        {
            get { return _squids; }
            set
            {
                _squids = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ImageHolder> BombImages
        {
            get { return _bombImages; }
            set
            {
                _bombImages = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ImageHolder> SquidsLeftImages
        {
            get { return _squidsLeftImages; }
            set
            {
                _squidsLeftImages = value;
                RaisePropertyChanged();
            }
        }

        public string Feedback
        {
            get { return _feedback; }
            set
            {
                _feedback = value;
                RaisePropertyChanged();
            }
        }


        public int SquidKillIndex { get; set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value)
                    return;

                _selectedIndex = value;
                RaisePropertyChanged();

                Debug.WriteLine($"Selected Index: {_selectedIndex}");


                if (_selectedIndex != -1 && OneDSquares[_selectedIndex].AttackStatus == true) // -1 = setup value
                    AttackSquare(_selectedIndex);

            }
        }


        /// <summary>
        /// When called this resets the application so the user can play the game again
        /// </summary>
        public void RestartGame(GameViewModel vm)
        {
            GameState.SetupGame(vm);
        }


        /// <summary>
        /// Forces an update of the displayed properties in the main window
        /// </summary>
        private void UpdateMainWindow()
        {
            BombImages = new ObservableCollection<ImageHolder>(BombImages);
            OneDSquares = new ObservableCollection<Square>(OneDSquares);
            SquidsLeftImages = new ObservableCollection<ImageHolder>(SquidsLeftImages);
        }

        /// <summary>
        /// Run when a square on the grid is selected. If the square has a squid
        /// then attack logic is performed, elseif there is no squid present then miss logic is run. 
        /// A call is then made to check if the game is over.
        /// </summary>
        private void AttackSquare(int selectedIndex)
        {

            //If attackable and squid present
            if (OneDSquares[selectedIndex].SquidPresent == true)
            {

                OneDSquares[selectedIndex].ImagePath = GameConstants.SquareHitPath;

                if (OneDSquares[selectedIndex].AttackSquid())
                {
                    //Returned true which means a squid killed

                    SquidsLeftImages[SquidKillIndex].ImagePath = GameConstants.SquidDeadPath;

                    SquidKillIndex++;

                    SquidKilledMethod(); //Hit- Squid killed
                    Feedback = "Squid destroyed!";
                }
                else
                {
                    AttackMethod(true); //Hit
                    Feedback = "Hit!";
                }
                    

            }
            else //If attackable but no squid
            {

                OneDSquares[selectedIndex].ImagePath = GameConstants.SquareMissPath;

                AttackMethod(false); //Miss
                Feedback = "Miss!";

            }

            //Update selected square and change it's attack status so it cannot be attacked again
            OneDSquares[selectedIndex].AttackStatus = false;

            //Change a bomb to unavailable
            BombImages[ShotCounter].ImagePath = GameConstants.BombUnavailablePath;
            ShotCounter++;

            UpdateMainWindow();

            CheckForEnd();

        }

        /// <summary>
        /// Checks if game has reached the end state (Win/Lose)
        /// </summary>
        private void CheckForEnd()
        {
            if (SquidKillIndex == NumberOfSquid)
            {
                MessageBox.Show("You Win");
                CheckForHighScore();
                GameState.SetupGame(this);

            }
            else if (ShotCounter == GameConstants.MaxShotCount)
            {
                MessageBox.Show("You Lose");
                GameState.SetupGame(this);
            }
        }

        /// <summary>
        /// Checks whether the final score of the user beats that of the stored Highscore
        /// If so, it updates the Highscore property and writes the new result to the Highscore file.
        /// </summary>
        private void CheckForHighScore()
        {
           
            if (ShotCounter < Highscore)
            {
                //update highscore property and textfile
                Highscore = ShotCounter;
                FileRepository.WriteStringToFile(GameConstants.HighscoreFileName, Highscore.ToString());

            }

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

                //ScreenShakeAnimation();

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

            //ScreenShakeAnimation();
        }


        //Dont know how to get this to work as will require injection of MainWindow object which itslef needs the GameView model- endless circle
        
        ///// <summary>
        ///// This performs an anmination on the mian window- shaking it
        ///// </summary>
        //private void ScreenShakeAnimation()
        //{

        //    DoubleAnimation leftAnimation = new DoubleAnimation();

        //    leftAnimation.From = _window.Left;
        //    leftAnimation.To = _window.Left - 30;
        //    leftAnimation.Duration = TimeSpan.FromSeconds(0.15);
        //    leftAnimation.AutoReverse = true;
        //    _window.BeginAnimation(Window.LeftProperty, leftAnimation);
        //}


    }

}

