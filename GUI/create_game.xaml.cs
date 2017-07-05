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

namespace GUI
{
    /// <summary>
    /// Interaction logic for create_game.xaml
    /// </summary>
    public partial class create_game : Window
    {
        businessLayer BL;
        User user;
        bool isCreate;

        public create_game(businessLayer bl, User user, bool isCreate)
        {
            InitializeComponent();
            this.BL = bl;
            this.user = user;
            this.isCreate = isCreate;
            if (isCreate)
                create_button.Content = "CREATE";
            else
                create_button.Content = "SEARCH";
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            gameCenter GC = new gameCenter(BL, user, null);
            GC.Show();
            this.Close();
        }

        private void create_button_Click(object sender, RoutedEventArgs e)
        {
            String gameID="";
            int limit, buyIN, chipPolicy, minBet, minPlayers, MaxPlayers;
            bool spectatable=false, leaguable=false;
            GameType type = GameType.POT_LIMIT;
            try
            { limit = int.Parse(limit_textBox.Text); }
            catch { limit = 0; }
            try
            { buyIN = int.Parse(buyIn_textBox.Text); }
            catch { buyIN = 0; }
            try
            { chipPolicy = int.Parse(chipPolicy_textBox.Text); }
            catch { chipPolicy = 0; }
            try
            { minBet = int.Parse(minBet_textBox.Text); }
            catch { minBet = 0; }
            try
            { minPlayers = int.Parse(minPlayers_textBox.Text); }
            catch { minPlayers = 2; }
            try
            { MaxPlayers = int.Parse(maxPlayers_textBox.Text); }
            catch { MaxPlayers = 8; }
            if (spectatable_comboBox.SelectedIndex==0)
                spectatable = true;
            if (leaguable_comboBox.SelectedIndex == 0)
                leaguable = true;
            if (comboBox.SelectedIndex == 0)
                type = GameType.LIMIT;
            if (comboBox.SelectedIndex == 1)
                type = GameType.NO_LIMIT;

            if (isCreate)
            {
                gameID = BL.createGame(user.ID, type, limit, buyIN, chipPolicy, minBet, minPlayers, MaxPlayers, spectatable, leaguable, this.user.league);
                if (gameID == null)
                    MessageBox.Show("error \n one or more of the parameters is invalid");

                BL.game game = BL.getGameByID(gameID);
                game g = new game(BL, game, user);
                g.Show();
                this.Close();
            }
            else
            {
                LinkedList<string> prefsGames;
                prefsGames=BL.searchGameByPrefs(type, limit, buyIN, chipPolicy, minBet, minPlayers, MaxPlayers, spectatable, leaguable, this.user.league);
                if (prefsGames == null)
                {
                    prefsGames = new LinkedList<string>();
                    prefsGames.AddLast("none");
                }
                gameCenter GC = new gameCenter(BL, user, prefsGames);
                GC.Show();
                this.Close();
            }
            
        }
    }
}
