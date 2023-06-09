using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sploosh.Models
{
    public class Square
    {

        public Uri ImagePath { get; set; } = new Uri( @"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquareStart2.png");

        public int ID { get; set; }

        public bool SquidPresent { get; set; }

        public bool AttackStatus { get; set; } = true;

        public Squid Squid { get; private set; }

        public int[] SquarePosition { get; private set; }

        public Square(int[] squarePosition)
        {
            SquarePosition = squarePosition;
            SquidPresent = false;
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

        public bool AttackSquare() //returns true if squid killed, false if not
        {

            if (SquidPresent == false)
            {
                AttackStatus = false;
                ImagePath = new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquareMiss.png");
                
            }
            else
            {
                AttackStatus = false;
                ImagePath = new Uri(@"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\SquareHit.png");
                if (AttackSquid())
                {
                    //Squid killed, update the squid remaining array
                    return true;
                }
            }

            return false;

        }



        public bool AttackSquid()
        {
            return Squid.Attack();

        }

    }
}
