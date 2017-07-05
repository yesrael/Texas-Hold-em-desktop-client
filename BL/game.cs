using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public enum GameType
    {
        LIMIT, NO_LIMIT, POT_LIMIT
    }

    public class game
    {
        public string GameID;
        public LinkedList<player> players;
        public LinkedList<player> activePlayers;
        public LinkedList<User> spectators;
        public int blindBit;
        public String CurrentPlayer;
        public int cardsOnTable;
        public card[] table;

        public int MaxPlayers;
        public int cashOnTable;
        public int CurrentBet;
        public int cashOnTheTable = 0;
        public bool isWaitingForYourAction;
        public int minimumBet;
        public int isWaitingForLeaving;
        public Queue<string> chat;
        public Queue<string> privateChat;
        public bool isWaitingForUpdate;
        public bool isThereWinners;
        public Dictionary<player, int> winnersToAmount;
        /**

* PLAYERS = "*PLAYER USER NAME*,*Player Name*,"{0,n} 
*  CARDS = "*CARD NUMBER*,*CARD TYPE*,"{0,n}
*  GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
* table=*CARDS*&MaxPlayers=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
* @param request is string that has this format: "JOINGAME *GAME ID* *USER NAME*"
* @return "JOINGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "JOINGAME *GAME ID* *USER NAME* FAILED *MSG*"
*/

        public game()
        {
            players = new LinkedList<player>();
            activePlayers = new LinkedList<player>();
            spectators = new LinkedList<User>();
            table = new card[5];
            chat = new Queue<string>();
            privateChat = new Queue<string>();
            isWaitingForYourAction = false;
            isWaitingForLeaving = 0;
            isWaitingForUpdate = true;
            isThereWinners = false;
            winnersToAmount = new Dictionary<player, int>();
            minimumBet = 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string getNewMsgInChat()
        {
            if(chat.Count>0)
            return chat.Dequeue();
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void addMsg(string msg)
        {
            chat.Enqueue(msg);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string getNewMsgInPrivateChat()
        {
            if (privateChat.Count > 0)
                return privateChat.Dequeue();
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void addWhisper(string msg)
        {
            privateChat.Enqueue(msg);
        }
    }


    public class GamePreferences
    {
        public GameType gameTypePolicy;
        public int buyInPolicy;
        public int chipPolicy;
        public int minBet;
        public int minPlayersNum;
        public int maxPlayersNum;
        public bool spectatable;

        public GamePreferences() 
        {
        }
}
}
