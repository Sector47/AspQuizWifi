using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;


namespace AdminMobileQuizOverWifi
{
    public class DatabaseCreator
    {
        // String location of the database locally
        // DEVCONFIG
        private string dbLocationLocal = ""; // For local replace "" with  "Data Source = C:\\Users\\       USERNAME HERE           \\AppData\\Roaming\\MobileQuizOverWifi\\MobileQuiz.db";

        // Below code will be used if we use a config file
        // "Data Source = " + System.IO.Path.Combine(
        //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //@"MobileQuizOverWifi\MobileQuiz.db");
        // TODO instead of making the dblocationlocal on startup each time, we make it once add it to a config file and then read the config file. This allows asp.net to get the user appdata folder as it is logged in as system/applicationpool and not the user.
        // DEV "Data Source = C:\\Users\\      Username goes here    \\AppData\\Roaming\\MobileQuizOverWifi\\MobileQuiz.db";

        // String location of the database server
        // DEVCONFIG
        private string dbLocationNetwork = "Server = bitweb3.nwtc.edu; Database = dbdev26; User Id = dbdev26; Password = 123456;";// For local db make this empty "" and make dbLocationLocal not with your username

        public void initDatabase()
        {
            // TODO check for a SQL Server at this location, if not use sqllite
            SqlConnection databaseConnection;
            SQLiteConnection sqliteConnection;
            sqliteConnection = new SQLiteConnection(dbLocationLocal);
            SqlCommand command;
            try
            {
                databaseConnection = new SqlConnection(dbLocationNetwork);
                // Check for existing database dbdev26
                string sql = "Select * from information_schema.tables";
                databaseConnection.Open();
                command = new SqlCommand(sql, databaseConnection);

                // read the results
                SqlDataReader dataReader = command.ExecuteReader();

                int count = 0;
                while (dataReader.Read())
                {
                    //count number of tables
                    count++;
                    // check if tables already exist, right now we assume they are correct tables
                    // TODO check for the tables we expect
                    string s = dataReader.GetValue(2).ToString();

                    // DEV output to console each table name
                    System.Diagnostics.Debug.WriteLine(s);
                }
                // if able to connect to database without issues we can start modifiying it.
                sql = "CREATE TABLE Logins (User_Name VARCHAR(50) NOT NULL PRIMARY KEY," +
                                "Password VARCHAR(50) NOT NULL, isAdmin BIT NOT NULL," +
                                "sessionid CHAR(24))";

                System.Diagnostics.Debug.WriteLine("database exists");
                databaseConnection.Close();
            }
            catch (System.Exception e)
            {
                // if database can't connect we use sqllite instead
                try
                {
                    //Create Directory at CurrentUser\AppData\Roaming\MobileQuizOverWifi  if it doesn't already exist
                    string dbDirectory = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"MobileQuizOverWifi");
                    if (!Directory.Exists(dbDirectory))
                    {
                        Directory.CreateDirectory(dbDirectory);
                    }

                    sqliteConnection.Open();
                    Console.WriteLine(sqliteConnection.ConnectionString);
                    var SqLitePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"RegisterMobileQuizOverWifi\MobileQuizDb.db"));
                    Console.WriteLine(SqLitePath);
                    string sql = "CREATE TABLE Logins (User_Name VARCHAR(50) NOT NULL PRIMARY KEY," +
                                "Password VARCHAR(50) NOT NULL, isAdmin BIT NOT NULL," +
                                "sessionid CHAR(24))";
                    SQLiteCommand liteCmd = sqliteConnection.CreateCommand();
                    liteCmd.CommandText = sql;
                    liteCmd.ExecuteNonQuery();

                }
                catch (System.Exception sqlE)
                {
                    // this just makes sure we don't crash when database already exists
                    // TODO catch specific exceptions
                }


                // TODO update config file as well
            }
            finally
            {
                try
                {
                    databaseConnection = new SqlConnection(dbLocationNetwork);
                    if (databaseConnection.State == ConnectionState.Open)
                    {
                        databaseConnection.Close();
                    }
                }
                catch (System.Exception e)
                {

                }

                if (sqliteConnection.State == ConnectionState.Open)
                {
                    sqliteConnection.Close();
                }
            }

            // Attempt to add our tables to the created network database if they don't already exist
            try
            {
                // connect to the new database and create our tables if they don't exist
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                // Create our sql string and pass it through to the database as a query
                // TODO this is where we could add our sql for the initial creation of all of our tables.
                string sql = "CREATE TABLE Logins (User_Name VARCHAR(50) NOT NULL PRIMARY KEY," +
                                "Password VARCHAR(50) NOT NULL, isAdmin BIT NOT NULL," +
                                "sessionid CHAR(24))";
                command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();
                // DEV? Display confirmation message that tables were added succesfully
                MessageBox.Show("Tables Added Successfully", "DatabaseCreation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception e)
            {
                // Exception for Table already exists
                Console.WriteLine(e.ToString());
                // MessageBox.Show(e.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                try
                {
                    databaseConnection = new SqlConnection(dbLocationNetwork);
                    if (databaseConnection.State == ConnectionState.Open)
                    {
                        databaseConnection.Close();
                    }
                }
                catch (System.Exception e)
                {

                }
            }
        }
        public string readDB()
        {
            // Create an output string to display our data with
            string output = "This is the current data in LOGIN table\n\nUsername - Password - IsAdmin\n";
            // Connect to database using data connection TODO Make all of these database connection opennings to a method
            string sql = "SELECT User_Name, Password, isAdmin, sessionid FROM Logins";
            // DEV Create our sql query string for displaying the LOGIN table
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    output = output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + "\n";
                }
            }
            catch (System.Exception e)
            {
                // if unable to connect or tables don't exist it should be caught and gone through sqlite instead
                // TODO check config for bool. Possibly checkbox on local application
                // SQLite read of db
                SQLiteConnection sqliteConnection;
                sqliteConnection = new SQLiteConnection(dbLocationLocal);
                sqliteConnection.Open();

                SQLiteCommand liteCmd = sqliteConnection.CreateCommand();
                liteCmd.CommandText = sql;

                SQLiteDataReader liteCmdReader = liteCmd.ExecuteReader();
                while (liteCmdReader.Read())
                {
                    output = output + liteCmdReader.GetValue(0) + " - " + liteCmdReader.GetValue(1) + " - " + liteCmdReader.GetValue(2) + "\n";
                }
            }

            return output;
        }
        public string addUserDB(string sql, string username, string password)
        {
            // TODO check for vaild entry ie: no spaces in username/password, not too long or short etc...
            // First check if username already exists.
            string sqlSelect = "SELECT User_Name FROM Logins WHERE User_Name = '" + username + "'";
            bool alreadyExists = false;
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sqlSelect, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                // In order to check for existing usernames we use a datareader to loop through sql select statement results TODO turn this into a sql query, I already wrote this before i thought to use sql to ask for existing rows
                // If it already exists we return null, else we add it to the table
                while (dataReader.Read())
                {
                    if (dataReader.GetValue(0).ToString().ToLower() == username.ToLower())
                    {
                        alreadyExists = true;
                    }
                }

                if (alreadyExists)
                {
                    return null;
                }
                else
                {
                    databaseConnection.Close();
                    databaseConnection = new SqlConnection(dbLocationNetwork);
                    databaseConnection.Open();
                    command = new SqlCommand(sql, databaseConnection);
                    command.ExecuteNonQuery();
                    readDB();
                }
                return null;
            }
            catch (System.Exception e)
            {
                // if can't connect we go to sqlite
                // TODO make this catch only expected exceptions and instead check config file
                SQLiteConnection sqliteConnection;
                // Connect to database using data connection
                sqliteConnection = new SQLiteConnection(dbLocationLocal);
                sqliteConnection.Open();
                SQLiteCommand liteCmd = sqliteConnection.CreateCommand();
                liteCmd.CommandText = sqlSelect;
                SQLiteDataReader liteCmdReader = liteCmd.ExecuteReader();

                // In order to check for existing usernames we use a datareader to loop through sql select statement results TODO turn this into a sql query, I already wrote this before i thought to use sql to ask for existing rows
                // If it already exists we return null, else we add it to the table
                while (liteCmdReader.Read())
                {
                    if (liteCmdReader.GetValue(0).ToString().ToLower() == username.ToLower())
                    {
                        alreadyExists = true;
                    }
                }

                if (alreadyExists)
                {
                    return null;
                }
                else
                {
                    liteCmd = sqliteConnection.CreateCommand();
                    liteCmd.CommandText = sql;
                    liteCmd.ExecuteNonQuery();
                    readDB();
                }
                return null;
            }
        }
        public bool loginDB(string username, string password, string session)
        {
            string sql = "SELECT User_name, Password FROM Logins WHERE User_name = '" + username + "'";
            try
            {
                // attempt to connect to our database
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();

                // build our sql command                
                SqlCommand command = new SqlCommand(sql, databaseConnection);

                // read the results
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    // check if password matches saved password
                    if (dataReader.GetValue(1).ToString() == password)
                    {
                        sql = "UPDATE Logins " +
                              "SET sessionid = '" + session + "'" +
                              "WHERE User_name = '" + username + "';";

                        databaseConnection.Close();
                        databaseConnection = new SqlConnection(dbLocationNetwork);
                        databaseConnection.Open();
                        command = new SqlCommand(sql, databaseConnection);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                // if can't connect we go to sqlite
                // TODO make this catch only expected exceptions and instead check config file
                SQLiteConnection sqliteConnection;
                // Connect to database using data connection
                sqliteConnection = new SQLiteConnection(dbLocationLocal);
                sqliteConnection.Open();
                SQLiteCommand liteCmd = sqliteConnection.CreateCommand();
                liteCmd.CommandText = sql;
                SQLiteDataReader liteCmdReader = liteCmd.ExecuteReader();

                // In order to check for existing usernames we use a datareader to loop through sql select statement results TODO turn this into a sql query, I already wrote this before i thought to use sql to ask for existing rows
                // If it already exists we return null, else we add it to the table
                while (liteCmdReader.Read())
                {
                    // check if password matches saved password
                    if (liteCmdReader.GetValue(1).ToString() == password)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
