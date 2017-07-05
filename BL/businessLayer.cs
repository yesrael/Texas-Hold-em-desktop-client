using System;
using System.Collections.Generic;
using System.Threading;

namespace BL
{
    public class businessLayer
    {
        private static businessLayer singelton = new businessLayer();
        public static businessLayer getBL()
        {
            return singelton;
        }
        communicationLayer CL;
        int isDone;
        User user;
        string recived, gameReview;
        LinkedList<game> games;
        Dictionary<string, int> isActionMaked;
        private businessLayer()
        {
            CL = new communicationLayer();
            isDone = 0;
            user = null;
            recived = "";
            games = new LinkedList<game>();
            isActionMaked = new Dictionary<string, int>();
        }


        public void registered(string msg){
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("REG") && msg.Contains("DONE"))
                isDone = 1;
            else isDone = 2;
        }
        /**
         * 
         * @param request is string That has this format: "LOGIN *USER NAME* *PASSWORD*"
         * @return "LOGIN DONE *USER NAME* *NAME* *CASH* *SCORE* *LEAGUE* *AVATAR*" if succeed to login, "LOGIN FAILED" else
         */
        public void loggedin(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("LOGIN") && msg.Contains("DONE"))
            {
                string[] parts = msg.Split(' ');
                Console.WriteLine(msg.ToString());
                user = new User(parts[2], parts[3], Int32.Parse(parts[4]), Int32.Parse(parts[5]), Int32.Parse(parts[6]),parts[7]);
                isDone = 1;
            }

            else isDone = 2;
        }
        /**
* 
* @param request is string that has this format: "EDITPASS *USER NAME* *NEWPASSWORD*"
* @return "EDITPASS DONE" if succeed to edit the user password, "EDITPASS FAILED *MSG*" else
*/
        public void edittedUserPassword(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("EDITPASS") && msg.Contains("DONE"))
            {
                isDone = 1;
            }

            else isDone = 2;
        }

        public void takeAction(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            string[] msgs = msg.Split(' ');
            if (msg.Contains("TAKEACTION"))
            {
                getGameByID(msgs[1]).isWaitingForYourAction = true;
                getGameByID(msgs[1]).minimumBet = Int32.Parse(msgs[2]);

            }
        }

        public void edittedUserName(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("EDITUSERNAME") && msg.Contains("DONE"))
            {
                isDone = 1;
            }

            else isDone = 2;
        }


        public void edittedUserEmail(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("EDITUSEREMAIL") && msg.Contains("DONE"))
            {
                isDone = 1;
            }

            else isDone = 2;
        }

        public void edittedUserAvatar(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("EDITUSERAVATAR") && msg.Contains("DONE"))
            {
                isDone = 1;
            }

            else isDone = 2;
        }

        /** 
 *  GAMES DETAILS = "*ONE GAME DETAILS*"{0,n} 
 *  ONE GAME DETAILS= "GAMEID=*GAME ID* ENDGAME"
 * @param request is string that has this format: "SEARCHGAMESBYPOTSIZE *POT SIZE*"
 * @return "SEARCHGAMESBYPOTSIZE DONE *GAMES DETAILS*", "SEARCHGAMESBYPOTSIZE FAILED"
  */
        public void searchedGamesByPotSize(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("DONE"))
            {
                this.recived = msg;
                isDone = 1;
            }

            else isDone = 2;
        }


        /** 
* PLAYERS = "*PLAYER USER NAME* "{0,n}
* CARDS = "*CARD NUMBER* *CARD TYPE* "{0,n}
* GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
* table=*CARDS*&MaxPlayers=*NUMBER*&activePlayersNumber=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
* @param request is string that has this format: "SPECTATEGAME *GAME ID* *USER NAME*"
* @return "SPECTATEGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "SPECTATEGAME FAILED *GAME ID* *USER NAME* *MSG*"
*/
        public void spectated(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("SPECTATEGAME") && msg.Contains("DONE"))
            {
                String part2 = msg.Substring(msg.IndexOf("DONE ") + "DONE ".Length);
                string[] msgs = part2.Split('&');
                game newGame = new game();
                string players = extractString2(msgs, "players=");
                LinkedList<player> playerss = extractPlayers(players);
                players = extractString2(msgs, "activePlayers=");
                LinkedList<player> activePlayers = extractPlayers(players);
                card[] table = extractCards(extractString2(msgs, "table="));

                newGame.GameID = extractString2(msgs, "GameID=");
                newGame.players = playerss;
                newGame.activePlayers = activePlayers;
                newGame.blindBit = Int32.Parse(extractString2(msgs, "blindBit="));
                newGame.CurrentPlayer = extractString2(msgs, "CurrentPlayer=");
                newGame.table = table;
                if (table != null)
                    newGame.cardsOnTable = table.Length;
                else newGame.cardsOnTable = 0;
                newGame.MaxPlayers = Int32.Parse(extractString2(msgs, "MaxPlayers="));
                newGame.cashOnTheTable = Int32.Parse(extractString2(msgs, "cashOnTheTable="));
                newGame.CurrentBet = Int32.Parse(extractString2(msgs, "CurrentBet="));
                this.games.AddFirst(newGame);
                isDone = 1;
            }

            else isDone = 2;
        }

        /**

     * PLAYERS = "*PLAYER USER NAME*,*Player Name*,"{0,n} 
     *  CARDS = "*CARD NUMBER*,*CARD TYPE*,"{0,n}
     *  GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
     * table=*CARDS*&MaxPlayers=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
     * @param request is string that has this format: "JOINGAME *GAME ID* *USER NAME*"
     * @return "JOINGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "JOINGAME *GAME ID* *USER NAME* FAILED *MSG*"
     */
        public void joinedGame(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("JOINGAME") && msg.Contains("DONE"))
            {
                String part2 = msg.Substring(msg.IndexOf("DONE ") + "DONE ".Length);
                string[] msgs = part2.Split('&');
                game newGame = new game();
                string players = extractString2(msgs, "players=");
                LinkedList<player> playerss = extractPlayers(players);
                players = extractString2(msgs, "activePlayers=");
                LinkedList<player> activePlayers = extractPlayers(players);
                card[] table = extractCards(extractString2(msgs, "table="));

                newGame.GameID = extractString2(msgs, "GameID=");
                newGame.players = playerss;
                newGame.activePlayers = activePlayers;
                newGame.blindBit = Int32.Parse(extractString2(msgs, "blindBit="));
                newGame.CurrentPlayer = extractString2(msgs, "CurrentPlayer=");
                newGame.table = table;
                if (table != null)
                    newGame.cardsOnTable = table.Length;
                else newGame.cardsOnTable = 0;
                newGame.MaxPlayers = Int32.Parse(extractString2(msgs, "MaxPlayers="));
                newGame.cashOnTheTable = Int32.Parse(extractString2(msgs, "cashOnTheTable="));
                newGame.CurrentBet = Int32.Parse(extractString2(msgs, "CurrentBet="));
                this.games.AddFirst(newGame);
                isDone = 1;
            }

            else isDone = 2;
        }


        /**

* PLAYERS = "*PLAYER USER NAME*,*Player Name*,"{0,n} 
*  CARDS = "*CARD NUMBER*,*CARD TYPE*,"{0,n}
*  GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
* table=*CARDS*&MaxPlayers=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
* @param request is string that has this format: "JOINGAME *GAME ID* *USER NAME*"
* @return "JOINGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "JOINGAME *GAME ID* *USER NAME* FAILED *MSG*"
*/
        public void gameUpdated(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("GAMEUPDATE"))
            {
                String part2 = msg.Substring(msg.IndexOf("GAMEUPDATE ") + "GAMEUPDATE ".Length);
                string[] msgs = part2.Split('&');
                int myPrevTotalCash = user.totalCash;
                game newGame;
                string players = extractString2(msgs, "players=");
                LinkedList<player> playerss = extractPlayers(players);
                players = extractString2(msgs, "activePlayers=");
                LinkedList<player> activePlayers = extractPlayers(players);
                string spectators = extractString2(msgs, "spectators=");
                LinkedList<User> spectatorss = extractSpectators(spectators);
                card[] table = extractCards(extractString2(msgs, "table="));


                newGame = getGameByID(extractString2(msgs, "GameID="));
                if (newGame != null)
                {
                    newGame.players = playerss;

                    newGame.blindBit = Int32.Parse(extractString2(msgs, "blindBit="));
                    newGame.CurrentPlayer = extractString2(msgs, "CurrentPlayer=");
                    newGame.table = new card[0];
                    newGame.table = table;
                    if (table != null)
                        newGame.cardsOnTable = table.Length;
                    else newGame.cardsOnTable = 0;
                    newGame.MaxPlayers = Int32.Parse(extractString2(msgs, "MaxPlayers="));

                    int PrevcashonTable = newGame.cashOnTheTable;
                    newGame.cashOnTheTable = Int32.Parse(extractString2(msgs, "cashOnTheTable="));
                    //find winners if there
                    if (PrevcashonTable > newGame.cashOnTheTable && newGame.cashOnTheTable == 0) {
                        newGame.isThereWinners = true;
                        foreach (player p in activePlayers) {
                            foreach (player prev in newGame.activePlayers) {
                                if (p.user.ID.Equals(prev.user.ID) && (p.user.totalCash > prev.user.totalCash)) 
                                    newGame.winnersToAmount.Add(p, p.user.totalCash - prev.user.totalCash);
                                else if (p.user.ID.Equals(prev.user.ID) && p.user.ID.Equals(user.ID) && (p.user.totalCash > myPrevTotalCash))
                                    newGame.winnersToAmount.Add(p, p.user.totalCash - myPrevTotalCash);
                            }
                                }
                    }
                    newGame.activePlayers = activePlayers;
                    newGame.spectators = spectatorss;
                    newGame.CurrentBet = Int32.Parse(extractString2(msgs, "CurrentBet="));
                    newGame.isWaitingForUpdate = true;
                }

            }
        }

        /** 
* PLAYERS = "*PLAYER USER NAME* "{0,n}
* CARDS = "*CARD NUMBER* *CARD TYPE* "{0,n}
* GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
* table=*CARDS*&MaxPlayers=*NUMBER*&activePlayersNumber=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
* GAME PREF = gameTypePolicy=*GAME TYPE POLICY*&potLimit=*POT LIMIT*&buyInPolicy=*BUY IN POLICY*&chipPolicy=*CHIP POLICY*&minBet=*MIN BET*&minPlayersNum=*MIN PLAY NUM*&maxPlayersNum=*MAX PLAYER NUMBER*&spectatable=*T/F*&leaguable=*T/F*&league=*NUNBER*
* @param request is string that has this format: "CREATEGAME *USER NAME* *GAME PREF*"
* @return "CREATEGAME *USER NAME* DONE *GAME FULL DETAILS*", "CREATEGAME FAILED" else
*/
        public void createdGame(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("CREATEGAME") && msg.Contains("DONE"))
            {
                String part2 = msg.Substring(msg.IndexOf("DONE ")+"DONE ".Length);
                string[] msgs = part2.Split('&');
                game newGame = new game();
            //    Console.WriteLine(extractString(part2, "players="));
            //    Console.WriteLine(extractString2(msgs, "players="));
                string players = extractString2(msgs, "players=");
                LinkedList<player> playerss = extractPlayers(players);
                players = extractString2(msgs, "activePlayers=");
                LinkedList<player> activePlayers = extractPlayers(players);
                card[] table = extractCards(extractString2(msgs, "table="));

                newGame.GameID = extractString2(msgs, "GameID=");
                newGame.players = playerss;
                newGame.activePlayers = activePlayers;
                string blindBitS = extractString2(msgs, "blindBit=");
                newGame.blindBit = Int32.Parse(extractString2(msgs, "blindBit="));
                newGame.CurrentPlayer = extractString2(msgs, "CurrentPlayer=");
                newGame.table = table;
                if (table != null)
                    newGame.cardsOnTable = table.Length;
                else newGame.cardsOnTable = 0;
                blindBitS = extractString2(msgs, "MaxPlayers=");
                newGame.MaxPlayers = Int32.Parse(extractString2(msgs, "MaxPlayers="));
                blindBitS = extractString2(msgs, "cashOnTheTable=");
                newGame.cashOnTheTable = Int32.Parse(extractString2(msgs, "cashOnTheTable="));
                blindBitS = extractString2(msgs, "CurrentBet=");
                newGame.CurrentBet = Int32.Parse(extractString2(msgs, "CurrentBet="));
                this.games.AddFirst(newGame);
                this.recived = newGame.GameID;
                isDone = 1;
            }

            else isDone = 2;
        }
        private string extractString(string input, string splitter)
        {
            int beginning = input.IndexOf(splitter) + splitter.Length;
            int end = input.IndexOf("&", beginning) == -1 ? input.Length - 1 : input.IndexOf("&", beginning);
            return input.Substring(beginning, end);

        }
        private string extractString2(string[] input, string splitter)
        {
            foreach (string s in input) {
                if (s.Contains(splitter)) {
                    return s.Substring(splitter.Length);
                }

            }
            return "";

        }

        //PLAYERS = "*PLAYER USER NAME*,*PLAYER NAME*,*CASH*,*HAND*,*AVATAR*
        private LinkedList<player> extractPlayers(string players)
        {
            int i = 0;
            string name;
            string ID;
            int cash;

            card card1 = null;
            card card2 = null;
            string avatar;
            LinkedList<player> result = new LinkedList<player>();
            while (i < players.Length - 1)
            {

                ID = players.Substring(i, players.IndexOf(",", i) - i);
                i = players.IndexOf(",", i) + 1;
                name = players.Substring(i, players.IndexOf(",", i) - i);
                i = players.IndexOf(",", i) + 1;
                cash = Int32.Parse(players.Substring(i, players.IndexOf(",", i) - i));
                i = players.IndexOf(",", i) + 1;
                string cards = players.Substring(i, players.IndexOf(",", i) - i);
                i = players.IndexOf(",", i) + 1;

                avatar = players.Substring(i, players.IndexOf(",", i) - i);
                i = players.IndexOf(",", i) + 1;

                string[] cardDetil = cards.Split(' ');
                if (!cardDetil[0].Contains("NU"))
                {
                    card1 = new card(Int32.Parse(cardDetil[1]), toCardType(cardDetil[0]));
                    card2 = new card(Int32.Parse(cardDetil[3]), toCardType(cardDetil[2]));
                }
                player p = new player(); ;
                if (!this.user.ID.Equals(ID))
                {

                    p.user = new User(ID, name, cash, avatar, card1, card2);
                    p.hand[0] = card1;
                    p.hand[1] = card2;
                }
                else
                {
                    this.user.UserName = name;
                    this.user.totalCash = cash;
                    this.user.avatar = avatar;
                    this.user.hand[0] = card1;
                    this.user.hand[1] = card2;
                    p.hand[0] = card1;
                    p.hand[1] = card2;
                    p.user = this.user;
                }
                result.AddLast(p);
            }
            return result;
        }

        private LinkedList<User> extractSpectators(string spectators)
        {
            int i = 0;
            string name;
            string ID;

            LinkedList<User> result = new LinkedList<User>();
            while (i < spectators.Length - 1)
            {

                ID = spectators.Substring(i, spectators.IndexOf(",", i) - i);
                i = spectators.IndexOf(",", i) + 1;
                name = spectators.Substring(i, spectators.IndexOf(",", i) - i);
                i = spectators.IndexOf(",", i) + 1;

                if (ID != user.ID)
                {
                    User u = new User(ID, name, 0, 0, 0, null);
                    result.AddLast(u);
                }
            }
            return result;
        }

        private CardType toCardType(string type)
        {

            if (type.Contains("SPADES")) return CardType.SPADES;
            else if (type.Contains("HEARTS")) return CardType.HEARTS;
            else if (type.Contains("DIAMONDS")) return CardType.DIAMONDS;
            else if (type.Contains("CLUBS")) return CardType.CLUBS;
            else return CardType.SPADES;
        }

        private card[] extractCards(string cards)
        {
            int i = 0;
            string cardType;
            string CardNumber;
            List<card> result = new List<card>();
            while (i < cards.Length - 1)
            {
                
                CardNumber = cards.Substring(i, cards.IndexOf(",", i) - i);
                i = cards.IndexOf(",", i) + 1;
                cardType = cards.Substring(i, cards.IndexOf(",", i) - i);
                i = cards.IndexOf(",", i) + 1;

                card p = new card(Int32.Parse(CardNumber), toCardType(cardType));

                result.Add(p);
            }
            return result.ToArray();
        }



        /**
         * 	
         * @param request is string That has this format: "REG *USER NAME* *PASSWORD* *NAME* *EMAIL* *AVATAR*"
         * @return "REG DONE" if the registration done, "REG FAILED" else
         */
        public bool register(String ID, String password, String name, String email, String avatar)              
        {

            isDone = 0;
            if (CL.send("REG " + ID + " " + password + " " + name + " " + email+" "+avatar+"\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }
        /**
         * 
         * @param request is string That has this format: "LOGIN *USER NAME* *PASSWORD*"
         * @return "LOGIN DONE *USER NAME* *NAME* *CASH* *SCORE* *LEAGUE*" if succeed to login, "LOGIN FAILED" else
         */
        public bool login(String ID, String password)
        {
            isDone = 0;
            if (CL.send("LOGIN " + ID + " " + password +"\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }

        public User getUser(String ID)
        {
            return this.user;
        }
        /**
 * 
 * @param id is string that has this format: "LOGOUT *USER NAME*"
 * @return "LOGOUT DONE" if succeed to logout, "LOGOUT FAILED *MSG*" else
 */
        public void logout(String ID)
        {
            isDone = 0;
            if (!CL.send("LOGOUT " + ID + "\n"))
            {
                logout(ID);
            }
            this.user = null;
        }
        /**
 * 
 * @param request is string that has this format: "EDITPASS *USER NAME* *NEWPASSWORD*"
 * @return "EDITPASS DONE" if succeed to edit the user password, "EDITPASS FAILED *MSG*" else
 */
        public bool editUserPassword(String userID, String oldPassword,  String newPassword)
        {
            isDone = 0;
            if (CL.send("EDITPASS " + userID + " " + oldPassword + " " + newPassword + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }






        public bool editUserName(String userID, String newName)
        {
            //TODO: call function via communication layer
            isDone = 0;
            if (CL.send("EDITUSERNAME " + userID + " " + newName + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    user.UserName = newName;
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool editUserEmail(String userID, String newEmail)
        {
            //TODO: call function via communication layer
            isDone = 0;
            if (CL.send("EDITUSEREMAIL " + userID + " " + newEmail + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }

        public bool editUserAvatar(String userID, String avatar)
        {
            //TODO: call function via communication layer
            isDone = 0;
            if (CL.send("EDITUSERAVATAR " + userID + " " + avatar + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    user.avatar = avatar;
                    return true;
                }
                return false;
            }
            return false;
        }
        /** 
         *  GAMES DETAILS = "*ONE GAME DETAILS*"{0,n} 
         *  ONE GAME DETAILS= "GAMEID=*GAME ID* ENDGAME"
         * @param request is string that has this format: "SEARCHGAMESBYPOTSIZE *POT SIZE*"
         * @return "SEARCHGAMESBYPOTSIZE DONE *GAMES DETAILS*", "SEARCHGAMESBYPOTSIZE FAILED"
          */
        public LinkedList<string> searchGamesByPotSize(int potSize)
        {
            isDone = 0;
            if (CL.send("SEARCHGAMESBYPOTSIZE " + potSize + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    LinkedList<string> result = new LinkedList<string>();
                    if (!recived.Contains("GAMEID=")) return null;
                    String part2 = recived.Substring(recived.IndexOf("GAMEID="));
                    if (!part2.StartsWith("GAMEID=")) return null;
                    int i = 0;
                    while (i < part2.Length - 3)
                    {
                        i = part2.IndexOf("GAMEID=") + "GAMEID=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" BUYIN=", i) - i));
                        i = part2.IndexOf(" BUYIN=") + " BUYIN=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" SETS=", i) - i));
                        i = part2.IndexOf(" SETS=") + " SETS=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" ENDGAME ", i) - i));
                        i = part2.IndexOf(" ENDGAME ", i) + " ENDGAME ".Length;
                        part2 = part2.Substring(part2.IndexOf(" ENDGAME ") + " ENDGAME ".Length);
                        i = 0;
                    }

                    this.recived = "";
                    return result;
                }
                this.recived = "";
                return null;
            }
            this.recived = "";
            return null;
        }

        /** 
         *  /** 
         *  GAMES DETAILS = "*ONE GAME DETAILS*"{0,n} 
         *  ONE GAME DETAILS= "GAMEID=*GAME ID* ENDGAME"
         * @param request is string that has this format: "SEARCHGAMESBYPOTSIZE *POT SIZE*"
         * @return "SEARCHGAMESBYPOTSIZE DONE *GAMES DETAILS*", "SEARCHGAMESBYPOTSIZE FAILED"
          */
        public LinkedList<string> searchGamesByPlayerName(String name)
        {
            isDone = 0;
            if (CL.send("SEARCHGAMESBYPLAYERNAME " + name + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    LinkedList<string> result = new LinkedList<string>();
                    if (!recived.Contains("GAMEID=")) return null;
                    String part2 = recived.Substring(recived.IndexOf("GAMEID="));
                    if (!part2.StartsWith("GAMEID=")) return null;
                    int i = 0;
                    while (i < part2.Length - 3)
                    {
                        i = part2.IndexOf("GAMEID=") + "GAMEID=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" BUYIN=", i) - i));
                        i = part2.IndexOf(" BUYIN=") + " BUYIN=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" SETS=", i) - i));
                        i = part2.IndexOf(" SETS=") + " SETS=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" ENDGAME ", i) - i));
                        i = part2.IndexOf(" ENDGAME ", i) + " ENDGAME ".Length;
                        part2 = part2.Substring(part2.IndexOf(" ENDGAME ") + " ENDGAME ".Length);
                        i = 0;
                    }

                    this.recived = "";
                    return result;
                }
                this.recived = "";
                return null;
            }
            this.recived = "";
            return null;
        }

        /**

     * PLAYERS = "*PLAYER USER NAME*,*Player Name*,"{0,n} 
     *  CARDS = "*CARD NUMBER*,*CARD TYPE*,"{0,n}
     *  GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
     * table=*CARDS*&MaxPlayers=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
     * @param request is string that has this format: "JOINGAME *GAME ID* *USER NAME*"
     * @return "JOINGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "JOINGAME *GAME ID* *USER NAME* FAILED *MSG*"
     */
        public bool joinGame(String gameID, String UserID)
        {
            //TODO: call function via communication layer
            isDone = 0;
            if (CL.send("JOINGAME " + gameID + " " + UserID + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }

        public game getGameByID(String gameID)
        {
            foreach (game currentGame in games)
            {
                if (currentGame.GameID.Equals(gameID))
                    return currentGame;
            }
            return null;
        }

        /** 
     * PLAYERS = "*PLAYER USER NAME* "{0,n}
     * CARDS = "*CARD NUMBER* *CARD TYPE* "{0,n}
     * GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
     * table=*CARDS*&MaxPlayers=*NUMBER*&activePlayersNumber=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
     * GAME PREF = gameTypePolicy=*GAME TYPE POLICY*&potLimit=*POT LIMIT*&buyInPolicy=*BUY IN POLICY*&chipPolicy=*CHIP POLICY*&minBet=*MIN BET*&minPlayersNum=*MIN PLAY NUM*&maxPlayersNum=*MAX PLAYER NUMBER*&spectatable=*T/F*&leaguable=*T/F*&league=*NUNBER*
     * @param request is string that has this format: "CREATEGAME *USER NAME* *GAME PREF*"
     * @return "CREATEGAME *USER NAME* *GAME FULL DETAILS*", "CREATEGAME FAILED" else
 */

        public String createGame(String UserID, GameType type, int Limit, int buyIn, int chipPolicy, int minBet,
              int minPlayers, int maxPlayers, bool spectatable, bool leaguable,int league)
        {
            string result = null; 
            isDone = 0;
            string spect = "";
            string leagu = "";
            if (spectatable) spect = "T";
            else spect = "F";
            if (leaguable) leagu = "T";
            else leagu = "F";
            if (CL.send("CREATEGAME " + UserID + " gameTypePolicy=" + type + "&potLimit=" + Limit + "&buyInPolicy=" +buyIn+
                "&chipPolicy="+ chipPolicy + "&minBet="+ minBet + "&minPlayersNum="+ minPlayers + "&maxPlayersNum="+ maxPlayers + "&spectatable="+spect+ "&leaguable="+leagu+ "&league="+ league + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) {
                    result = recived;
                    this.recived = "";
                    return result; }
                this.recived = "";
                return "one";
            }
            this.recived = "";
            return "two";
        }

        /** 
 * PLAYERS = "*PLAYER USER NAME* "{0,n}
 * CARDS = "*CARD NUMBER* *CARD TYPE* "{0,n}
 * GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
 * table=*CARDS*&MaxPlayers=*NUMBER*&activePlayersNumber=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
 * GAME PREF = gameTypePolicy=*GAME TYPE POLICY*&potLimit=*POT LIMIT*&buyInPolicy=*BUY IN POLICY*&chipPolicy=*CHIP POLICY*&minBet=*MIN BET*&minPlayersNum=*MIN PLAY NUM*&maxPlayersNum=*MAX PLAYER NUMBER*&spectatable=*T/F*&leaguable=*T/F*&league=*NUNBER*
 * @param request is string that has this format: "CREATEGAME *USER NAME* *GAME PREF*"
 * @return "CREATEGAME *USER NAME* *GAME FULL DETAILS*", "CREATEGAME FAILED" else
*/

        public LinkedList<string> searchGameByPrefs(GameType type, int Limit, int buyIn, int chipPolicy, int minBet,
              int minPlayers, int maxPlayers, bool spectatable, bool leaguable, int league)
        {
            isDone = 0;
            string spect = "";
            string leagu = "";
            if (spectatable) spect = "T";
            else spect = "F";
            if (leaguable) leagu = "T";
            else leagu = "F";
            if (CL.send("SEARCHGAMESBYPREFS " + "gameTypePolicy=" + type + "&potLimit=" + Limit + "&buyInPolicy=" + buyIn +
                "&chipPolicy=" + chipPolicy + "&minBet=" + minBet + "&minPlayersNum=" + minPlayers + "&maxPlayersNum=" + maxPlayers + "&spectatable=" + spect + "&leaguable=" + leagu + "&league=" + league + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {

                    LinkedList<string> result = new LinkedList<string>();
                    if (!recived.Contains("GAMEID=")) return null;
                    String part2 = recived.Substring(recived.IndexOf("GAMEID="));
                    if (!part2.StartsWith("GAMEID=")) return null;
                    int i = 0;
                    while (i < part2.Length - 3)
                    {
                        i = part2.IndexOf("GAMEID=") + "GAMEID=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" BUYIN=", i) - i));
                        i = part2.IndexOf(" BUYIN=") + " BUYIN=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" SETS=", i) - i));
                        i = part2.IndexOf(" SETS=") + " SETS=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" ENDGAME ", i) - i));
                        i = part2.IndexOf(" ENDGAME ", i) + " ENDGAME ".Length;
                        part2 = part2.Substring(part2.IndexOf(" ENDGAME ") + " ENDGAME ".Length);
                        i = 0;
                    }

                    this.recived = "";
                    foreach (String str in result)
                        Console.WriteLine("*******************"+str+"****************");
                    return result;
                }
                this.recived = "";
                return null;
            }
            this.recived = "";
            return null;
        }

        /** 
 *  GAMES DETAILS = "*ONE GAME DETAILS*"{0,n} 
 *  ONE GAME DETAILS= "GAMEID=*GAME ID* ENDGAME "
 * @param request is string that has this format: "LISTJOINABLEGAMES *USER NAME*"
 * @return "LISTJOINABLEGAMES DONE *GAMES DETAILS*", "LISTJOINABLEGAMES FAILED"
 */
        public LinkedList<string> listOfJoinableGames(String UserID)
        {
            isDone = 0;
            if (CL.send("LISTJOINABLEGAMES " + UserID + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    LinkedList<string> result = new LinkedList<string>();
                    if (!recived.Contains("GAMEID=")) return null;
                    String part2 = recived.Substring(recived.IndexOf("GAMEID="));
                    if (!part2.StartsWith("GAMEID=")) return null;
                    int i = 0;
                    while (i < part2.Length - 3)
                    {
                        i = part2.IndexOf("GAMEID=") + "GAMEID=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" BUYIN=", i) - i));
                        i = part2.IndexOf(" BUYIN=") + " BUYIN=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" SETS=", i) - i));
                        i = part2.IndexOf(" SETS=") + " SETS=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" ENDGAME ", i) - i));
                        i = part2.IndexOf(" ENDGAME ", i) + " ENDGAME ".Length;
                        part2 = part2.Substring(part2.IndexOf(" ENDGAME ")+ " ENDGAME ".Length);
                        i = 0;
                    }

                    this.recived = "";
                    return result;
                }
                this.recived = "";
                return null;
            }
            this.recived = "";
            return null;
        }

        /** 
 *  GAMES DETAILS = "*ONE GAME DETAILS*"{0,n} 
 *  ONE GAME DETAILS= "GAMEID=*GAME ID* ENDGAME "
 * @param request is string that has this format: "LISTSPECTATEABLEGAMES"
 * @return "LISTSPECTATEABLEGAMES DONE *GAMES DETAILS*", "LISTSPECTATEABLEGAMES FAILED" 
 */
        public LinkedList<string> listOfSpectatableGames()
        {
            isDone = 0;
            if (CL.send("LISTSPECTATEABLEGAMES\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1)
                {
                    LinkedList<string> result = new LinkedList<string>();
                    if (!recived.Contains("GAMEID=")) return null;
                    String part2 = recived.Substring(recived.IndexOf("GAMEID="));
                    if (!part2.StartsWith("GAMEID=")) return null;
                    int i = 0;
                    while (i < part2.Length - 3)
                    {
                        i = part2.IndexOf("GAMEID=") + "GAMEID=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" BUYIN=", i) - i));
                        i = part2.IndexOf(" BUYIN=") + " BUYIN=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" SETS=", i) - i));
                        i = part2.IndexOf(" SETS=") + " SETS=".Length;
                        result.AddLast(part2.Substring(i, part2.IndexOf(" ENDGAME ", i) - i));
                        i = part2.IndexOf(" ENDGAME ", i) + " ENDGAME ".Length;
                        part2 = part2.Substring(part2.IndexOf(" ENDGAME ") + " ENDGAME ".Length);
                        i = 0;
                    }

                    this.recived = "";
                    return result;
                }
                this.recived = "";
                return null;
            }
            this.recived = "";
            return null;
        }

        /** 
 * PLAYERS = "*PLAYER USER NAME* "{0,n}
 * CARDS = "*CARD NUMBER* *CARD TYPE* "{0,n}
 * GAME FULL DETAILS= "GameID=*ID*&players=*PLAYERS*&activePlayers=*PLAYERS*&blindBit=*NUMBER*&CurrentPlayer=*PLAYER USER NAME*&
 * table=*CARDS*&MaxPlayers=*NUMBER*&activePlayersNumber=*NUMBER*&cashOnTheTable=*NUMBER*&CurrentBet=*NUMBER*"
 * @param request is string that has this format: "SPECTATEGAME *GAME ID* *USER NAME*"
 * @return "SPECTATEGAME *GAME ID* *USER NAME* DONE *GAME FULL DETAILS*", "SPECTATEGAME FAILED *GAME ID* *USER NAME* *MSG*"
 */
        public bool spectateGame(String UserID, String GameID)
        {
            isDone = 0;
            if (CL.send("SPECTATEGAME " + GameID + " " + UserID + "\n"))
            {
                while (isDone == 0) ;
                if (isDone == 1) return true;
                return false;
            }
            return false;
        }

        /** 
* ACTION TYPE = FOLD/BET/CHECK
* MONEY = NUMBER/NULL
* @param action is string that has this format: "ACTION *ACTION TYPE* *GAME ID* *USER NAME* *MONEY*"
* @return "ACTION *ACTION TYPE* *GAME ID* *USER NAME* DONE" if succeed to make the action,"ACTION *ACTION TYPE* *GAME ID* *USER NAME* FAILED" else
*/
        public bool Action(String UserID, String GameID, String actionType, int money)
        {
            if (this.isActionMaked.ContainsKey(GameID))
                return false;

            string toSend = "";
            if (actionType.Equals("FOLD") || actionType.Equals("CHECK"))
                toSend = "ACTION " + actionType + " " + GameID + " " + UserID;
            else if (actionType.Equals("BET"))
                toSend = "ACTION " + actionType + " " + GameID + " " + UserID + " " + money;
            else return false;
            this.isActionMaked.Add(GameID, 0);
            int value = 0;
            toSend += "\n";
            if (CL.send(toSend))
            {
                while (isActionMaked[GameID] == 0) ;
                value = isActionMaked[GameID];
                isActionMaked.Remove(GameID);
                if (value == 1) return true;
                return false;
            }
            return false;
        }

        /** 
* ACTION TYPE = FOLD/BET/CHECK
* MONEY = NUMBER/NULL
* @param action is string that has this format: "ACTION *ACTION TYPE* *GAME ID* *USER NAME* *MONEY*"
* @return "ACTION *ACTION TYPE* *GAME ID* *USER NAME* DONE" if succeed to make the action,"ACTION *ACTION TYPE* *GAME ID* *USER NAME* FAILED" else
*/
        public void ActionMakedd(string msg)
        {
            string[] parts = msg.Split(' ');
            if (msg.Contains("ACTION") && msg.Contains("DONE"))
            {

                Console.WriteLine(msg.ToString());   
                getGameByID(parts[2]).isWaitingForYourAction = false;
                this.isActionMaked[parts[2]] = 1;

            }

            else this.isActionMaked[parts[2]] = 2;
        }

        public void sendMsgToChar(string msg, string GameID, string UserID)
        {
            CL.send("CHATMSG " + GameID + " " + UserID + " " + msg);

        }

        public void reciveMsgToChat(string msg)
        {
            string[] partsMsg = msg.Split(' ');
            bool isFound = false;

            foreach (player p in getGameByID(partsMsg[1]).activePlayers)
            {
                if (p.user.ID == partsMsg[2])
                {
                    getGameByID(partsMsg[1]).addMsg(p.user.UserName + ": " + appendArray(partsMsg, 3));
                    isFound = true;
                    break;
                }
            }
            if(!isFound)
            foreach (User u in getGameByID(partsMsg[1]).spectators)
            {
                if (u.ID == partsMsg[2])
                {
                    getGameByID(partsMsg[1]).addMsg(u.UserName + ": " + appendArray(partsMsg, 3));
                        isFound = true;
                        break;
                }
            }
            if(!isFound && user.ID== partsMsg[2])
                getGameByID(partsMsg[1]).addMsg(user.UserName + ": " + appendArray(partsMsg, 3));
        }

        public void sendWhisper(string msg, string GameID, string UserID, string receiverID)
        {
            CL.send("WHISPERMSG " + GameID + " " + UserID + " " + receiverID + " " + msg);

        }

        public void reciveWhisper(string msg)
        {
            string[] partsMsg = msg.Split(' ');
            bool isPlayer = false;

            foreach (player p in getGameByID(partsMsg[1]).activePlayers)
            {
                if (p.user.ID == partsMsg[2])
                {
                    getGameByID(partsMsg[1]).addWhisper(p.user.UserName + ": " + appendArray(partsMsg, 4));
                    isPlayer = true;
                    break;
                }
            }
            if(!isPlayer)
            foreach (User u in getGameByID(partsMsg[1]).spectators)
            {
                if (u.ID == partsMsg[2])
                {
                    getGameByID(partsMsg[1]).addWhisper(u.UserName + ": " + appendArray(partsMsg, 4));
                    break;
                }
            }
        }

        private string appendArray(string[] partsMsg, int startIndex)
        {
            string result = "";
            for (int i = startIndex; i < partsMsg.Length; i++)
            {
                result += partsMsg[i] + " ";
            }
            return result;
        }

        /**
 * 
 * @param request is string that has this format: "LEAVEGAME *GAME ID* *USER NAME*"
 * @return "LEAVEGAME *GAME ID* *USER NAME* DONE" if succeed to leave, "LEAVEGAME *GAME ID* *USER NAME* FAILED *MSG*" else
 */
        public bool leaveGame(String GameID, String UserID)
        {
            getGameByID(GameID).isWaitingForLeaving = 0; 
            if (CL.send("LEAVEGAME " + GameID + " " + UserID + "\n"))
            {
                game currentGame = getGameByID(GameID);
                while (currentGame.isWaitingForLeaving == 0) ;
                if (currentGame.isWaitingForLeaving == 1) return true;
                return false;
            }
            return false;
        }

        public void leavedGame(string msg)
        {
            string[] parts = msg.Split(' ');
            if (msg.Contains("LEAVEGAME") && msg.Contains("DONE"))
            {

                Console.WriteLine(msg.ToString());
                getGameByID(parts[1]).isWaitingForLeaving = 1;

            }

            else getGameByID(parts[1]).isWaitingForLeaving = 2;
        }

        public bool check(String userID, String gameID)
        {
            return Action(userID, gameID, "CHECK", 0);
        }

        public bool fold(String userID, String gameID)
        {
            return Action(userID, gameID, "FOLD", 0);
        }
        public bool raise(String userID, String gameID, int money)
        {
            return Action(userID, gameID, "BET", money);
        }
        public bool call(String userID, String gameID, int money)
        {
            return Action(userID, gameID, "BET", money);
        }

        public void gameReplayed(string msg)
        {
            msg = msg.Substring(0, msg.Length - 2);
            if (msg.Contains("GAMEREPLAY") && msg.Contains("DONE"))
            {
                string[] parts = msg.Split(' ');
                gameReview = msg.Substring(14, msg.Length-18);
                isDone = 1;
            }
            else isDone = 2;
        }

        public String gameReplay(String GameID)
        {
            isDone = 0;
            if (CL.send("REPLAY " + GameID + "\n"))
            {
                while (isDone == 0) ;

                if (isDone == 1) return gameReview;
                return null;
            }
            return null;
        }

    }
}
