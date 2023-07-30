using HelperClass;
using Sploosh.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sploosh.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private int _selectedIndex = -1;
        private int _shotCounter;
        private int _numberOfSquid;
        private int _highScore;
        private ObservableCollection<ImageHolder> _squidsLeftImages;
        private ObservableCollection<ImageHolder> _bombImages;
        private ObservableCollection<Square> _oneDSquares;
        private ObservableCollection<Squid> _squids;
        private string _feedback;

        public MainWindowViewModel()
        {

            GameState.SetupGame(this);

            //Remove test string
            Feedback = "";

            BackgroundImagePath = new BitmapImage(GameConstants.BackgroundImage);

            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow, CanShowNewContactWindow);

        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public delegate void AttackEventAction(bool hit);
        public event AttackEventAction? AttackEvent;

        public delegate void SquidKilledEventAction();
        public event SquidKilledEventAction? SquidKilledEvent;

        public ICommand ShowSettingsWindowCommand { get; set; }

        public ImageSource BackgroundImagePath { get; private set; }

        public int NumberOfSquid
        {
            get { return _numberOfSquid; }
            set
            {
                _numberOfSquid = value;
                OnPropertyChanged();
            }
        }

        public int ShotCounter
        {
            get { return _shotCounter; }
            set
            {
                _shotCounter = value;
                OnPropertyChanged();
            }
        }

        public int Highscore
        {
            get { return _highScore; }
            set
            {
                _highScore = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Square> OneDSquares
        {
            get { return _oneDSquares; }
            set
            {
                _oneDSquares = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Squid> Squids
        {
            get { return _squids; }
            set
            {
                _squids = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageHolder> BombImages
        {
            get { return _bombImages; }
            set
            {
                _bombImages = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageHolder> SquidsLeftImages
        {
            get { return _squidsLeftImages; }
            set
            {
                _squidsLeftImages = value;
                OnPropertyChanged();
            }
        }

        public string Feedback
        {
            get { return _feedback; }
            set
            {
                _feedback = value;
                OnPropertyChanged();
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
                OnPropertyChanged();

                Debug.WriteLine($"Selected Index: {_selectedIndex}");


                if (_selectedIndex != -1 && OneDSquares[_selectedIndex].AttackStatus == true) // -1 = setup value
                    AttackSquare(_selectedIndex);

            }
        }


        /// <summary>
        /// When called this resets the application so the user can play the game again
        /// </summary>
        public void RestartGame(MainWindowViewModel vm)
        {
            GameState.SetupGame(vm);
        }

        private bool CanShowNewContactWindow(object obj)
        {
            return true;
        }

        private void ShowSettingsWindow(object obj)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this);     
            settingsWindow.ShowDialog();
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

                    SquidKilledEvent?.Invoke(); //Hit- Squid killed
                    Feedback = "Squid destroyed!";
                }
                else
                {
                    AttackEvent?.Invoke(true); //Hit
                    Feedback = "Hit!";
                }
                    

            }
            else //If attackable but no squid
            {

                OneDSquares[selectedIndex].ImagePath = GameConstants.SquareMissPath;

                AttackEvent?.Invoke(false); //Miss
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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }

}

