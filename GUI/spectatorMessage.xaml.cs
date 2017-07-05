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
    /// Interaction logic for spectatorMessage.xaml
    /// </summary>
    public partial class spectatorMessage : Window
    {

        businessLayer BL;
        String ReceieverID, ReceiverName, gameID, myID;
        LinkedList<User> spectators;
        ComboBoxItem [] items;

        public spectatorMessage(businessLayer bl, String gameID, String myID, LinkedList<User> spectatorss)
        {
            InitializeComponent();
            this.BL = bl;
            this.gameID = gameID;
            this.myID = myID;
            this.spectators = new LinkedList<User>(spectatorss);
           // this.spectators = spectators;
            items = new ComboBoxItem[spectators.Count];
            initializeComboBox();
        }

        private void initializeComboBox()
        {
            int i = 0;
            foreach(User spec in spectators)
            {
                    items[i] = new ComboBoxItem();
                    items[i].Content = spec.ID + ": " + spec.UserName;
                    comboBox.Items.Add(items[i]);
                i++;
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (User spec in spectators)
            {
                if(items[i].IsSelected)
                {
                    ReceieverID = spec.ID;
                    ReceiverName = spec.UserName;
                    break;
                }
                i++;
            }
            String message = messageBox.Text + "\n";
            this.BL.sendWhisper(message, gameID, myID, ReceieverID);
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
