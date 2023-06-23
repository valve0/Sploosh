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




        public Uri ImagePath { get; set; }

        public int ID { get; set; }

        public bool SquidPresent { get; set; }

        public bool AttackStatus { get; set; } = true;

        public Squid Squid { get; private set; }

        public int[] SquarePosition { get; private set; }

        public Square(int[] squarePosition, Uri imagePath)
        {
            SquarePosition = squarePosition;
            SquidPresent = false;
            ImagePath = imagePath;
        }

        void UpdateAttackStatus()
        {
            AttackStatus = false;

        }

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
