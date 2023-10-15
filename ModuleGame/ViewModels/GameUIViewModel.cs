using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sploosh.GameEngine;
using Sploosh.Services;
using Sploosh.UI.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Sploosh.Modules.Game.ViewModels
{
    public class GameUIViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        public GameState _gameState;

        private int _selectedIndex = -1;

        private int _shotCounter;
        private int _highScore;
        private ObservableCollection<ImageHolder> _squareImages;
        private ObservableCollection<ImageHolder> _bombImages;
        private ObservableCollection<ImageHolder> _squidsLeftImages;
        private string _feedback;

        private MediaPlayer backgroundPlayer = new();
        private MediaPlayer SoundEffectsPlayer = new();

        // LOAD part of Game Loop. Called just once at the beginning of the game; used to set up
        //game objects, variables, etc. and prepare the game world.
        public GameUIViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {

            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _gameState = new GameState();

            RestartGame();
            Feedback = "";

            backgroundPlayer.Open(EnvironmentVariables.backgroundMusicPath);
            backgroundPlayer.MediaEnded += new EventHandler(BackgroundMusicEnded);
            backgroundPlayer.Play();

            SoundEffectsPlayer.Volume = 0;

            _eventAggregator.GetEvent<SoundStateEvent>().Subscribe(OnMessageReceived);
            _eventAggregator.GetEvent<RestartGameEvent>().Subscribe(RestartGame);

            NavigateSettingsCommand = new DelegateCommand<string>(NavigateSettings);
        }

        public DelegateCommand<string> NavigateSettingsCommand { get; private set; }

        private void NavigateSettings(string navigationPath)
        {

            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            _regionManager.RequestNavigate("ContentRegion", navigationPath);

        }

        private void OnMessageReceived(string message)
        {
            switch (message)
            {
                case "SoundEffectsToggle":
                    if (SoundEffectsPlayer.Volume == 0)
                        SoundEffectsPlayer.Volume = 20;
                    else
                        SoundEffectsPlayer.Volume = 0;
                    break;
            }
        }

        public int ShotCounter
        {
            get { return _shotCounter; }
            set { SetProperty(ref _shotCounter, value); }
        }

        public int HighScore
        {
            get { return _highScore; }
            set { SetProperty(ref _highScore, value); }
        }

        public ObservableCollection<ImageHolder> SquareImages
        {
            get { return _squareImages; }
            set { SetProperty(ref _squareImages, value); }
        }

        public ObservableCollection<ImageHolder> BombImages
        {
            get { return _bombImages; }
            set { SetProperty(ref _bombImages, value); }
        }

        public ObservableCollection<ImageHolder> SquidsLeftImages
        {
            get { return _squidsLeftImages; }
            set { SetProperty(ref _squidsLeftImages, value); }
        }

        public string Feedback
        {
            get { return _feedback; }
            set { SetProperty(ref _feedback, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {

                if (_selectedIndex == value)
                    return;

                SetProperty(ref _selectedIndex, value);

                if (_selectedIndex != -1) // -1 = setup value
                    AttackSquare(_selectedIndex);
            }
        }

        /// <summary>
        /// When called this resets the application so the user can play the game again
        /// </summary>
        public void RestartGame()
        {
            _gameState.SetupGame();

            ShotCounter = 0;
            HighScore = _gameState.HighScore;

            SquidsLeftImages = new();
            BombImages = new();
            SquareImages = new();

            for (int i = 0; i < _gameState.MaxSquidCount; i++)
                SquidsLeftImages.Add(new ImageHolder(EnvironmentVariables.SquidAlivePath));

            for (int i = 0; i < _gameState.MaxShotCount; i++)
                BombImages.Add(new ImageHolder(EnvironmentVariables.BombAvailablePath));

            for (int i = 0; i < _gameState.BoardSize * _gameState.BoardSize; i++)
                SquareImages.Add(new ImageHolder(EnvironmentVariables.SquareStartImagePath));

            //BombImages = new ObservableCollection<Uri>(BombImages);
            //SquidsLeftImages = new ObservableCollection<Uri>(SquidsLeftImages);
            //SquareImages = new ObservableCollection<Uri>(SquareImages);

        }

        /// <summary>
        /// UPDATE GAME: part of game loop
        /// Run when a square on the grid is selected. If the square has a squid
        /// then attack logic is performed, elseif there is no squid present then miss logic is run. 
        /// A call is then made to check if the game is over.
        /// </summary>
        private void AttackSquare(int selectedIndex)
        {

            //Convert selected index into 2d array index.
            int[] twoDIndex = { selectedIndex / 8, selectedIndex % 8 };


            var attackResultCode = _gameState.MakeShot(twoDIndex);

            //GameState
            switch (attackResultCode)
            {
                case AttackResultCode.None:
                    break;

                case AttackResultCode.Miss:
                    SoundEffectsPlayer.Open(EnvironmentVariables.squidMissSoundPath);
                    SoundEffectsPlayer.Play();

                    SquareImages[selectedIndex].FilePath = EnvironmentVariables.SquareMissPath;

                    BombImages[_gameState.ShotCount - 1].FilePath = EnvironmentVariables.BombUnavailablePath;

                    break;

                case AttackResultCode.Hit:
                    SoundEffectsPlayer.Open(EnvironmentVariables.squidHitSoundPath);
                    SoundEffectsPlayer.Play();

                    SquareImages[selectedIndex].FilePath = EnvironmentVariables.SquareHitPath;

                    BombImages[_gameState.ShotCount - 1].FilePath = EnvironmentVariables.BombUnavailablePath;

                    _eventAggregator.GetEvent<ScreenShakeEvent>().Publish();

                    break;

                case AttackResultCode.SquidDead:
                    SoundEffectsPlayer.Open(EnvironmentVariables.squidDeadSoundPath);
                    SoundEffectsPlayer.Play();

                    SquareImages[selectedIndex].FilePath = EnvironmentVariables.SquareHitPath;

                    SquidsLeftImages[_gameState.SquidKillCount - 1].FilePath = EnvironmentVariables.SquidDeadPath;

                    BombImages[_gameState.ShotCount - 1].FilePath = EnvironmentVariables.BombUnavailablePath;

                    _eventAggregator.GetEvent<ScreenShakeEvent>().Publish();
                    break;

                case AttackResultCode.GameWin:
                    MessageBox.Show("You Win");
                    //CheckForHighScore();
                    RestartGame();
                    break;

                case AttackResultCode.GameLose:
                    MessageBox.Show("You Lose");
                    RestartGame();
                    break;
            }

            ShotCounter = _gameState.ShotCount;

            //RENDER PART OF GAMELOOP
            //Force Update Dsipalyed Properties
            BombImages = new ObservableCollection<ImageHolder>(BombImages);
            SquareImages = new ObservableCollection<ImageHolder>(SquareImages);
            SquidsLeftImages = new ObservableCollection<ImageHolder>(SquidsLeftImages);

        }

        /// <summary>
        /// This plasy the background music on loop
        /// </summary>
        private void BackgroundMusicEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }
    }
}

