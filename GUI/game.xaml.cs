using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for game.xaml
    /// </summary>
    public partial class game : Window
    {
        businessLayer BL;
        BL.game Game;
        User user;
        Thread newThread;
        bool isMaked;

        BitmapImage[] cards = new BitmapImage[52];
        BitmapImage[] avatars = new BitmapImage[4];
        Border[] playerAvatar = new Border[8];
        Border[] tableCards = new Border[5];
        Label[] playerName = new Label[8];
        Label[] playerCash = new Label[8];
        Button[] messages = new Button[8];

        public game(businessLayer bl, BL.game Game, User user)
        {
            InitializeComponent();
            this.BL = bl;
            this.Game = Game;
            this.user = user;
            isMaked = false;
            InitializeAvatrsImages();
            InitializeCardsImages();
            InitializePlayersComponents();
            this.labelName.Content = this.user.UserName;
              newThread = new Thread(new ThreadStart(Run));
              newThread.Start(); 
        }

        private void Run()
        {
            while (Game.isWaitingForLeaving == 0)
            {
                updateChat(); updatePrivateChat();
                if (Game.isWaitingForUpdate)
                {
                    updateGame();
                    Game.isWaitingForUpdate = false;
                }
                while (Game.activePlayers.Count > 0)
                {
                    updateChat(); updatePrivateChat();
                    if (Game.isWaitingForUpdate)
                    {
                        updateGame();
                        Game.isWaitingForUpdate = false;
                    }
                    if (this.Game.isWaitingForYourAction && !isMaked)
                    {
                        myTurn();
                        isMaked = true;
                    }
                }
            }
        }



        private void InitializeAvatrsImages()
        {
            for (int i = 0; i < 4; i++)
            {
                if(i==1)
                avatars[1] = new BitmapImage(new Uri("avatar" + (i + 1) + ".png", UriKind.Relative));
                else
                avatars[i] = new BitmapImage(new Uri("avatar"+(i+1)+".jpg", UriKind.Relative));
            }
        }

        private void InitializePlayersComponents()
        {
            playerAvatar[0] = player1_avatar;
            playerAvatar[1] = player2_avatar;
            playerAvatar[2] = player3_avatar;
            playerAvatar[3] = player4_avatar;
            playerAvatar[4] = player5_avatar;
            playerAvatar[5] = player6_avatar;
            playerAvatar[6] = player7_avatar;
            playerAvatar[7] = player8_avatar;

            tableCards[0] = card1;
            tableCards[1] = card2;
            tableCards[2] = card3;
            tableCards[3] = card4;
            tableCards[4] = card5;

            playerName[0] = player1_name;
            playerName[1] = player2_name;
            playerName[2] = player3_name;
            playerName[3] = player4_name;
            playerName[4] = player5_name;
            playerName[5] = player6_name;
            playerName[6] = player7_name;
            playerName[7] = player8_name;

            playerCash[0] = player1_cash;
            playerCash[1] = player2_cash;
            playerCash[2] = player3_cash;
            playerCash[3] = player4_cash;
            playerCash[4] = player5_cash;
            playerCash[5] = player6_cash;
            playerCash[6] = player7_cash;
            playerCash[7] = player8_cash;

            messages[0] = message1;
            messages[1] = message2;
            messages[2] = message3;
            messages[3] = message4;
            messages[4] = message5;
            messages[5] = message6;
            messages[6] = message7;
            messages[7] = message8;
        }

        private void InitializeCardsImages()
        {
            String[] royalFamily = { "jack", "queen", "king"};

            cards[0] = new BitmapImage(new Uri("ace_of_clubs.png", UriKind.Relative));
            cards[13] = new BitmapImage(new Uri("ace_of_diamonds.png", UriKind.Relative));
            cards[26] = new BitmapImage(new Uri("ace_of_hearts.png", UriKind.Relative));
            cards[39] = new BitmapImage(new Uri("ace_of_spades.png", UriKind.Relative));

            for (int i = 1; i < 10; i++)
            {
                cards[i] = new BitmapImage(new Uri((i + 1) + "_of_clubs.png", UriKind.Relative));
                cards[i + 13] = new BitmapImage(new Uri((i + 1) + "_of_diamonds.png", UriKind.Relative));
                cards[i + 26] = new BitmapImage(new Uri((i + 1) + "_of_hearts.png", UriKind.Relative));
                cards[i + 39] = new BitmapImage(new Uri((i + 1) + "_of_spades.png", UriKind.Relative));
            }

            for (int i = 10; i < 13; i++)
            {
                cards[i] = new BitmapImage(new Uri(royalFamily[i - 10] + "_of_clubs.png", UriKind.Relative));
                cards[i + 13] = new BitmapImage(new Uri(royalFamily[i - 10] + "_of_diamonds.png", UriKind.Relative));
                cards[i + 26] = new BitmapImage(new Uri(royalFamily[i - 10] + "_of_hearts.png", UriKind.Relative));
                cards[i + 39] = new BitmapImage(new Uri(royalFamily[i - 10] + "_of_spades.png", UriKind.Relative));
            }
        }
        private void updateChat()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {

                bool isEmptyChat = false;
                while (!isEmptyChat)
                {
                    string msgg = this.Game.getNewMsgInChat();
                    if (msgg != null)
                        publicMessagesBox.Text += msgg;
                    else isEmptyChat = true;
                }
            }));
  
        }

        private void updatePrivateChat()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {

                bool isEmptyChat = false;
                while (!isEmptyChat)
                {
                    string msgg = this.Game.getNewMsgInPrivateChat();
                    if (msgg != null)
                        privateMessagesBox.Text += msgg;
                    else isEmptyChat = true;
                }
            }));

        }

        private void updateTableCards()
        {
            int numOfCards = Game.cardsOnTable;
            int[] cardIndex=new int[numOfCards];

            for(int i=0; i<numOfCards; i++)
            {
               if(Game.table[i].type.Equals(CardType.CLUBS))
                   cardIndex[i] = Game.table[i].number - 1;
               if (Game.table[i].type.Equals(CardType.DIAMONDS))
                   cardIndex[i] = Game.table[i].number - 1 + 13;
               if (Game.table[i].type.Equals(CardType.HEARTS))
                   cardIndex[i] = Game.table[i].number - 1 + 26;
               if (Game.table[i].type.Equals(CardType.SPADES))
                   cardIndex[i] = Game.table[i].number - 1 + 39;
            }

            setTableCards(numOfCards, cardIndex);
        }

        private void setTableCards(int numOfCards, int[] cardIndex)
        {
            int i;
            for(i=0; i<numOfCards; i++)
             tableCards[i].Background = new ImageBrush(cards[cardIndex[i]]);
            while(i < 5)
            {
                tableCards[i].Background = new ImageBrush();
                i++;
            }

        }

        private void updateMyCards(player myPlayer)
        {
            int[] cardIndex = new int[2];

            if (myPlayer.hand[0] != null && myPlayer.hand[1] != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (myPlayer.hand[i].type.Equals(CardType.CLUBS))
                        cardIndex[i] = myPlayer.hand[i].number - 1;
                    if (myPlayer.hand[i].type.Equals(CardType.DIAMONDS))
                        cardIndex[i] = myPlayer.hand[i].number - 1 + 13;
                    if (myPlayer.hand[i].type.Equals(CardType.HEARTS))
                        cardIndex[i] = myPlayer.hand[i].number - 1 + 26;
                    if (myPlayer.hand[i].type.Equals(CardType.SPADES))
                        cardIndex[i] = myPlayer.hand[i].number - 1 + 39;
                }
                my_first_card.Background = new ImageBrush(cards[cardIndex[0]]);
                my_second_card.Background = new ImageBrush(cards[cardIndex[1]]);
            }
            else
            {
                my_first_card.Background = new ImageBrush();
                my_second_card.Background = new ImageBrush();

            }
        }


        public void updatePlayers()
        { 
            int numOfPlayers = Game.activePlayers.Count;
            bool itIsMyTurn = false;

            for (int i = 0; i < 8; i++)
            {
                playerAvatar[i].Background = new ImageBrush();
                messages[i].Visibility = System.Windows.Visibility.Hidden;
                playerName[i].Visibility = System.Windows.Visibility.Hidden;
                playerCash[i].Visibility = System.Windows.Visibility.Hidden;
            }


            if (this.user.ID.Equals(Game.CurrentPlayer))
                itIsMyTurn = true;

                for (int i = 0; i < numOfPlayers; i++)
            {
               if(Game.activePlayers.ElementAt(i).user.avatar.Equals("avatar1"))
                playerAvatar[i].Background = new ImageBrush(avatars[0]);
               if (Game.activePlayers.ElementAt(i).user.avatar.Equals("avatar2"))
                playerAvatar[i].Background = new ImageBrush(avatars[1]);
               if (Game.activePlayers.ElementAt(i).user.avatar.Equals("avatar3"))
                playerAvatar[i].Background = new ImageBrush(avatars[2]);
               if (Game.activePlayers.ElementAt(i).user.avatar.Equals("avatar4"))
                playerAvatar[i].Background = new ImageBrush(avatars[3]);

                playerName[i].Visibility = System.Windows.Visibility.Visible;
                playerName[i].Content = Game.activePlayers.ElementAt(i).user.UserName;

                playerCash[i].Visibility = System.Windows.Visibility.Visible;
                playerCash[i].Content ="      $ "+Game.activePlayers.ElementAt(i).user.totalCash;

                bool isSpectator = true;
                foreach (player p in Game.activePlayers)
                    if (p.user.ID == user.ID)
                    {
                        isSpectator = false;
                        break;
                    }


                if (!Game.activePlayers.ElementAt(i).user.ID.Equals(this.user.ID) && !isSpectator)
                    messages[i].Visibility = System.Windows.Visibility.Visible;

                if (itIsMyTurn)
                {
                    if (Game.activePlayers.ElementAt(i).user.ID.Equals(this.user.ID))
                    {
                        playerName[i].Background = Brushes.Blue;
                        playerCash[i].Background = Brushes.Blue;
                    }
                }
                else
                {
                    if (Game.activePlayers.ElementAt(i).user.ID.Equals(this.user.ID))
                    {
                        playerName[i].Background = Brushes.DarkGray;
                        playerCash[i].Background = Brushes.DarkGray;
                    }
                }
            }
        }

        private void updateGame()
        {
            this.Game = BL.getGameByID(Game.GameID);
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                if (this.Game.isThereWinners) {
                    string messageWinner = "";
                    int i = 1;
                    foreach(KeyValuePair<player,int> p in Game.winnersToAmount) {
                        messageWinner += i + ": " + p.Key.user.UserName + " Got: " + p.Value + "$\n";
                        i++;
                    }
                    MessageBox.Show(messageWinner,"We Have Winners", MessageBoxButton.OK, MessageBoxImage.Information);
                    Game.isThereWinners = false;
                    Game.winnersToAmount = new Dictionary<player, int>();
                }
                updateTableCards();
                updatePlayers();
                cashOnTheTable.Text = "cash: " +this.Game.cashOnTheTable +"$";
                updateChat();

                foreach (player currentPlayer in this.Game.activePlayers)
                    if (currentPlayer.user.ID == this.Game.CurrentPlayer)
                    {
                        if (currentPlayer.user.ID !=user.ID)
                          now_playing.Text = "now playing: " + currentPlayer.user.UserName;
                        else
                          now_playing.Text = "now playing: Me";
                        break;
                    }

                my_first_card.Background = new ImageBrush();
                my_second_card.Background = new ImageBrush();
                foreach (player myPlayer in this.Game.activePlayers)
                if(myPlayer.user.ID==this.user.ID)
                {
                    updateMyCards(myPlayer);
                    break;
                }
            }));
            }

        private  void myTurn()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
            fold_button.Visibility = System.Windows.Visibility.Visible;
            check_button.Visibility = System.Windows.Visibility.Visible;
            call_button.Visibility = System.Windows.Visibility.Visible;
            raise_button.Visibility = System.Windows.Visibility.Visible;
            Raise_to.Visibility = System.Windows.Visibility.Visible;
            raiseBet.Visibility = System.Windows.Visibility.Visible;
            slider.Visibility = System.Windows.Visibility.Visible;

                call_button.IsEnabled = true;
                check_button.IsEnabled = true;
                if (this.Game.CurrentBet == 0)
                    call_button.IsEnabled = false;
                if (this.Game.CurrentBet > 0)
                    check_button.IsEnabled = false;

            int maxPossibleBet=this.user.totalCash;
            int minPossibleBet = this.Game.minimumBet;

                foreach (player p in this.Game.activePlayers)
                {
                    if (p.user.totalCash < maxPossibleBet)
                        maxPossibleBet = p.user.totalCash;
                    if (p.user.totalCash < minPossibleBet && p.user.totalCash>0)
                        minPossibleBet = p.user.totalCash;
                }
                  
                slider.Maximum = maxPossibleBet;
                slider.Minimum = minPossibleBet;
                slider.Value = slider.Minimum;
            }));
        }


        private void leave_game_Click(object sender, RoutedEventArgs e)
        {
            BL.leaveGame(Game.GameID, user.ID);
            newThread.Abort();
            gameCenter GC = new gameCenter(BL, user, null);
            GC.Show();
            this.Close();
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            if (BL.fold(user.ID, this.Game.GameID))
            {

                this.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application
                fold_button.Visibility = System.Windows.Visibility.Hidden;
                    check_button.Visibility = System.Windows.Visibility.Hidden;
                    call_button.Visibility = System.Windows.Visibility.Hidden;
                    raise_button.Visibility = System.Windows.Visibility.Hidden;
                    Raise_to.Visibility = System.Windows.Visibility.Hidden;
                    raiseBet.Visibility = System.Windows.Visibility.Hidden;
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }));
                isMaked = false;
            }
            

        }

        private void check_button_Click(object sender, RoutedEventArgs e)
        {
            if (BL.check(user.ID, this.Game.GameID))
            {

                this.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application
                fold_button.Visibility = System.Windows.Visibility.Hidden;
                    check_button.Visibility = System.Windows.Visibility.Hidden;
                    call_button.Visibility = System.Windows.Visibility.Hidden;
                    raise_button.Visibility = System.Windows.Visibility.Hidden;
                    Raise_to.Visibility = System.Windows.Visibility.Hidden;
                    raiseBet.Visibility = System.Windows.Visibility.Hidden;
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }));
                isMaked = false;
            }

        }

        private void call_button_Click(object sender, RoutedEventArgs e)
        {
            int bet=this.Game.minimumBet;

            foreach (player myPlayer in this.Game.activePlayers)
                if (myPlayer.user.ID == this.user.ID)
                 if(bet>myPlayer.user.totalCash)
                    {
                        bet = myPlayer.user.totalCash;
                    }

            if (BL.call(user.ID, this.Game.GameID, bet))
            {


                this.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application
                fold_button.Visibility = System.Windows.Visibility.Hidden;
                    check_button.Visibility = System.Windows.Visibility.Hidden;
                    call_button.Visibility = System.Windows.Visibility.Hidden;
                    raise_button.Visibility = System.Windows.Visibility.Hidden;
                    Raise_to.Visibility = System.Windows.Visibility.Hidden;
                    raiseBet.Visibility = System.Windows.Visibility.Hidden;
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }));
                isMaked = false;
            }
        }

        private void raise_button_Click(object sender, RoutedEventArgs e)
        {
            if (BL.raise(user.ID, this.Game.GameID, int.Parse(raiseBet.Text)))
            {

                this.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application
                fold_button.Visibility = System.Windows.Visibility.Hidden;
                    check_button.Visibility = System.Windows.Visibility.Hidden;
                    call_button.Visibility = System.Windows.Visibility.Hidden;
                    raise_button.Visibility = System.Windows.Visibility.Hidden;
                    Raise_to.Visibility = System.Windows.Visibility.Hidden;
                    raiseBet.Visibility = System.Windows.Visibility.Hidden;
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }));
                isMaked = false;
            }

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            publicWriteMessageBox.Clear();
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {

            BL.sendMsgToChar(publicWriteMessageBox.Text + "\n", this.Game.GameID, this.user.ID);
            publicWriteMessageBox.Clear();


        }

        private void message1_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(0).user.ID, Game.activePlayers.ElementAt(0).user.UserName);
            PM.Show();
        }

        private void message2_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(1).user.ID, Game.activePlayers.ElementAt(1).user.UserName);
            PM.Show();
        }

        private void message3_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(2).user.ID, Game.activePlayers.ElementAt(2).user.UserName);
            PM.Show();
        }

        private void message4_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(3).user.ID, Game.activePlayers.ElementAt(3).user.UserName);
            PM.Show();
        }

        private void message5_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(4).user.ID, Game.activePlayers.ElementAt(4).user.UserName);
            PM.Show();
        }

        private void message6_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(5).user.ID, Game.activePlayers.ElementAt(5).user.UserName);
            PM.Show();
        }

        private void message7_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(6).user.ID, Game.activePlayers.ElementAt(6).user.UserName);
            PM.Show();
        }

        private void message8_Click(object sender, RoutedEventArgs e)
        {
            privateMessage PM = new privateMessage(this.BL, Game.GameID, user.ID, Game.activePlayers.ElementAt(7).user.ID, Game.activePlayers.ElementAt(7).user.UserName);
            PM.Show();
        }

        private void whisper_Click(object sender, RoutedEventArgs e)
        {
            this.Game = BL.getGameByID(Game.GameID);
            if (Game.spectators.Count > 0)
            {
                spectatorMessage SM = new spectatorMessage(BL, Game.GameID, user.ID, Game.spectators);
                SM.Show();
            }
            else
                MessageBox.Show("there isn't avilable spectator in the game for chat");

        }
    }
}
