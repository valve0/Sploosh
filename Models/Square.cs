using HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sploosh.Models
{
    public class Square
    {

        public Square(int[] squarePosition, Uri imagePath)
        {
            SquarePosition = squarePosition;
            SquidPresent = false;
            ImagePath = imagePath;
            AttackStatus = true;
        }

        public Uri ImagePath { get; set; }

        public int ID { get; private set; }

        public bool SquidPresent { get; private set; }

        public bool AttackStatus { get; set; }

        public Squid Squid { get; private set; }

        public int[] SquarePosition { get; private set; }


        public void SetSquid(Squid squid)
        {
            Squid = squid;

            SquidPresent = true;

            this.Squid.AddSquidPosition(this.SquarePosition);

        }

        public bool AttackSquid()
        {
            return Squid.Attack();
        }

    }
}
