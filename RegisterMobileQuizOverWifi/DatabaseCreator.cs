using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

namespace RegisterMobileQuizOverWifi
{
    public class DatabaseCreator
    {
        public void initDatabase()
        {
            // create a new connection to our local database by using a connectionstring
            SqlConnection databaseConnection;
            SqlCommand command;
            string connectionString = @"Server = (localdb)\MSSQLLocalDB;";
            databaseConnection = new SqlConnection(connectionString);    

            try
            {
                // Attempt to create the database MobileQuizDB
                string sql = "CREATE DATABASE MobileQuizDB";
                databaseConnection.Open();
                command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();
                // DEV? Show messagebox confirming the creation of a database
                MessageBox.Show("DataBase is Created Successfully", "DatabaseCreation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception e)
            {
                // Exception for Database already exists
                Console.WriteLine(e.ToString());
                // MessageBox.Show(e.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                if (databaseConnection.State == ConnectionState.Open)
                {
                    databaseConnection.Close();
                }
            }

            // Attempt to add our tables to the created database if they don't already exist
            try
            {
                // connect to the new database and create our tables if they don't exist
                connectionString = @"Server = (localdb)\MSSQLLocalDB";
                databaseConnection = new SqlConnection(connectionString);
                databaseConnection.Open();
                // Create our sql string and pass it through to the database as a query
                // TODO this is where we could add our sql for the initial creation of all of our tables.
                string sql =    "CREATE TABLE LOGIN (USER_NAME VARCHAR(50) NOT NULL PRIMARY KEY," +
                                "PASSWORD VARCHAR(50) NOT NULL, ISADMIN BIT NOT NULL)";
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
                if (databaseConnection.State == ConnectionState.Open)
                {
                    databaseConnection.Close();
                }
            }

            databaseConnection.Close();
        }
        public string readDB()
        {
            // create a datareader object to be able to display any data we may need from the database.
            SqlDataReader dataReader;
            SqlCommand command;
            SqlConnection databaseConnection;

            // Create an output string to display our data with
            string output = "This is the current data in LOGIN table\n\nUsername - Password - IsAdmin\n";
            // Connect to database using data connection TODO Make all of these database connection opennings to a method
            string connectionString = @"Server = (localdb)\MSSQLLocalDB";
            databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            // DEV Create our sql query string for displaying the LOGIN table
            string sql = "SELECT USER_NAME,PASSWORD,ISADMIN from LOGIN";

            command = new SqlCommand(sql, databaseConnection);
            dataReader = command.ExecuteReader();
            // Read through the return values of the sql query, while results still exist add them to our output string
            while (dataReader.Read())
            {
                output = output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + "\n";
            }

            databaseConnection.Close();
            return output;
        }
        public string addUserDB(string sql, string username, string password)
        {
            // TODO check for vaild entry ie: no spaces in username/password, not too long or short etc...
            SqlDataReader dataReader;
            SqlCommand command;
            bool alreadyExists = false;
            // First check if username already exists.
            // Connect to database using data connection
            SqlConnection databaseConnection;
            string sqlSelect = "SELECT USER_NAME from LOGIN";
            string connectionString = @"Server = (localdb)\MSSQLLocalDB;";

            databaseConnection = new SqlConnection(connectionString);            
            databaseConnection.Open();
            command = new SqlCommand(sqlSelect, databaseConnection);            
            dataReader = command.ExecuteReader();
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
                SqlDataAdapter adapter = new SqlDataAdapter();
                databaseConnection.Open();

                command = new SqlCommand(sql, databaseConnection);
                adapter.InsertCommand = new SqlCommand(sql, databaseConnection);
                adapter.InsertCommand.ExecuteNonQuery();

                command.Dispose();
                databaseConnection.Close();
                readDB();
            }
            return null;
        }
        public bool loginDB(string username, string password)
        {
            // connect to our database
            SqlConnection databaseConnection;
            string connectionString = @"Server = (localdb)\MSSQLLocalDB;";
            databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();

            // build are sql command
            string sql = "SELECT user_name,password FROM login WHERE user_name = '" + username + "'";
            SqlCommand command = new SqlCommand(sql, databaseConnection);

            // read the results
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                // check if password matches saved password
                if (dataReader.GetValue(1).ToString() == password)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
