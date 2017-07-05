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
    /// Interaction logic for signUp.xaml
    /// </summary>
    public partial class signUp : Window
    {
        businessLayer BL;

        public signUp(businessLayer bl)
        {
            InitializeComponent();
            this.BL = bl;
        }

        private void confirm_button_Click(object sender, RoutedEventArgs e)
        {
            string avatar="";
            if (avatar1.IsChecked == true)
                avatar = "avatar1";
            if (avatar2.IsChecked == true)
                avatar = "avatar2";
            if (avatar3.IsChecked == true)
                avatar = "avatar3";
            if (avatar4.IsChecked == true)
                avatar = "avatar4";

            bool function_succeed;
            function_succeed = BL.register(id_textBox.Text, pass_textBox.Text, name_textBox.Text, email_textBox.Text, avatar);
            if (!function_succeed)
            {
                MessageBox.Show("Registeration Failed, try to change the ID or to write stronger password", "Registeration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
            {
                MainWindow MW = new MainWindow(BL);
                MW.Show();
                this.Close();
            }
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void home_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new MainWindow(BL);
            MW.Show();
            this.Close();
        }
    }
}
