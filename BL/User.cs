using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class User
    {
        public string ID;
        public string UserName;
        public int totalCash;
        public int score;
        public int league;
        public card[] hand;
        public string avatar;
        public User(string id, string name, int totalCash, int score, int league,string avatar)
        {
            this.ID = id;
            this.UserName = name;
            this.totalCash = totalCash;
            this.score = score;
            this.league = league;
            this.avatar = avatar;
            hand = new card[2];
        }

        public User(string id, string name, int totalcash, string avatar, card card1, card card2)
        {

            this.ID = id;
            this.UserName = name;
            this.totalCash = totalcash;
            this.avatar = avatar;
            this.hand = new card[2];
            hand[0] = card1;
            hand[1] = card2;
        }
    }
}
