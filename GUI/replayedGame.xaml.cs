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
    /// Interaction logic for replayedGame.xaml
    /// </summary>
    public partial class replayedGame : Window
    {
        businessLayer BL;
        User user;

        public replayedGame(businessLayer bl, User user, String gameDescription)
        {
            InitializeComponent();
            this.BL = bl;
            this.user = user;
            writeGameInBox(gameDescription);
        }

        private void writeGameInBox(String game)
        {
            textBox.Text += "********************************************************************\n";
          string[] parts = game.Split('&');
            for (int i = 0; i < parts.Length; i++)
                textBox.Text += parts[i]+"\n";
            textBox.Text += "********************************************************************";
        }

        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            gameCenter GC = new gameCenter(BL, user, null);
            GC.Show();
            this.Close();
        }
    }
}
