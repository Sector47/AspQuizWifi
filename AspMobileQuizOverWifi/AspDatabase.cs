using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security;

namespace AspMobileQuizOverWifi
{
    public class AspDatabase
    {

        public string readDB()
        {
            SqlConnection databaseConnection;
            string sql, output = "";
            string connectionString = @"Server = (localdb)\MSSQLLocalDB;Initial Catalog=MobileQuizDB";
            databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            // Check if database exists
            try
            {
                SqlCommand command;
                SqlDataReader dataReader;
                sql = "SELECT USER_NAME,PASSWORD,ISADMIN from LOGIN";
                command = new SqlCommand(sql, databaseConnection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    output = output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + "\n";
                }
                databaseConnection.Close();
            }
            catch (System.Exception e)
            {
                //MessageBox.Show(e.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                if (databaseConnection.State == ConnectionState.Open)
                {
                    databaseConnection.Close();
                }
            }
            return output;
        }

        // loginDB will return true if logged in successfully and false if not.
        // TODO
        // Additionally loginDB will call a method or do it self 
        //     create a temporary userSessionID that will be used to continue with current user without having to log in. 
        //     This sessionid should be part of the DB until the user logs out, the server first starts up, or a specified time has passed since they have last accessed a page with the session id
        // The sessionID will be passed through to each page, or stored as a cookie <-- More correct but i'm unsure how to do this atm
        public bool logigfnDB(string username, string password)
        {
            // connect to our database
            SqlConnection databaseConnection;
            string connectionString = @"Server = (localdb)\MSSQLLocalDB;Initial Catalog=MobileQuizDB;";
            var s = new SecureString();
            s.AppendChar('A');
            s.AppendChar('s');
            s.AppendChar('p');
            s.AppendChar('S');
            s.AppendChar('e');
            s.AppendChar('r');
            s.AppendChar('v');
            s.AppendChar('e');
            s.AppendChar('r');
            s.AppendChar('P');
            s.AppendChar('a');
            s.AppendChar('s');
            s.AppendChar('s');
            s.MakeReadOnly();
            SqlCredential sqlCredential = new SqlCredential("AspMobileServer", s);
            databaseConnection = new SqlConnection(connectionString, sqlCredential);
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
