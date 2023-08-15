using HelperClass;
using Sploosh.Model;
using Sploosh.Models;
using Sploosh.Resources;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sploosh.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private int _selectedIndex = -1;
        private int _numberOfSquid;
        private int _squidKillIndex;

        private int _shotCounter;
        private int _highScore;
        private string _feedback;
        private ObservableCollection<ImageHolder> _squidsLeftImages;
        private ObservableCollection<ImageHolder> _bombImages;
        private ObservableCollection<Square> _oneDSquares;


        private MediaPlayer backgroundPlayer = new();
        private MediaPlayer SoundEffectsPlayer = new();


        private string backgroundMusicPath;
        private string squidDeadSoundPath;
        private string squidHitSoundPath;
        private string squidMissSoundPath;


        public GameViewModel(UserSettings userSettings)
        {
            
            UserSettings = userSettings;

            SetupGame();

            //Sounds
            backgroundMusicPath = GameModel.AssemblyDirectory + "/Sounds/BackgroundMusic.mp3";
            squidDeadSoundPath = GameModel.AssemblyDirectory + "/Sounds/SquidDead.mp3";
            squidHitSoundPath = GameModel.AssemblyDirectory + "/Sounds/SquidHit.mp3";
            squidMissSoundPath = GameModel.AssemblyDirectory + "/Sounds/SquidMiss.mp3";


            backgroundPlayer.Open(new Uri(backgroundMusicPath, UriKind.Relative));
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();

            SoundEffectsPlayer.Volume = 0;

        }


        public UserSettings UserSettings { get;}

        public delegate void AttackEventAction();
        public event AttackEventAction? ScreenShakeAnimationEvent;


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
        public void RestartGame()
        {
            SetupGame();
        }

        /// <summary>
        /// Runs everytime view model is loaded-checks fort changes in user settings
        /// </summary>
        public async override Task LoadAsync()
        {
            //Check for updates in UserSettings

            if(UserSettings.SoundEffectsStatus == false)
                SoundEffectsPlayer.Volume = 0;
            else
                SoundEffectsPlayer.Volume = 0.2;

            if(UserSettings.RestartGameTrigger == true)
            {
                UserSettings.UpdateRestartGameTrigger();
                RestartGame();
            }

        }


        /// <summary>
        /// Forces an update of the displayed properties in the main window
        /// At the moment the properties dont recognise when thier own properties get updated so need to force it
        /// </summary>
        private void UpdateMainWindow()
        {
            BombImages = new ObservableCollection<ImageHolder>(BombImages);
            OneDSquares = new ObservableCollection<Square>(OneDSquares);
            SquidsLeftImages = new ObservableCollection<ImageHolder>(SquidsLeftImages);
        }


        /// <summary>
        /// Initialises the fields and properties at the start of the game
        /// </summary>
        private void SetupGame()
        {

            _squidKillIndex = 0;
            _numberOfSquid = 3;

            ShotCounter = 0;
            Feedback = "";

            Highscore = GameModel.GetHighscore();
            BombImages = GameModel.GetBombImages();
            OneDSquares = GameModel.GetOneDSquares();
            SquidsLeftImages = GameModel.GetSquidImages();

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

                OneDSquares[selectedIndex].ImagePath = GameModel.SquareHitPath;

                if (OneDSquares[selectedIndex].AttackSquid())
                {
                    //Returned true which means a squid killed

                    SquidsLeftImages[_squidKillIndex].ImagePath = GameModel.SquidDeadPath;

                    _squidKillIndex++;

                    PlaySquidKilledSound(); //Hit- Squid killed
                    Feedback = "Squid destroyed!";
                }
                else
                {
                    PlayAttackSounds(true); //Hit
                    Feedback = "Hit!";
                }
                    

            }
            else //If attackable but no squid
            {

                OneDSquares[selectedIndex].ImagePath = GameModel.SquareMissPath;

                PlayAttackSounds(false); //Miss
                Feedback = "Miss!";

            }

            //Update selected square and change it's attack status so it cannot be attacked again
            OneDSquares[selectedIndex].AttackStatus = false;

            //Change a bomb to unavailable
            BombImages[ShotCounter].ImagePath = GameModel.BombUnavailablePath;
            ShotCounter++;

            UpdateMainWindow();

            CheckForEnd();

        }

        /// <summary>
        /// Checks if game has reached the end state (Win/Lose)
        /// </summary>
        private void CheckForEnd()
        {
            if (_squidKillIndex == _numberOfSquid)
            {
                MessageBox.Show("You Win");
                CheckForHighScore();
                SetupGame();

            }
            else if (ShotCounter == GameModel.MaxShotCount)
            {
                MessageBox.Show("You Lose");
                SetupGame();
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
                FileRepository.WriteStringToFile(GameModel.HighscoreFileName, Highscore.ToString());

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
        private void PlayAttackSounds(bool hit)
        {

            if (hit == true)
            {
                SoundEffectsPlayer.Open(new Uri(squidHitSoundPath, UriKind.Relative));
                SoundEffectsPlayer.Play();
                ScreenShakeAnimationEvent?.Invoke();

            }
            else
            {
                SoundEffectsPlayer.Open(new Uri(squidMissSoundPath, UriKind.Relative));
                SoundEffectsPlayer.Play();
            }

        }

        /// <summary>
        /// When the Squid Killed event occurs this plays the appropriate sound effect
        /// </summary>
        private void PlaySquidKilledSound()
        {
            SoundEffectsPlayer.Open(new Uri(squidDeadSoundPath, UriKind.Relative));
            SoundEffectsPlayer.Play();

        }

    }

}

