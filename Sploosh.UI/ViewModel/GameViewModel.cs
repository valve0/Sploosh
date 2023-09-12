using Sploosh.Resources;
using Sploosh.UI.Resources;
using SplooshGameEngine;
using SplooshKaboom.GameEngine;
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
        private ObservableCollection<Uri> _squidsLeftImages;
        private ObservableCollection<Uri> _bombImages;
        private ObservableCollection<Uri> _squareImages;


        private MediaPlayer backgroundPlayer = new();
        private MediaPlayer SoundEffectsPlayer = new();


        private string backgroundMusicPath;
        private string squidDeadSoundPath;
        private string squidHitSoundPath;
        private string squidMissSoundPath;

        public GameViewModel(UserSettings userSettings)
        {

            UserSettings = userSettings;

            RestartGame();
            Feedback = "";

            backgroundPlayer.Open(EnvironmentVariables.backgroundMusicPath);
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();

            SoundEffectsPlayer.Volume = 0;

            GameState = new();

        }


        public UserSettings UserSettings { get; }

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

        public ObservableCollection<Uri> SquareImages
        {
            get { return _squareImages; }
            set
            {
                _squareImages = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<Uri> BombImages
        {
            get { return _bombImages; }
            set
            {
                _bombImages = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Uri> SquidsLeftImages
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


                if (_selectedIndex != -1) // -1 = setup value
                    AttackSquare(_selectedIndex);

            }
        }

        public GameState GameState { get; private set; }


        /// <summary>
        /// When called this resets the application so the user can play the game again
        /// </summary>
        public void RestartGame()
        {
            GameState.SetupGame();

            BombImages = new ObservableCollection<Uri>(BombImages);
            SquidsLeftImages = new ObservableCollection<Uri>(SquidsLeftImages);
            SquareImages = new ObservableCollection<Uri>(SquareImages);


        }

        /// <summary>
        /// Runs everytime view model is loaded-checks fort changes in user settings
        /// </summary>
        public async override Task LoadAsync()
        {
            //Check for updates in UserSettings

            if (UserSettings.SoundEffectsStatus == false)
                SoundEffectsPlayer.Volume = 0;
            else
                SoundEffectsPlayer.Volume = 0.2;

            if (UserSettings.RestartGameTrigger == true)
            {
                UserSettings.UpdateRestartGameTrigger();
                RestartGame();
            }

        }


        ///// <summary>
        ///// Forces an update of the displayed properties in the main window
        ///// At the moment the properties dont recognise when thier own properties get updated so need to force it
        ///// </summary>
        //private void UpdateMainWindow()
        //{
        //    BombImages = new ObservableCollection<Uri>(BombImages);
        //    OneDSquares = new ObservableCollection<Uri>(OneDSquares);
        //    SquidsLeftImages = new ObservableCollection<Uri>(SquidsLeftImages);
        //}


        /// <summary>
        /// Run when a square on the grid is selected. If the square has a squid
        /// then attack logic is performed, elseif there is no squid present then miss logic is run. 
        /// A call is then made to check if the game is over.
        /// </summary>
        private void AttackSquare(int selectedIndex)
        {

            //Convert selected index into 2d array index.
            int[] twoDIndex = { selectedIndex / 8, selectedIndex % 8 };


            var attackResultCode = GameState.MakeShot(twoDIndex);

            switch (attackResultCode)
            {
                case AttackResultCode.None:
                    break;
                case AttackResultCode.Miss:
                    break;
                case AttackResultCode.Hit:
                    break;
                case AttackResultCode.SquidDead:
                    break;
                case AttackResultCode.GameWin:
                    break;
                case AttackResultCode.GameLose:
                    break;
            }


            ////If attackable and squid present
            //if (OneDSquares[selectedIndex].SquidPresent == true)
            //{

            //    OneDSquares[selectedIndex].ImagePath = GameState.SquareHitPath;

            //    if (OneDSquares[selectedIndex].AttackSquid())
            //    {
            //        //Returned true which means a squid killed

            //        SquidsLeftImages[_squidKillIndex].ImagePath = GameState.SquidDeadPath;

            //        _squidKillIndex++;

            //        PlaySquidKilledSound(); //Hit- Squid killed
            //        Feedback = "Squid destroyed!";
            //    }
            //    else
            //    {
            //        PlayAttackSounds(true); //Hit
            //        Feedback = "Hit!";
            //    }


            //}
            //else //If attackable but no squid
            //{

            //    OneDSquares[selectedIndex].ImagePath = GameState.SquareMissPath;

            //    PlayAttackSounds(false); //Miss
            //    Feedback = "Miss!";

            //}

            ////Update selected square and change it's attack status so it cannot be attacked again
            //OneDSquares[selectedIndex].AttackStatus = false;

            ////Change a bomb to unavailable
            //BombImages[ShotCounter].ImagePath = GameState.BombUnavailablePath;
            //ShotCounter++;

            //UpdateMainWindow();

            //CheckForEnd();

        }

        /// <summary>
        /// Checks if game has reached the end state (Win/Lose)
        /// </summary>
        private void CheckForEnd()
        {
            if (_squidKillIndex == _numberOfSquid)
            {
                MessageBox.Show("You Win");
                //CheckForHighScore();
                RestartGame();

            }
            else if (ShotCounter == GameState.MaxShotCount)
            {
                MessageBox.Show("You Lose");
                RestartGame();
            }
        }

        ///// <summary>
        ///// Checks whether the final score of the user beats that of the stored Highscore
        ///// If so, it updates the Highscore property and writes the new result to the Highscore file.
        ///// </summary>
        //private void CheckForHighScore()
        //{

        //    if (ShotCounter < Highscore)
        //    {
        //        //update highscore property and textfile
        //        Highscore = ShotCounter;
        //        FileRepository.WriteStringToFile(GameState.HighscoreFileName, Highscore.ToString());

        //    }

        //}

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

