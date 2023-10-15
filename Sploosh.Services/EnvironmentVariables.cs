using System.Reflection;

namespace Sploosh.Services
{
    public static class EnvironmentVariables
    {
        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string _resourceDirectory = AssemblyDirectory + @"\Resources";

        public static readonly Uri SquareStartImagePath = new Uri($@"{_resourceDirectory}\Images\SquareStart.png");
        public static readonly Uri SquareHitPath = new Uri(@$"{_resourceDirectory}\Images\SquareHit.png");
        public static readonly Uri SquareMissPath = new Uri($@"{_resourceDirectory}\Images\SquareMiss.png");
        public static readonly Uri BackgroundImage = new Uri($@"{_resourceDirectory}\Images\Pergament1.png");
        public static readonly Uri ArrowImage = new Uri($@"{_resourceDirectory}\Images\BackArrow.png");
        public static readonly Uri BombAvailablePath = new Uri($@"{_resourceDirectory}\Images\BombAvailable.png");
        public static readonly Uri BombUnavailablePath = new Uri($@"{_resourceDirectory}\Images\BombUnavailable.png");
        public static readonly Uri SquidAlivePath = new Uri($@"{_resourceDirectory}\Images\SquidAlive.png");
        public static readonly Uri SquidDeadPath = new Uri($@"{_resourceDirectory}\Images\SquidDead.png");
        public static readonly Uri MotifImage = new Uri($@"{_resourceDirectory}\Images\Motif.png");
        public static readonly Uri HelpImage1 = new Uri($@"{_resourceDirectory}\Images\Help1.png");
        public static readonly Uri HelpImage2 = new Uri($@"{_resourceDirectory}\Images\Help2.png");
        public static readonly Uri HelpImage3 = new Uri($@"{_resourceDirectory}\Images\Help3.png");

        //Sounds
        public static readonly Uri backgroundMusicPath = new Uri($@"{_resourceDirectory}\Sounds\BackgroundMusic.mp3");
        public static readonly Uri squidDeadSoundPath = new Uri($@"{_resourceDirectory}\Sounds\SquidDead.mp3");
        public static readonly Uri squidHitSoundPath = new Uri($@"{_resourceDirectory}\Sounds\SquidHit.mp3");
        public static readonly Uri squidMissSoundPath = new Uri($@"{_resourceDirectory}\Sounds\SquidMiss.mp3");
    }
}
