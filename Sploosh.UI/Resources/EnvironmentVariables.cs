using System;
using System.IO;
using System.Reflection;

namespace Sploosh.UI.Resources
{
    public static class EnvironmentVariables
    {
        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
        public static readonly Uri HelpImage1 = new Uri($@"{AssemblyDirectory}\Images\Help1.png");
        public static readonly Uri HelpImage2 = new Uri($@"{AssemblyDirectory}\Images\Help2.png");
        public static readonly Uri HelpImage3 = new Uri($@"{AssemblyDirectory}\Images\Help3.png");

        //Sounds
        public static readonly Uri backgroundMusicPath = new Uri($@"{AssemblyDirectory}/Sounds/BackgroundMusic.mp3");
        public static readonly Uri squidDeadSoundPath = new Uri($@"{AssemblyDirectory}/Sounds/SquidDead.mp3");
        public static readonly Uri squidHitSoundPath = new Uri($@"{AssemblyDirectory}/Sounds/SquidHit.mp3");
        public static readonly Uri squidMissSoundPath = new Uri($@"{AssemblyDirectory}/Sounds/SquidMiss.mp3");
    }
}
