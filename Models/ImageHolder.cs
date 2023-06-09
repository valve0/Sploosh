using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sploosh.Models
{
    class ImageHolder
    {
        public Uri ImagePath { get; set; }

        public string Name { get; set; }

        public ImageHolder(string imagePath, string name)
        {
            ImagePath = new Uri(imagePath);

            Name = name;
        }
    }
}
