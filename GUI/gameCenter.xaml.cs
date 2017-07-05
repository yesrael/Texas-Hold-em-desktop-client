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
using System.Data;

namespace GUI
{
    /// <summary>
    /// Interaction logic for gameCenter.xaml
    /// </summary>
    public partial class gameCenter : Window
    {
        businessLayer BL;
        User user;

        public gameCenter(businessLayer bl, User user, LinkedList<string> prefsGames)
        {
            InitializeComponent();
            this.BL = bl;
            this.user = user;
            this.userName.Content = this.user.UserName;
            showAvatar();
            if (prefsGames != null)
            showSearchResults(prefsGames);
        }

        private void showSearchResults(LinkedList<string> can_join)
        {
            games_table.ItemsSource = null;
            games_table.Visibility = System.Windows.Visibility.Visible;

            DataTable dt = new DataTable();

            //create table dynamically
            dt.Columns.Add("game id", typeof(string));
            dt.Columns.Add("buy in", typeof(string));
            dt.Columns.Add("avilable sets", typeof(string));

            if (can_join.Contains("none"))
            {
                games_table.Visibility = System.Windows.Visibility.Hidden;
                MessageBox.Show("Can't find any games");
            }
            else
            {
                string[] oneRow = new string[3];
                int index = 0;
                //add rows
                foreach (string i_game in can_join)
                {
                    oneRow[index] = i_game;
                    index++;
                    if (index > 2)
                    {
                        index = 0;
                        dt.Rows.Add(oneRow[0], oneRow[1], oneRow[2]);
                    }
                }
                games_table.ItemsSource = dt.DefaultView;
            }
        }

        private void showAvatar()
        {
                if (this.user.avatar.Equals("avatar1"))
                userAvatar.Background = new ImageBrush(new BitmapImage(new Uri("avatar1.jpg", UriKind.Relative)));
                if (this.user.avatar.Equals("avatar2"))
                userAvatar.Background = new ImageBrush(new BitmapImage(new Uri("avatar2.png", UriKind.Relative)));
                if (this.user.avatar.Equals("avatar3"))
                userAvatar.Background = new ImageBrush(new BitmapImage(new Uri("avatar3.jpg", UriKind.Relative)));
                if (this.user.avatar.Equals("avatar4"))
                userAvatar.Background = new ImageBrush(new BitmapImage(new Uri("avatar4.jpg", UriKind.Relative)));
        }


        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            BL.logout(user.ID);
            MainWindow MW = new MainWindow(BL);
            MW.Show();
            this.Close();
        }

        private void edit_profile_button_Click(object sender, RoutedEventArgs e)
        {
            comboBox.Visibility = System.Windows.Visibility.Visible;
            change_box.Visibility = System.Windows.Visibility.Visible;
            cancel_button.Visibility = System.Windows.Visibility.Visible;
            change_button.Visibility = System.Windows.Visibility.Visible;
            games_table.Visibility = System.Windows.Visibility.Hidden;
            change_box.Clear();
            comboBox.SelectedValue = null;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (change_pass_selected.IsSelected)
            {
                change_password_box.Visibility = System.Windows.Visibility.Visible;
                change_box.Text = "enter your old password";
                change_password_box.Text = "enter new password";
            }
            if (change_name_selected.IsSelected)
            {
                change_password_box.Visibility = System.Windows.Visibility.Hidden;
                change_box.Text = "enter new name";
            }
            if (change_email_selected.IsSelected)
            {
                change_password_box.Visibility = System.Windows.Visibility.Hidden;
                change_box.Text = "enter new email";
            }
            if (change_avatar_selected.IsSelected)
            {
                comboBox.Visibility = System.Windows.Visibility.Hidden;
                change_box.Visibility = System.Windows.Visibility.Hidden;
                change_password_box.Visibility = System.Windows.Visibility.Hidden;
                canvas1.Visibility = System.Windows.Visibility.Visible;
                canvas2.Visibility = System.Windows.Visibility.Visible;
                canvas3.Visibility = System.Windows.Visibility.Visible;
                canvas4.Visibility = System.Windows.Visibility.Visible;
                avatar1.Visibility = System.Windows.Visibility.Visible;
                avatar2.Visibility = System.Windows.Visibility.Visible;
                avatar3.Visibility = System.Windows.Visibility.Visible;
                avatar4.Visibility = System.Windows.Visibility.Visible;
                avatar_border.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            comboBox.Visibility = System.Windows.Visibility.Hidden;
            change_box.Visibility = System.Windows.Visibility.Hidden;
            change_password_box.Visibility = System.Windows.Visibility.Hidden;
            cancel_button.Visibility = System.Windows.Visibility.Hidden;
            change_button.Visibility = System.Windows.Visibility.Hidden;
            canvas1.Visibility = System.Windows.Visibility.Hidden;
            canvas2.Visibility = System.Windows.Visibility.Hidden;
            canvas3.Visibility = System.Windows.Visibility.Hidden;
            canvas4.Visibility = System.Windows.Visibility.Hidden;
            avatar1.Visibility = System.Windows.Visibility.Hidden;
            avatar2.Visibility = System.Windows.Visibility.Hidden;
            avatar3.Visibility = System.Windows.Visibility.Hidden;
            avatar4.Visibility = System.Windows.Visibility.Hidden;
            avatar_border.Visibility = System.Windows.Visibility.Hidden;
        }

        private void change_button_Click(object sender, RoutedEventArgs e)
        {
            if (change_pass_selected.IsSelected || change_name_selected.IsSelected || change_email_selected.IsSelected || change_avatar_selected.IsSelected)
            {
                comboBox.Visibility = System.Windows.Visibility.Hidden;
                change_box.Visibility = System.Windows.Visibility.Hidden;
                change_password_box.Visibility = System.Windows.Visibility.Hidden;
                cancel_button.Visibility = System.Windows.Visibility.Hidden;
                change_button.Visibility = System.Windows.Visibility.Hidden;
                canvas1.Visibility = System.Windows.Visibility.Hidden;
                canvas2.Visibility = System.Windows.Visibility.Hidden;
                canvas3.Visibility = System.Windows.Visibility.Hidden;
                canvas4.Visibility = System.Windows.Visibility.Hidden;
                avatar1.Visibility = System.Windows.Visibility.Hidden;
                avatar2.Visibility = System.Windows.Visibility.Hidden;
                avatar3.Visibility = System.Windows.Visibility.Hidden;
                avatar4.Visibility = System.Windows.Visibility.Hidden;
                avatar_border.Visibility = System.Windows.Visibility.Hidden;

                bool result=false;
                if (change_pass_selected.IsSelected)
                    result = BL.editUserPassword(this.user.ID, change_box.Text, change_password_box.Text);
                else if (change_name_selected.IsSelected)
                    result = BL.editUserName(this.user.ID, change_box.Text);
                else if (change_email_selected.IsSelected)
                    result = BL.editUserEmail(this.user.ID, change_box.Text);
                else //change avatar is selected
                {
                    string avatar = "";

                    if (avatar1.IsChecked == true)
                        avatar = "avatar1";
                    if (avatar2.IsChecked == true)
                        avatar = "avatar2";
                    if (avatar3.IsChecked == true)
                        avatar = "avatar3";
                    if (avatar4.IsChecked == true)
                        avatar = "avatar4";
                    result = BL.editUserAvatar(this.user.ID, avatar);
                }

                if (!result)
                    MessageBox.Show("an error was occurred, \n please enter valid parameters");
                else
                {
                    MessageBox.Show("success! \n the profile was changed");
                    this.user = BL.getUser(user.ID);
                    this.userName.Content = this.user.UserName;
                    showAvatar();
                }
               
            }
        }

        private void search_games_button_Click(object sender, RoutedEventArgs e)
        {
            ok_button.Visibility = System.Windows.Visibility.Visible;
            cancel_game_button.Visibility = System.Windows.Visibility.Visible;
            serachGame_comboBox.Visibility = System.Windows.Visibility.Visible;
            game_box.Visibility = System.Windows.Visibility.Hidden;
            game_label.Visibility = System.Windows.Visibility.Hidden;
            spectate_button.Visibility = System.Windows.Visibility.Hidden;
            replay_button.Visibility = System.Windows.Visibility.Hidden;
            games_table.Visibility = System.Windows.Visibility.Hidden;

            game_label.Content = null;
        //    game_label.Content = "please enter pot size:";
            ok_button.Content = "search";
        }

        private void join_game_button_Click(object sender, RoutedEventArgs e)
        {
            game_box.Visibility = System.Windows.Visibility.Visible;
            ok_button.Visibility = System.Windows.Visibility.Visible;
            cancel_game_button.Visibility = System.Windows.Visibility.Visible;
            game_label.Visibility = System.Windows.Visibility.Visible;
            spectate_button.Visibility = System.Windows.Visibility.Visible;
            serachGame_comboBox.Visibility = System.Windows.Visibility.Hidden;
            replay_button.Visibility = System.Windows.Visibility.Hidden;
            games_table.Visibility = System.Windows.Visibility.Hidden;

            game_label.Content = "please enter game id:";
            ok_button.Content = "join";
        }

        private void create_game_button_Click(object sender, RoutedEventArgs e)
        {
            create_game CG = new create_game(BL, user, true);
            CG.Show();
            this.Close();
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            if (ok_button.Content.Equals("search"))
            {
                games_table.ItemsSource = null;
                games_table.Visibility = System.Windows.Visibility.Visible;

                LinkedList<string> can_join = new LinkedList<string>();
                DataTable dt = new DataTable();

                    //create table dynamically
                    dt.Columns.Add("game id", typeof(string));
                    dt.Columns.Add("buy in", typeof(string));
                    dt.Columns.Add("avilable sets", typeof(string));


                if (search_by_pot_size.IsSelected)
                {
                    int pot_size;
                    pot_size = int.Parse(game_box.Text);
                    can_join = BL.searchGamesByPotSize(pot_size);
                }
                else if (search_by_player_name.IsSelected)
                {
                    can_join = BL.searchGamesByPlayerName(game_box.Text);
                }
                else if (search_by_prefs.IsSelected)
                {
                    can_join = null;
                }
                else if (joinable_list.IsSelected)
                    can_join = BL.listOfJoinableGames(this.user.ID);
                else if (spectatable_list.IsSelected)
                    can_join = BL.listOfSpectatableGames();

                if (can_join == null)
                        MessageBox.Show("Can't find any games");
                    else
                    {
                    string[] oneRow = new string[3];
                    int index = 0;
                        //add rows
                        foreach (string i_game in can_join)
                        {
                          oneRow[index] = i_game;
                          index++;
                          if (index > 2)
                          {
                            index = 0;
                            dt.Rows.Add(oneRow[0], oneRow[1], oneRow[2]);
                          }
                    }
                    }

                games_table.ItemsSource = dt.DefaultView;
            }
            else if (ok_button.Content.Equals("join"))
            {
                bool canJoin= BL.joinGame(game_box.Text, this.user.ID);
                BL.game choosenGame = BL.getGameByID(game_box.Text);
                if (choosenGame != null && canJoin)
                {
                    game g = new game(BL, choosenGame, user);
                    g.Show();
                    this.Close();
                }
                else MessageBox.Show("Can't join this game");
            }
        }

        private void cancel_game_button_Click(object sender, RoutedEventArgs e)
        {
            games_table.Visibility = System.Windows.Visibility.Hidden;
            game_box.Visibility = System.Windows.Visibility.Hidden;
            ok_button.Visibility = System.Windows.Visibility.Hidden;
            cancel_game_button.Visibility = System.Windows.Visibility.Hidden;
            game_label.Visibility = System.Windows.Visibility.Hidden;
            serachGame_comboBox.Visibility = System.Windows.Visibility.Hidden;
            spectate_button.Visibility = System.Windows.Visibility.Hidden;
            replay_button.Visibility = System.Windows.Visibility.Hidden;
        }

        private void serachGame_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (search_by_pot_size.IsSelected)
            {
                serachGame_comboBox.Visibility = System.Windows.Visibility.Hidden;
                game_box.Visibility = System.Windows.Visibility.Visible;
                game_label.Visibility = System.Windows.Visibility.Visible;
                game_label.Content = "please enter pot size:";
            }
            else if(search_by_player_name.IsSelected)
            {
                serachGame_comboBox.Visibility = System.Windows.Visibility.Hidden;
                game_box.Visibility = System.Windows.Visibility.Visible;
                game_label.Visibility = System.Windows.Visibility.Visible;
                game_label.Content = "please enter name:";
            }
            else if (search_by_prefs.IsSelected)
            {
                create_game CG = new create_game(BL, user, false);
                CG.Show();
                this.Close();
            }
            else
            {
                game_box.Visibility = System.Windows.Visibility.Hidden;
                game_label.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void spectate_button_Click(object sender, RoutedEventArgs e)
        {
            Boolean isSpectable = BL.spectateGame(this.user.ID, game_box.Text);
               BL.game choosenGame = BL.getGameByID(game_box.Text);
            if (choosenGame != null && isSpectable)
            {
                game g = new game(BL, choosenGame, user);
                g.Show();

                this.Close();
            }
            else MessageBox.Show("Can't join this game");

        }

        private void replay_game_button_Click(object sender, RoutedEventArgs e)
        {
            replay_button.Visibility = System.Windows.Visibility.Visible;
            game_box.Visibility = System.Windows.Visibility.Visible;
            cancel_game_button.Visibility = System.Windows.Visibility.Visible;
            game_label.Visibility = System.Windows.Visibility.Visible;
            spectate_button.Visibility = System.Windows.Visibility.Hidden;
            serachGame_comboBox.Visibility = System.Windows.Visibility.Hidden;
            ok_button.Visibility = System.Windows.Visibility.Hidden;
            games_table.Visibility = System.Windows.Visibility.Hidden;

            game_label.Content = "please enter game id:";
        }

        private void replay_button_Click(object sender, RoutedEventArgs e)
        {
            String gameDescription = BL.gameReplay(game_box.Text);
            if (gameDescription != null)
            {
                replayedGame RG = new replayedGame(BL, this.user, gameDescription);
                RG.Show();
                this.Close();
            }
            else MessageBox.Show("Can't replay this game");
        }
    }
}
