using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class player
    {
        public User user;
        public card[] hand;
        public int cash;

        public player()
        {
            hand = new card[2];
        }
    }
}
