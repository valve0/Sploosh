using System.Collections.Generic;

namespace Sploosh.Models
{
    public class Squid
    {

        public int Length { get; set; }
        public bool Alive { get; set; } = true;
        private int hitCounter;
        private List<int[]> squidPositions;


        public Squid(int length)
        {
            Length = length;
            hitCounter = 0;
            squidPositions = new List<int[]>();
        }

        /// <summary>
        /// When a squid is attacked, increase hit counter 
        /// and returns true status if killed, else false
        /// </summary>
        public bool Attack()
        {
            hitCounter++;

            if (hitCounter == Length)
            {
                return true; //squid now dead
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds the supplied coordinate position to a list
        /// of coodinates for the squid
        /// </summary>
        public void AddSquidPosition(int[] position)
        {
            squidPositions.Add(position);
        }
    }
}
