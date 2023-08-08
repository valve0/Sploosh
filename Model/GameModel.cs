using HelperClass;
using Sploosh.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Sploosh.Model;

public class GameModel
{

    public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public static readonly string HighscoreFileName = "Highscore.txt";
    public static readonly int Size = 8;
    public static readonly int MaxShotCount = 24;
    public static readonly int NumberOfSquid = 3;

    public static readonly Uri SquareStartImagePath = new Uri($@"{AssemblyDirectory}\Images\SquareStart.png");
    public static readonly Uri SquareHitPath = new Uri(@$"{AssemblyDirectory}\Images\SquareHit.png");
    public static readonly Uri SquareMissPath = new Uri($@"{AssemblyDirectory}\Images\SquareMiss.png");
    public static readonly Uri BackgroundImage = new Uri($@"{AssemblyDirectory}\Images\Pergament1.png");
    public static readonly Uri ArrowImage = new Uri($@"{AssemblyDirectory}\Images\BackArrow.png");
    public static readonly Uri BombAvailablePath = new Uri($@"{AssemblyDirectory}\Images\BombAvailable.png");
    public static readonly Uri BombUnavailablePath = new Uri($@"{AssemblyDirectory}\Images\BombUnavailable.png");
    public static readonly Uri SquidAlivePath = new Uri($@"{AssemblyDirectory}\Images\SquidAlive.png");
    public static readonly Uri SquidDeadPath = new Uri($@"{AssemblyDirectory}\Images\SquidDead.png");
    public static readonly Uri MotifImage = new Uri($@"{AssemblyDirectory}\Images\Motif.png");

    /// <summary>
    /// Returns the highscore read from the Highscore textfile
    /// </summary>
    /// <returns></returns>
    public static int GetHighscore()
    {
        return int.Parse(FileRepository.LoadStringFromFile(HighscoreFileName).Replace("\r\n", string.Empty));
    }


    /// <summary>
    /// Returns a collection of image holder objects that hold the initial squid images
    /// </summary>
    /// <returns></returns>

    public static ObservableCollection<ImageHolder> GetSquidImages()
    {
        ObservableCollection<ImageHolder> SquidsLeftImages = new();

        for (int i = 0; i < NumberOfSquid; i++)
        {
            string squidName = $"Squid {i + 1}";
            SquidsLeftImages.Add(new ImageHolder(SquidAlivePath, squidName));
        }

        return SquidsLeftImages;
    }

    /// <summary>
    /// Returns a collection of image holder objects that hold the initial bomb images
    /// </summary>
    /// <returns></returns>
    public static ObservableCollection<ImageHolder> GetBombImages()
    {
        ObservableCollection<ImageHolder> BombImages = new();

        for (int i = 0; i < MaxShotCount; i++)
        {
            string bombName = $"Bomb {i + 1}";
            BombImages.Add(new ImageHolder(BombAvailablePath, bombName));
        }

        return BombImages;
    }


    /// <summary>
    /// Returns a 1 dimensional array equal to the 2d board
    /// </summary>
    /// <returns></returns>
    public static ObservableCollection<Square> GetOneDSquares()
    {

        //Fill array of twoDSquares with new twoDSquares, setting them to default start value
        Square[,] twoDSquares = new Square[Size, Size];

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                //Fill list with default value of sea state
                int[] squarePosition = { row, col };

                twoDSquares[row, col] = new Square(squarePosition, SquareStartImagePath);
            }
        }

        PlaceSquid(2, twoDSquares);
        PlaceSquid(3, twoDSquares);
        PlaceSquid(4, twoDSquares);

        ObservableCollection<Square> OneDSquares = new();

        //Convert 2d grid to 1d list for gridlist
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                OneDSquares.Add(twoDSquares[i, j]);
            }
        }

        return OneDSquares;

    }


    /// <summary>
    /// This method works out where to put each squid. It loops through
    /// the length of each squid putting the selected location for each part
    /// </summary>
    private static void PlaceSquid(int length, Square[,] twoDSquares)
    {
        int orientation;
        int row;
        int col;

        while (true)
        {
            //generate new orientation (0 = up down, 1 = left right)
            Random Random = new Random();
            orientation = Random.Next(2);

            row = Random.Next(Size); //Starting row
            col = Random.Next(Size); //Starting col


            if (Fits(twoDSquares, length, orientation, row, col))
                break;

        }

        //Create the squid
        Squid squid = new Squid(length);

        //Place the squid
        if (orientation == 0)
        {
            for (int i = 0; i < length; i++)
            {
                twoDSquares[row, col + i].SetSquid(squid);
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {

                twoDSquares[row + i, col].SetSquid(squid);
            }
        }

    }


    /// <summary>
    /// Checks to see if the squid will fit on the board
    /// </summary>
    /// <returns></returns>
    private static bool Fits(Square[,] twoDSquares, int length, int orientation, int row, int col)
    {


        for (int i = 0; i < length; ++i) //Loop through the length of the squid
        {
            if (orientation == 0)
            {
                if (col + i >= Size) //Is is out of bounds? 
                    return false;


                if (twoDSquares[row, col + i].SquidPresent == true) //Are there any squid present at this location?             
                    return false;

            }
            else
            {

                if (row + i >= Size)
                    return false;

                if (twoDSquares[row + i, col].SquidPresent == true)
                    return false;

            }
        }
        return true; // All squid checked 


    }

}