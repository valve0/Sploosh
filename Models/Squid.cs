using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sploosh.Models
{
    public class Squid
    {

        public int Size { get; set; }
        public int Id { get; set; }
        public bool Alive { get; set; } = true;
        private int hitCounter;
        private List<int[]> squidPositions;


        public Squid(int Size, int id)
        {
            Size = Size;
            Id = id;
            hitCounter = 0;
            squidPositions = new List<int[]>();

        }

        public bool Attack()
        {
            hitCounter++;

            if (hitCounter == Size)
            {
                return true; //squid now dead
            }
            else
            {
                return false;
            }
        }

        public void AddSquidPosition(int[] squarePositions)
        {
            squidPositions.Add(squarePositions);
        }
    }
}
