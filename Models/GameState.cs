using HelperClass;
using Sploosh.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Sploosh.Models
{
    public class GameState
    {

        private static List<Squid> listOfSquids;

        private static Square[,] twoDSquares;

        /// <summary>
        /// Resets the Properties of the MainWindowViewModel raeady for a new game
        /// </summary>
        public static void SetupGame(MainWindowViewModel viewModel)
        {
            //Reset counters
            viewModel.SquidKillIndex = 0;
            viewModel.ShotCounter = 0;
            viewModel.Feedback = "Test string";

            //Set highscore
            viewModel.Highscore = int.Parse(FileRepository.LoadStringFromFile(GameConstants.HighscoreFileName).Replace("\r\n", string.Empty));

            //Fill array of twoDSquares with new twoDSquares, setting them to default start value
            twoDSquares = new Square[GameConstants.Size, GameConstants.Size];

            for (int row = 0; row < GameConstants.Size; row++)
            {
                for (int col = 0; col < GameConstants.Size; col++)
                {
                    //Fill list with default value of sea state
                    int[] squarePosition = { row, col };

                    twoDSquares[row, col] = new Square(squarePosition, GameConstants.SquareStartImagePath);
                }
            }

            listOfSquids = new List<Squid>();

            //Load Bombs

            viewModel.BombImages = new ObservableCollection<ImageHolder>();

            for (int i = 0; i < GameConstants.MaxShotCount; i++)
            {
                string bombName = $"Bomb {i + 1}";
                viewModel.BombImages.Add(new ImageHolder(GameConstants.BombAvailablePath, bombName));
            }

            //Necessary for the view to update with the new property change
            //PropertyChanged(this, new PropertyChangedEventArgs(nameof(BombImages)));
            viewModel.NumberOfSquid = 0;

            foreach (var squidTuple in GameConstants.SquidTuples)
            {
                viewModel.NumberOfSquid += squidTuple.noSquid;
            }

            //Load Squids left

            viewModel.SquidsLeftImages = new ObservableCollection<ImageHolder>();

            for (int i = 0; i < viewModel.NumberOfSquid; i++)
            {

                string squidName = $"Squid {i + 1}";
                viewModel.SquidsLeftImages.Add(new ImageHolder(GameConstants.SquidAlivePath, squidName));
            }

            //Create squid collection
            viewModel.Squids = new ObservableCollection<Squid>();
            PlaceSquids();

            viewModel.OneDSquares = new ObservableCollection<Square>();

            //Convert 2d grid to 1d list for gridlist
            for (int i = 0; i < GameConstants.Size; i++)
            {
                for (int j = 0; j < GameConstants.Size; j++)
                {
                    viewModel.OneDSquares.Add(twoDSquares[i, j]);
                }
            }

        }

        /// <summary>
        /// This method works out where to put each squid. It loops through
        /// the length of each squid putting the selected location for each part
        /// of the squid into a temporary 2D list "twoDSquares".
        /// 
        /// </summary>
        private static void PlaceSquids()
        {
            //Create a temporary list of 
            List<Squid> squids = new List<Squid>();
            int squidID = 0;


            //Loop through array of tuples
            foreach (var squidTuple in GameConstants.SquidTuples)
            {
                squidID++;

                //Loop through the number of squid for a given Size
                for (int squidNo = 1; squidNo < squidTuple.noSquid + 1; squidNo++)
                {
                    //Initialise
                    bool squidPartPlaced = false;
                    bool squidPlaced = false;

                    do //Loop through the parts of the squid- only leave when it is placed successfully
                    {
                        //Create a temporary variable to hold the row,col positions for each parts of the squid
                        List<int[]> squidPartPositions = new List<int[]>();

                        //create empty list of arrays to match the Size of the squid
                        for (int i = 0; i < squidTuple.squidSize; i++)
                        {
                            int[] array = new int[2];
                            squidPartPositions.Add(array);
                        }
                        //genreate new direction
                        Random Random = new Random();
                        int direction = Random.Next(4);

                        // Loop through the parts for the given squid
                        for (int squidPart = 0; squidPart < squidTuple.squidSize; squidPart++)
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
                                squidPartPlaced = PlaceSubsequentSquidPart(squidTuple, squidID, squidPart, direction, ref squidPartPositions);

                                if (squidPartPlaced == false)
                                    break; //Redo squid place loop
                                else if (squidPartPlaced == true && squidPart + 1 == squidTuple.squidSize)
                                    squidPlaced = true;
                            }


                        }

                    } while (squidPlaced == false);

                }

            }

        }

        /// <summary>
        /// The logic for fidning the first loaction of a squid (independant of direction it is pointing
        /// </summary>
        /// <param name="squidPartPositions"></param>
        private static void PlaceFirstPartOfSquid(ref List<int[]> squidPartPositions)
        {

            Random Random = new Random();
            //Choose a random spot to place the first part of the squid
            bool firstPartPlaced = false;

            do // Keep looping until squid placed
            {
                //Randomly place first part
                squidPartPositions[0][0] = Random.Next(GameConstants.Size);
                squidPartPositions[0][1] = Random.Next(GameConstants.Size);

                //check to see if no squid already in this spot
                if (twoDSquares[squidPartPositions[0][0], squidPartPositions[0][1]].SquidPresent == false)
                    firstPartPlaced = true;

            } while (firstPartPlaced == false);


        }
        /// <summary>
        /// This method helps generate a new location for a subsequent part of a squid (not first).
        /// It is a subsquent location as it required the direction the squid is pointing.
        /// The method then and checks whether it is a valid location and then checks to see
        /// if all the parts of the squid have been successfully placed in the temporary list it
        /// adds the temporary squid locations into the permannent 2d array.
        /// </summary>
        private static bool PlaceSubsequentSquidPart((string name, int squidSize, int noSquid) squidTuple, int squidNo, int squidPart, int direction, ref List<int[]> squidPartPositions)
        {


            //Based on the first part of the squid generate the following location in the supplied direction
            int[] nextTile = GenerateNextTile(squidPartPositions[0][0], squidPartPositions[0][1], squidPart, direction);
            int nextTileRow = nextTile[0];
            int nextTileCol = nextTile[1];

            //Make sure new grid reference is valid (on board and no squid present)
            if (nextTileRow >= 0 && nextTileRow < GameConstants.Size && nextTileCol >= 0 && nextTileCol < GameConstants.Size && twoDSquares[nextTileRow, nextTileCol].SquidPresent == false)
            {

                // Add squid position to list of squid positions
                squidPartPositions[squidPart][0] = nextTileRow;
                squidPartPositions[squidPart][1] = nextTileCol;

                //check to see if at end of loop and therefore last part of squid placed successfully
                if (squidPart + 1 == squidTuple.squidSize)
                {
                    //int tempSize = ;

                    //Instantiate squid object add to list of squids
                    listOfSquids.Add(new Squid(squidTuple.squidSize, squidNo));


                    //Loop through all the current squid positions and create a squid object in their locations
                    foreach (var positon in squidPartPositions)
                    {
                        // Place a squid at this position
                        twoDSquares[positon[0], positon[1]].SetSquid(listOfSquids.Last());
                    }
                }

                return true; // Subsequent Part placed

            }

            //Cannot place squid here
            return false;

        }

        /// <summary>
        /// Helps generate the next location the squid part would be in- based on the direction the squid is pointing.
        /// </summary>
        private static int[] GenerateNextTile(int startTileRow, int startTileCol, int squidPart, int direction)
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

    }
 
}
