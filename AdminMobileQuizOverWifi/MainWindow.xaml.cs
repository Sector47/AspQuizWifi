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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace AdminMobileQuizOverWifi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Create an instance of our databasecreator class
            DatabaseCreator creator = new DatabaseCreator();
            // call the init database method to create the database if it doesn't already exist     
            creator.initDatabase();
        }

        // pass username and password through as a sql query to the database
        // TODO add method and call method before query to verify username password are valid.
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // create our sql query string using the text from the textboxes.
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Text;
            string sql = "";
            // create our sql based on whether admin checkbox is checked
            if (AdminCheckbox.IsChecked.HasValue && AdminCheckbox.IsChecked.Value)
            {
                sql = "INSERT INTO USERS (USERNAME, PASSWORD, IS_INSTRUCTOR) VALUES('" + username + "', '" + password + "', 1)";
            }
            else
            {
                sql = "INSERT INTO USERS (USERNAME, PASSWORD, IS_INSTRUCTOR) VALUES('" + username + "', '" + password + "', 0)";
            }




            // Create an instance of our databasecreator class
            DatabaseCreator creator = new DatabaseCreator();
            // DEV show messsage box with before Select * from LOGIN
            MessageBox.Show("Before \n" + creator.readDB());
            // call the addUserDB method to have the DatabaseCreator class deal with adding our new user to the database
            creator.addUserDB(sql, username, password);
            // DEV show message box with after Select * from LOGIN
            MessageBox.Show("After \n" + creator.readDB());
        }

        // On focus, empty the text box and remove the gotfocus handler
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = "";
            textBox.GotFocus -= TextBox_GotFocus;
        }
    }
}
