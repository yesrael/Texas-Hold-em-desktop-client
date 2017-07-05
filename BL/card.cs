using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public enum CardType
    {
        SPADES, HEARTS, DIAMONDS, CLUBS
    }


    public class card 
    {

        public int number;
        public CardType type;

        public card(int number, CardType type)
        {
            this.number = number;
            this.type = type;
        }


    }

    public class Deck
    {

        public card[] cards;
        public int currentCardPlace;

        public Deck()
        {
        }


    }
}
