using System;

namespace Sploosh.Modules.Game.ViewModels
{
    public class ImageHolder
    {
        public ImageHolder(Uri filePath)
        {
            FilePath = filePath;
        }

        public Uri FilePath { get; set; }
    }
}