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
            string fName = FNameTextBox.Text;
            string lName = LNameTextBox.Text;
            bool isInstructor = false;
            // check if the isinstructor box is checked.
            if (InstructorStatusCheckbox.IsChecked.HasValue && InstructorStatusCheckbox.IsChecked.Value)
            {
                isInstructor = true;
            }
            // Create an instance of our databasecreator class
            DatabaseCreator databaseCreator = new DatabaseCreator();
            MessageBox.Show(databaseCreator.addUserDB(username, password, fName, lName, isInstructor));
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
