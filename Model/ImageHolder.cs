using System;

namespace Sploosh.Models
{
    public class ImageHolder
    {
        public Uri ImagePath { get; set; }

        public string Name { get; set; }

        public ImageHolder(Uri imagePath, string name)
        {
            ImagePath = imagePath;

            Name = name;
        }
    }
}
