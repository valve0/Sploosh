using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Sploosh.Models;
using System.Data.SqlTypes;
using System.Windows;
using LeSploosh;

namespace Sploosh.ViewModels
{
    //PropertyChanged(this, new PropertyChangedEventArgs("ImagePath"));
    //= new RelayCommand(SelectImageMethod, CanSelectImage);

    class MainViewModel : INotifyPropertyChanged, ISetupGame
    {
        
        public delegate void AttackEventAction(bool hit);
        public event AttackEventAction? AttackEvent;


        
        public delegate void SquidKilledEventAction();
        public event SquidKilledEventAction? SquidKilledEvent;


        public ICommand ShowSettingsWindowCommand { get; set; }

        private int shotCounter;

        public int ShotCounter
        {
            get { return shotCounter; }
            set
            {

                if (shotCounter != value)
                {
                    shotCounter = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();
                }
            }
        }

        private int _highscore;

        public int Highscore
        {
            get { return _highscore; }
            set
            {
                if (_highscore != value)
                {
                    _highscore = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();
                }
            }
        }


        //Acts as a form of kill counter
        private int squidKillIndex = 0;
        //public ICommand GridClick { get; set; }
        private ObservableCollection<Square> _convertedSquares;
        public ObservableCollection<Square> ConvertedSquares {
            get { return _convertedSquares; }
            set
            {
                if (_convertedSquares != value)
                {
                    _convertedSquares = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();

                }

            }
        }

        public Square[,] squares;

        private static int _size = 8;

        private static int maxShotCount = 24;

        private static (string name, int squidsize, int noSquid)[] squidTuples = new (string name, int squidsize, int noSquid)[]
        {
            ("small", 1, 0),
            ("medium", 2, 1),
            ("large", 3, 1),
            ("giant", 4, 1)
        };

        private int numberOfSquid;


        

        private List<Squid> listOfSquids;

        public ObservableCollection<Squid> Squids;


        private int _selectedIndex =-1;

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


                if(_selectedIndex != -1) // -1 = setup value
                    AttackSquare(_selectedIndex);


            }
        }



        private int _selectedSquareId;


        private ObservableCollection<Square> _squares;

        public ObservableCollection<Square> Squares
        {
            get { return _squares; }
            set
            {
                if (_squares != value)
                {
                    _squares = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();

                }
            }
        }


        private ObservableCollection<ImageHolder> bombImages;

        public ObservableCollection<ImageHolder> BombImages
        {
            get { return bombImages; }
            set
            {

                if (bombImages != value)
                {
                    bombImages = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();


                }
            }
        }

        private ObservableCollection<ImageHolder> squidsLeftImages;

        public ObservableCollection<ImageHolder> SquidsLeftImages
        {
            get { return squidsLeftImages; }
            set
            {
                if (squidsLeftImages != value)
                {
                    squidsLeftImages = value;

                    //Necessary for the view to update with the new property change
                    OnPropertyChanged();


                }
            }
        }




        public MainViewModel()
        {

            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow, CanShowNewContactWindow);

            SetupGame();

        }

        public void SetupGame()
        {
            //Reset kill counter index
            squidKillIndex = 0;

            Highscore = int.Parse(TextFileRepository.LoadStringFromFile("Highscore.txt").Replace("\r\n", string.Empty));
            //string Highscoress = TextFileRepository.LoadStringFromFile("Highscore.txt");

            ShotCounter = 0;

            //SelectedIndex = 0;
            //Fill array of squares with new squares, setting them to default start value
            squares = new Square[_size, _size];

            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    //Fill list with default value of sea state
                    int[] squarePosition = { row, col };

                    squares[row, col] = new Square(squarePosition);
                }
            }

            listOfSquids = new List<Squid>();

            //Load Bombs

            BombImages = new ObservableCollection<ImageHolder>();

            for (int i = 0; i < maxShotCount; i++)
            {
                string bombImagePath = @"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\BombUnavailable.png";
                string bombName = $"Bomb {i + 1}";
                BombImages.Add(new ImageHolder(bombImagePath, bombName));
            }

            //Necessary for the view to update with the new property change
            //PropertyChanged(this, new PropertyChangedEventArgs(nameof(BombImages)));
            numberOfSquid = 0;

            foreach (var squidTuple in squidTuples)
            {
                numberOfSquid += squidTuple.noSquid;
            }

            //Load Squids left

            SquidsLeftImages = new ObservableCollection<ImageHolder>();

            for (int i = 0; i < numberOfSquid; i++)
            {
                string squidImagePath = @"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquidAlive2.png";
                string squidName = $"Squid {i + 1}";
                SquidsLeftImages.Add(new ImageHolder(squidImagePath, squidName));
            }

            //Create squid collection
            Squids = new ObservableCollection<Squid>();
            PlaceSquids();

            ConvertedSquares = new ObservableCollection<Square>();

            //Convert 2d grid to 1d list for gridlist
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    ConvertedSquares.Add(squares[i, j]);
                }
            }

        }

        private bool CanShowNewContactWindow(object obj)
        {
            return true;
        }

        private void ShowSettingsWindow(object obj)
        {
            UpdateMainWindow();
        }

        private void UpdateMainWindow()
        {
            BombImages = new ObservableCollection<ImageHolder>(BombImages);
            ConvertedSquares = new ObservableCollection<Square>(ConvertedSquares);
            SquidsLeftImages = new ObservableCollection<ImageHolder>(SquidsLeftImages);
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

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

        private void PlaceFirstPartOfSquid(ref List<int[]> squidPartPositions)
        {

            Random Random = new Random();
            //Choose a random spot to place the first part of the squid
            bool firstPartPlaced = false;

            do // Keep looping until squid placed
            {
                //Randomly place first part
                squidPartPositions[0][0] = Random.Next(_size);
                squidPartPositions[0][1] = Random.Next(_size);

                //check to see if no squid already in this spot
                if (squares[squidPartPositions[0][0], squidPartPositions[0][1]].SquidPresent == false)
                    firstPartPlaced = true;

            } while (firstPartPlaced == false);


        }

        private bool PlaceSubsequentSquidPart((string name, int squidsize, int noSquid) squidTuple, int squidNo, int squidPart, int direction, ref List<int[]> squidPartPositions)
        {
            //Generate a random number between 0 and 3 inclusive
            //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)

            //Based on the first part of the squid generate the following tiles int he supplied direction
            int[] nextTile = GenerateNextTile(squidPartPositions[0][0], squidPartPositions[0][1], squidPart, direction);
            int nextTileRow = nextTile[0];
            int nextTileCol = nextTile[1];

            //Make sure new grid reference is valid (on board and no squid present)
            if (nextTileRow >= 0 && nextTileRow < _size && nextTileCol >= 0 && nextTileCol < _size && squares[nextTileRow, nextTileCol].SquidPresent == false)
            {

                // Add squid position to list of squid positions
                squidPartPositions[squidPart][0] = nextTileRow;
                squidPartPositions[squidPart][1] = nextTileCol;

                //check to see if at end of loop and therefore last part of squid placed successfully
                if (squidPart + 1 == squidTuple.squidsize)
                {

                    //Instantiate squid object add to list of squids
                    listOfSquids.Add(new Squid(squidTuple.squidsize, squidNo));


                    //Loop through all the current squid positions and create a squid object in their locations
                    foreach (var positon in squidPartPositions)
                    {
                        // Place a squid at this position
                        squares[positon[0], positon[1]].SetSquid(listOfSquids.Last());


                    }

                }

                return true; // Subsequent Part placed

            }

            //Cannot place squid here
            return false;

        }

        private int[] GenerateNextTile(int startTileRow, int startTileCol, int squidPart, int direction)
        {

            int rowAdder = 0;
            int colAdder = 0;

            int[] nexttiles = new int[2];
            //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)
            switch (direction)
            {
                case 0:
                    {
                        rowAdder = -1;
                        colAdder = 0;
                        break;
                    }
                case 1:
                    {
                        rowAdder = 0;
                        colAdder = 1;
                        break;
                    }
                case 2:
                    {
                        rowAdder = 1;
                        colAdder = 0;
                        break;
                    }
                case 3:
                    {
                        rowAdder = 0;
                        colAdder = -1;
                        break;
                    }
            }

            nexttiles[0] = startTileRow + (rowAdder * squidPart);
            nexttiles[1] = startTileCol + (colAdder * squidPart);

            return nexttiles;

        }

        private void PlaceSquids()
        {
            //Create a temporary list of 
            List<Squid> squids = new List<Squid>();


            //Loop through array of tuples
            foreach (var squidTuple in squidTuples)
            {
                //Loop through the number of squid for a given size
                for (int squidNo = 1; squidNo < squidTuple.noSquid + 1; squidNo++)
                {
                    //Initialise
                    bool squidPartPlaced = false;
                    bool squidPlaced = false;

                    do //Loop through the parts of the squid- only leave when it is placed successfully
                    {
                        //Create a temporary variable to hold the row,col positions for each parts of the squid
                        List<int[]> squidPartPositions = new List<int[]>();

                        //create empty list of arrays to match the size of the squid
                        for (int i = 0; i < squidTuple.squidsize; i++)
                        {
                            int[] array = new int[2];
                            squidPartPositions.Add(array);
                        }
                        //genreate new direction
                        Random Random = new Random();
                        int direction = Random.Next(4);

                        // Loop through the parts for the given squid
                        for (int squidPart = 0; squidPart < squidTuple.squidsize; squidPart++)
                        {

                            //Place first part of squid
                            if (squidPart == 0)
                            {
                                //Get and place first part of squid
                                PlaceFirstPartOfSquid(ref squidPartPositions);
                            }
                            else
                            {
                                // Try and place the subsequent part of squid
                                squidPartPlaced = PlaceSubsequentSquidPart(squidTuple, squidNo, squidPart, direction, ref squidPartPositions);

                                if (squidPartPlaced == false)
                                    break; //Redo squid place loop
                                else if (squidPartPlaced == true && squidPart + 1 == squidTuple.squidsize)
                                    squidPlaced = true;
                            }


                        }

                    } while (squidPlaced == false);

                }

            }

        }


        private void AttackSquare(int selectedIndex)
        {


            if (ConvertedSquares[selectedIndex].AttackStatus) //If attackable
            {

                if (ConvertedSquares[selectedIndex].SquidPresent == true)
                {
                    
                    ConvertedSquares[selectedIndex].ImagePath = new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquareHit2.png");

                    if (ConvertedSquares[selectedIndex].AttackSquid())
                    {
                        //Returned true which means a squid killed

                        SquidsLeftImages[squidKillIndex].ImagePath =
                            new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquidDead2.png");

                        squidKillIndex++;

                        SquidKilledEvent?.Invoke();

                    }
                    else
                        AttackEvent?.Invoke(true); //Hit

                }
                
                else 
                {

                    ConvertedSquares[selectedIndex].ImagePath = new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquareMiss2.png");

                    AttackEvent?.Invoke(false); //Miss
                }


                //Change a bomb to unavailable
                BombImages[ShotCounter].ImagePath = new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\BombUnvailable2.png");
                ShotCounter++;

                UpdateMainWindow();

                if (squidKillIndex == numberOfSquid)
                {
                    MessageBox.Show("You Win");
                    CheckForHighScore();
                    SetupGame();

                }
                else if (shotCounter == maxShotCount)
                {
                    MessageBox.Show("You Lose");
                    SetupGame();
                }

            }

        }


        public void CheckForHighScore()
        {
            string HighScoreFileName = "Highscore.txt";


            if (ShotCounter < Highscore)
            {
                //update highscore property and textfile
                Highscore = ShotCounter;
                TextFileRepository.WriteStringToFile(HighScoreFileName, Highscore.ToString());

            }
            
        }




    }



    public interface ISetupGame
    {
        void SetupGame(); 
    }
}

