using HelperClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sploosh.ViewModels
{
    public class GameConstants
    {

        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static readonly string HighscoreFileName = "Highscore.txt";

        public static readonly int Size = 8;

        public static readonly int MaxShotCount = 24;

        public static readonly (string name, int squidSize, int noSquid)[] SquidTuples = new (string name, int squidSize, int noSquid)[]
        {
            ("small", 1, 0),
            ("medium", 2, 1),
            ("large", 3, 1),
            ("giant", 4, 1)
        };

        public static readonly Uri SquareStartImagePath = new Uri($@"{AssemblyDirectory}\Images\SquareStart.png");
        public static readonly Uri SquareHitPath = new Uri(@$"{AssemblyDirectory}\Images\SquareHit.png");
        public static readonly Uri SquareMissPath = new Uri($@"{AssemblyDirectory}\Images\SquareMiss.png");
        public static readonly Uri BackgroundImage = new Uri($@"{AssemblyDirectory}\Images\Pergament1.png");
        public static readonly Uri ArrowImage = new Uri($@"{AssemblyDirectory}\Images\BackArrow.png");
        public static readonly Uri BombAvailablePath = new Uri($@"{AssemblyDirectory}\Images\BombAvailable.png");
        public static readonly Uri BombUnavailablePath = new Uri($@"{AssemblyDirectory}\Images\BombUnvailable.png");
        public static readonly Uri SquidAlivePath = new Uri($@"{AssemblyDirectory}\Images\SquidAlive.png");
        public static readonly Uri SquidDeadPath = new Uri($@"{AssemblyDirectory}\Images\SquidDead.png");
    }
}
