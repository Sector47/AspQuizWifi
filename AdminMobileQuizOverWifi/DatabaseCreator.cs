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
    /// <summary>
    /// The database creator class holds the methods used to directly connect and work with our database. 
    /// This was used as I didn't know how to work with entity framework. Having seen the entity framework code used by others, I should have just learned how to use it. -JD
    /// </summary>
    public class DatabaseCreator
    {
        // String location of the database server
        private string dbLocationNetwork = "Server = bitweb3.nwtc.edu; Database = dbdev26; User Id = dbdev26; Password = 123456;";// For local db make this empty "" and make dbLocationLocal not with your username

        /// <summary>
        /// This method will create our initial database using the generated script from sql server management software if it can't find any tables on the connected database(SELECT * FROM information_schema.tables)
        /// </summary>
        public void initDatabase()
        {
            // TODO check for a SQL Server at this location, if not use sqllite
            SqlConnection databaseConnection;
            int count = 0;

            SqlCommand command;
            try
            {

                databaseConnection = new SqlConnection(dbLocationNetwork);
                // Check for existing database dbdev26
                string sql = "SELECT * FROM information_schema.tables";
                databaseConnection.Open();
                command = new SqlCommand(sql, databaseConnection);

                // read the results
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    count++;

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
                // Don't crash
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

            // Attempt to add our tables to the created network database if they don't already exist
            try
            {
                // connect to the new database and create our tables if they don't exist
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                // Create our sql string and pass it through to the database as a query
                // TODO this is where we could add our sql for the initial creation of all of our tables.
                string sql = "USE [dbdev26] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[ANSWER]( 	[QUE_ID] [int] NOT NULL, 	[DESCRIPTION] [varchar](max) NOT NULL, 	[CORRECT_ANS] [bit] NOT NULL, 	[ANS_ID] [int] IDENTITY(1,1) NOT NULL, PRIMARY KEY CLUSTERED  ( 	[ANS_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[COURSE]( 	[COU_NAME] [varchar](45) NOT NULL, 	[COU_SEM] [varchar](45) NULL, 	[COU_YEAR] [int] NULL, 	[COU_START_DATE] [date] NULL, 	[COU_END_DATE] [date] NULL, 	[COURSE_ID] [int] IDENTITY(1,1) NOT NULL, PRIMARY KEY CLUSTERED  ( 	[COURSE_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[COURSE_QUIZ]( 	[QUI_ID] [int] NOT NULL, 	[COURSE_ID] [int] NOT NULL, 	[COURSE_QUI_ID] [int] IDENTITY(1,1) NOT NULL, PRIMARY KEY CLUSTERED  ( 	[COURSE_QUI_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],  CONSTRAINT [UNIQUE_COURSE_QUIZ] UNIQUE NONCLUSTERED  ( 	[COURSE_ID] ASC, 	[QUI_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[GRADE]( 	[GRA_ID] [int] IDENTITY(1,1) NOT NULL, 	[USER_ID] [int] NOT NULL, 	[COURSE_QUI_ID] [int] NOT NULL, 	[GRA_GRADE] [int] NOT NULL, 	[GRA_NEEDSGRADING] [bit] NOT NULL,  CONSTRAINT [PK__GRADE__23772D18D6D82861] PRIMARY KEY CLUSTERED  ( 	[GRA_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[QUESTION]( 	[QUI_ID] [int] NOT NULL, 	[QUE_QUESTION] [varchar](max) NOT NULL, 	[TYPE_ID] [nvarchar](50) NOT NULL, 	[QUE_ID] [int] IDENTITY(1,1) NOT NULL, 	[QUESTION_ANSWER] [varchar](max) NULL, PRIMARY KEY CLUSTERED  ( 	[QUE_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[QUIZ]( 	[QUI_NAME] [varchar](45) NULL, 	[QUI_NOTES] [varchar](200) NULL, 	[QUI_ID] [int] IDENTITY(1,1) NOT NULL, PRIMARY KEY CLUSTERED  ( 	[QUI_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[RESPONSE]( 	[QUE_ID] [int] NOT NULL, 	[COMMENTS] [varchar](max) NULL, 	[USER_ID] [int] NOT NULL, 	[COURSE_QUI_ID] [int] NOT NULL, 	[RESPONSE_ID] [int] IDENTITY(1,1) NOT NULL, PRIMARY KEY CLUSTERED  ( 	[RESPONSE_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[ROSTER]( 	[USER_ID] [int] NOT NULL, 	[COURSE_ID] [int] NOT NULL, 	[ROSTER_ID] [int] IDENTITY(1,1) NOT NULL,  CONSTRAINT [PK__ROSTER__F3BEEBFFFF9E8AE6] PRIMARY KEY CLUSTERED  ( 	[ROSTER_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],  CONSTRAINT [UNIQUE_ROSTER] UNIQUE NONCLUSTERED  ( 	[USER_ID] ASC, 	[COURSE_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[TYPE]( 	[TYPE_ID] [nvarchar](50) NOT NULL, 	[TYPE_NAME] [nvarchar](200) NOT NULL,  CONSTRAINT [PK_TYPE] PRIMARY KEY CLUSTERED  ( 	[TYPE_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO  SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[USERS]( 	[F_NAME] [varchar](15) NULL, 	[L_NAME] [varchar](25) NULL, 	[IS_INSTRUCTOR] [bit] NOT NULL, 	[USERNAME] [varchar](25) NOT NULL, 	[PASSWORD] [varchar](25) NOT NULL, 	[SESSION_ID] [varchar](25) NULL, 	[USER_ID] [int] IDENTITY(8,1) NOT NULL,  CONSTRAINT [PK__USER__F3BEEBFF8742512C] PRIMARY KEY CLUSTERED  ( 	[USER_ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],  CONSTRAINT [unique_username] UNIQUE NONCLUSTERED  ( 	[USERNAME] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] GO ALTER TABLE [dbo].[ANSWER]  WITH CHECK ADD FOREIGN KEY([QUE_ID]) REFERENCES [dbo].[QUESTION] ([QUE_ID]) GO ALTER TABLE [dbo].[COURSE_QUIZ]  WITH CHECK ADD  CONSTRAINT [FK_COURSE_QUIZ_COURSE] FOREIGN KEY([COURSE_ID]) REFERENCES [dbo].[COURSE] ([COURSE_ID]) GO ALTER TABLE [dbo].[COURSE_QUIZ] CHECK CONSTRAINT [FK_COURSE_QUIZ_COURSE] GO ALTER TABLE [dbo].[COURSE_QUIZ]  WITH CHECK ADD  CONSTRAINT [FK_COURSE_QUIZ_QUIZ] FOREIGN KEY([QUI_ID]) REFERENCES [dbo].[QUIZ] ([QUI_ID]) GO ALTER TABLE [dbo].[COURSE_QUIZ] CHECK CONSTRAINT [FK_COURSE_QUIZ_QUIZ] GO ALTER TABLE [dbo].[GRADE]  WITH CHECK ADD FOREIGN KEY([COURSE_QUI_ID]) REFERENCES [dbo].[COURSE_QUIZ] ([COURSE_QUI_ID]) GO ALTER TABLE [dbo].[GRADE]  WITH CHECK ADD  CONSTRAINT [USER_ID] FOREIGN KEY([USER_ID]) REFERENCES [dbo].[USERS] ([USER_ID]) GO ALTER TABLE [dbo].[GRADE] CHECK CONSTRAINT [USER_ID] GO ALTER TABLE [dbo].[QUESTION]  WITH CHECK ADD  CONSTRAINT [FK_QUESTION_TYP] FOREIGN KEY([TYPE_ID]) REFERENCES [dbo].[TYPE] ([TYPE_ID]) GO ALTER TABLE [dbo].[QUESTION] CHECK CONSTRAINT [FK_QUESTION_TYP] GO ALTER TABLE [dbo].[QUESTION]  WITH CHECK ADD  CONSTRAINT [FK_QUI_QUE] FOREIGN KEY([QUI_ID]) REFERENCES [dbo].[QUIZ] ([QUI_ID]) GO ALTER TABLE [dbo].[QUESTION] CHECK CONSTRAINT [FK_QUI_QUE] GO ALTER TABLE [dbo].[RESPONSE]  WITH CHECK ADD FOREIGN KEY([COURSE_QUI_ID]) REFERENCES [dbo].[COURSE_QUIZ] ([COURSE_QUI_ID]) GO ALTER TABLE [dbo].[RESPONSE]  WITH CHECK ADD FOREIGN KEY([QUE_ID]) REFERENCES [dbo].[QUESTION] ([QUE_ID]) GO ALTER TABLE [dbo].[RESPONSE]  WITH CHECK ADD  CONSTRAINT [fk_user_Response] FOREIGN KEY([USER_ID]) REFERENCES [dbo].[USERS] ([USER_ID]) GO ALTER TABLE [dbo].[RESPONSE] CHECK CONSTRAINT [fk_user_Response] GO ALTER TABLE [dbo].[ROSTER]  WITH CHECK ADD  CONSTRAINT [FK__ROSTER__COURSE_I__4959E263] FOREIGN KEY([COURSE_ID]) REFERENCES [dbo].[COURSE] ([COURSE_ID]) GO ALTER TABLE [dbo].[ROSTER] CHECK CONSTRAINT [FK__ROSTER__COURSE_I__4959E263] GO ALTER TABLE [dbo].[ROSTER]  WITH CHECK ADD  CONSTRAINT [FK_ROSTER_USERS] FOREIGN KEY([USER_ID]) REFERENCES [dbo].[USERS] ([USER_ID]) GO ALTER TABLE [dbo].[ROSTER] CHECK CONSTRAINT [FK_ROSTER_USERS] GO ";
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

        /// <summary>
        /// Returns all of the users in the database, used to display confirmation that the user was added for the local app user creator
        /// </summary>
        /// <returns></returns>
        public string readDB()
        {
            // Create an output string to display our data with
            string output = "This is the current data in LOGIN table\n\nUsername - Password - Fname - Lname - IsAdmin\n";
            // Connect to database using data connection TODO Make all of these database connection opennings to a method
            string sql = "SELECT USERNAME, PASSWORD, F_NAME, L_NAME, IS_INSTRUCTOR, SESSION_ID FROM USERS";
            // DEV Create our sql query string for displaying the LOGIN table
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);
                while (dataReader.Read())
                {
                    output = output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + dataReader.GetValue(3) + " - " + dataReader.GetValue(4) + dataReader.GetValue(5) + "\n";
                }
                dataReader.Close();
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
            return output;
        }

        /// <summary>
        /// This method will take a username and password, f_name, l_name, isInstructor bool and add them to the users table. 
        /// </summary>
        /// <param name="username">The username to be entered</param>
        /// <param name="password">The password to b e entered</param>
        /// <param name="fName">fName to be entered</param>
        /// <param name="lName">lName to be enterd</param>
        /// <param name="isInstructor">A bool to flag whether a user being added is an instructor account.</param>
        /// <returns> A string value of any exceptions that occured</returns>
        public string addUserDB(string username, string password, string fName, string lName, bool isInstructor)
        {
            // TODO check for vaild entry ie: no spaces in username/password, not too long or short etc...
            if (username.Length > 25 || username.Contains(" "))
                return "Username must be shorter than 25 characters and contain no spaces";
            if (password.Length > 25 || password.Contains(" "))
                return "Password must be shorter than 25 characters and contain no spaces";
            if (fName.Length > 15 || fName.Contains(" "))
                return "First name must be shorter than 15 characters and contain no spaces";
            if (lName.Length > 15 || lName.Contains(" "))
                return "Last name must be shorter than 25 characters and contain no spaces";

            // Check if username already exists.
            string sqlSelect = "SELECT USERNAME FROM USERS WHERE USERNAME = '" + username + "'";
            bool alreadyExists = false;
            try
            {
                SqlDataReader dataReader = GetDataReader(sqlSelect);
                // In order to check for existing usernames we use a datareader to loop through sql select statement results TODO turn this into a sql query, I already wrote this before i thought to use sql to ask for existing rows
                // If it already exists we return null, else we add it to the table
                while (dataReader.Read())
                {
                    if (dataReader.GetValue(0).ToString().ToLower() == username.ToLower())
                    {
                        alreadyExists = true;
                    }
                }
                dataReader.Close();
                if (alreadyExists)
                {
                    return "A user with that name already exits.";
                }
                // build our sql command
                string sql = "INSERT INTO USERS (USERNAME, PASSWORD, F_NAME, L_NAME, IS_INSTRUCTOR) VALUES ('" + username + "','" + password + "','" + fName + "','" + lName + "','" + isInstructor + "')";

                // insert into our tables
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();
                databaseConnection.Close();
                return "The User: " + username + " was successfully added to the database";

            }
            catch (System.Exception e)
            {
                return e.ToString(); ;
            }
        }

        /// <summary>
        /// Logs in the given username if the password matches the stored password
        /// </summary>
        /// <param name="username">The username attempting to login</param>
        /// <param name="password">The password of the user attempting to login</param>
        /// <param name="session">The sessionid of the browser attempting to login. This will be stored in the db to know if a user is already logged in.</param>
        /// <returns></returns>
        public bool loginDB(string username, string password, string session)
        {
            string sql = "SELECT USERNAME, PASSWORD FROM USERS WHERE USERNAME = '" + username + "'";
            try
            {
                // read the results
                SqlDataReader dataReader = GetDataReader(sql);

                while (dataReader.Read())
                {
                    // First we check if password matches saved password
                    if (dataReader.GetValue(1).ToString() == password)
                    {
                        // Since password matched, we can update their loginstatus
                        sql = "UPDATE USERS " +
                              "SET SESSION_ID = '" + session + "'" +
                              "WHERE USERNAME = '" + username + "';";

                        SqlConnection databaseConnection = new SqlConnection(dbLocationNetwork);
                        databaseConnection.Open();
                        SqlCommand command = new SqlCommand(sql, databaseConnection);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Will check if the user_id password matches the one stored. 
        /// </summary>
        /// <param name="user_ID"> The id of the user that we are checking if the password matches for</param>
        /// <param name="currentPassword"> The password we are checking against the stored value</param>
        /// <returns>A boolean that verifys whether the password matched or not</returns>
        public bool CheckPasswordDB(int user_ID ,string currentPassword)
        {
            string sql = "SELECT PASSWORD FROM USERS WHERE USER_ID = '" + user_ID + "';";
            try
            {
                // read the results
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                if (command.ExecuteScalar().ToString() == currentPassword)
                {
                    databaseConnection.Close();
                    return true;
                }
                    

                return false;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// This method updates the Users table for a given user with a new password
        /// </summary>
        /// <param name="username">The username to update the password for</param>
        /// <param name="NewPassword"> The new password to update their password with</param>
        public void UpdatePasswordDB(string username, string NewPassword)
        {
            string sql = "UPDATE USERS SET PASSWORD = '" + NewPassword + "' WHERE USERNAME = '" + username + "';";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {

            }
            
        }

        /// <summary>
        /// Will remove the sessionid from the db. This means the user is no longer logged in according to the server
        /// </summary>
        /// <param name="session">The session id to clear.</param>
        public void removeSesssionDB(string session)
        {
            string sql = "UPDATE USERS SET SESSION_ID = NULL WHERE SESSION_ID = '" + session + "';";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {

            }
        }

        /// <summary>
        /// Returns a 2d array of quizzes that a given user_id would be a part of.
        /// </summary>
        /// <param name="user_id"> The user_id that we want to return quizzes for</param>
        /// <returns>A 2d array of quiz rows.</returns>
        public string[,] getQuizzesDB(int user_id)
        {
            // To size the array we count how many quizzes exist
            int quizCount = countTableDB("QUIZ");
            // make a list to hold the courses that the user is in
            List<string> courseList = new List<string>();
            // the string array to hold our results that we will return
            string[,] results = new string[quizCount,4];
            // First we get the courses the user is a part of
            string sql = "SELECT COURSE_ID FROM ROSTER WHERE USER_ID = '" + user_id + "'";
            
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);
                int i = 0;
                while (dataReader.Read())
                {
                    courseList.Add(dataReader.GetValue(0).ToString());
                    i++;
                }
                dataReader.Close();                
            }
            catch (System.Exception e)
            {
            }
            // After we get the list of courses the user is in, we get the quiz rows that matches that course_quiz using a join.
            if(courseList != null)
            {
                foreach (string s in courseList)
                {
                    sql = "SELECT QUIZ.QUI_ID, QUIZ.QUI_NAME, QUIZ.QUI_NOTES FROM QUIZ, COURSE_QUIZ WHERE QUIZ.QUI_ID = COURSE_QUIZ.QUI_ID AND COURSE_QUIZ.COURSE_ID = " + s;
                    try
                    {
                        SqlDataReader dataReader = GetDataReader(sql);
                        int i = 0;
                        while (dataReader.Read())
                        {
                            results[i, 0] = dataReader.GetValue(0).ToString();
                            results[i, 1] = dataReader.GetValue(1).ToString();
                            results[i, 2] = dataReader.GetValue(2).ToString();
                            i++;
                        }
                        dataReader.Close();
                    }
                    catch (System.Exception e)
                    {
                    }

                }
            }
            
            // TODO Check if quiz is already completed. Using the Grades table. if that quiz is already completed remove it from our array  
            
            return results;
        }

        /// <summary>
        /// This is the same as getQuizzesDB except it doesn't need a specified user as it will just return all quizzes
        /// </summary>
        /// <returns>A 2d array of quiz rows.</returns>
        public string[,] getQuizzesInstructorDB()
        {
            int quizCount = countTableDB("QUIZ");
            string[,] results = new string[quizCount, 4];
            string sql = "SELECT QUI_ID, QUI_NAME, QUI_NOTES FROM QUIZ";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                int i = 0;
                while (dataReader.Read())
                {
                    results[i, 0] = dataReader.GetValue(0).ToString();
                    results[i, 1] = dataReader.GetValue(1).ToString();
                    results[i, 2] = dataReader.GetValue(2).ToString();
                    i++;
                }
                databaseConnection.Close();
            }
            catch (System.Exception e)
            {
            }
            // Then we get the quizzes for each of those courses. We return this in a 2 dimensional array example row in the array: Qui_id 3 Qui_name PROGQUIZ1 Qui_Notes First Unit Quiz  Course_id 2  3,2,PROGQUIZ1,First Unit Quiz
            return results;

        }

        /// <summary>
        /// Counts a given tables number of rows
        /// </summary>
        /// <param name="tableName">The table to be counted</param>
        /// <returns> An integer representing the number of rows counted </returns>
        public int countTableDB(string tableName)
        {
            int count = 0;
            string sql = "SELECT COUNT(*) FROM " + tableName;
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (System.Exception e)
            {
            }
            return count;
        }

        /// <summary>
        /// Gets a user_id for a given username
        /// </summary>
        /// <param name="username"> The username to search for the user_id of</param>
        /// <returns>An integer representing the user_id</returns>
        public int getUserIDDB(string username)
        {
            string sql = "SELECT USER_ID FROM USERS WHERE USERNAME = '" + username + "'";
            try
            {
                // attempt to connect to our database
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();

                // build our sql command                
                SqlCommand command = new SqlCommand(sql, databaseConnection);

                // read the results
                // No need to iterate through datareader as we only expect one return value, so we can executescalar instead
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (System.Exception e)
            {
                // Maybe we got more than one user, or no users.
            }
            return 0;
        }

        /// <summary>
        /// Returns whether a given user_id is an instructor
        /// </summary>
        /// <param name="user_id">The user_id to be checked for access level</param>
        /// <returns>A boolean of whether the given user was an instructor</returns>
        public bool isInstructorDB(int user_id)
        {
            string sql = "SELECT IS_INSTRUCTOR FROM USERS WHERE USER_ID = '" + user_id + "'";
            try
            {
                // attempt to connect to our database
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();

                // build our sql command                
                SqlCommand command = new SqlCommand(sql, databaseConnection);

                // read the results
                // No need to iterate through datareader as we only expect one return value, so we can executescalar instead
                if (Convert.ToInt32(command.ExecuteScalar()) == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception e)
            {

            }
            return false;
        }

        /// <summary>
        /// Counts the number of questions for a given quiz_id
        /// </summary>
        /// <param name="quiz_ID">The id of the quiz to be counted</param>
        /// <returns>An integer representing the number of questions counted in the given quiz</returns>
        public int countQuestionsDB(int quiz_ID)
        {
            int count = 0;
            string sql = "SELECT COUNT(*) FROM QUIZ WHERE QUI_ID ='" + quiz_ID + "'";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (System.Exception e)
            {
            }
            return count;
        }

        /// <summary>
        /// This gets a int array of grade id, grade/points earned, and a bit flag of whether it needs additional grading.
        /// </summary>
        /// <param name="user_ID">The user_id to be getting the grade for</param>
        /// <param name="course_Quiz_ID"> The course_qui_ID of the grade to be retrieved</param>
        /// <returns> An int array containing gra_id, gra_grade, gra_needsGrading</returns>
        public int[] getGradeDB(int user_ID, int course_Quiz_ID)
        {
            // result is gra_ID, gra_grade, gra_needsgrading
            int[] result = new int[3];
            string sql = "SELECT GRA_ID, GRA_GRADE, GRA_NEEDSGRADING FROM GRADE WHERE COURSE_QUI_ID ='" + course_Quiz_ID + "' AND USER_ID = '" + user_ID + "'";
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);
                // We do a while loop to make sure values exist. We assume that they can only have one grade per course_quiz_id so this should only go through once but if there are multiple rows it will return the last row read.
                while(dataReader.Read())
                {
                    result[0] = Convert.ToInt32(dataReader.GetValue(0));
                    result[1] = Convert.ToInt32(dataReader.GetValue(1));
                    result[2] = Convert.ToInt32(dataReader.GetValue(2));
                }
            }
            catch (System.Exception e)
            {

            }
            return result;
        }

        /// <summary>
        /// Thie gets the quizName for a given quiz ID
        /// </summary>
        /// <param name="quiz_ID"></param>
        /// <returns></returns>
        public string getQuizNameDB(int quiz_ID)
        {
            string result = "";
            string sql = "SELECT QUI_NAME FROM QUIZ WHERE QUI_ID ='" + quiz_ID + "'";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                result = command.ExecuteScalar().ToString();
            }
            catch (System.Exception e)
            {

            }
            return result;
        }


        /// <summary>
        /// Gets a list of the question_ids for a given quiz_id
        /// </summary>
        /// <param name="quiz_ID">The id of the quiz to be searched for question_ids</param>
        /// <returns>A list of the question_ids that were part of the given quiz</returns>
        public List<int> getQuestionIDsDB(int quiz_ID)
        {
            List<int> results = new List<int>();
       
            string sql = "SELECT QUE_ID FROM QUESTION WHERE QUI_ID = '" + quiz_ID + "'";
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);
                while (dataReader.Read())
                {
                    results.Add(Convert.ToInt32(dataReader.GetValue(0)));
                }
                dataReader.Close();
            }
            catch (System.Exception e)
            {
            }
            return results;
        }

        /// <summary>
        /// Gets a list of string arrays of the rows of questions for a given quiz_id
        /// </summary>
        /// <param name="quiz_id"> The id of the quiz to be searched for the question data</param>
        /// <returns> A list of string arrays holding the question rows as data.</returns>
        public List<string[]> getQuestionDataDB(int quiz_id)
        {
            List<string[]> results = new List<string[]>();
            string sql = "SELECT QUE_ID, QUE_QUESTION, TYPE_ID, QUESTION_ANSWER FROM QUESTION WHERE QUI_ID = '" + quiz_id+ "'";
            try
            {

                SqlDataReader dataReader = GetDataReader(sql);
                while (dataReader.Read())
                {
                    results.Add(new string[] { dataReader.GetValue(0).ToString(), dataReader.GetValue(1).ToString(), dataReader.GetValue(2).ToString(), dataReader.GetValue(3).ToString() });
                }
                dataReader.Close();
            }
            catch (System.Exception e)
            {
            }
            // Then we get the quizzes for each of those courses. We return this in a 2 dimensional array example row in the array: Qui_id 3 Qui_name PROGQUIZ1 Qui_Notes First Unit Quiz  Course_id 2  3,2,PROGQUIZ1,First Unit Quiz
            return results;
        }

        /// <summary>
        /// Gets a List of the rows of answers for each question_id in a questionIDList
        /// </summary>
        /// <param name="questionIDList"> The list of question_ids to get the answers for</param>
        /// <returns> A List holding the answer rows as string arrays.</returns>
        public List<string[]> getAnswerDataDB(List<int> questionIDList)
        {

            List<string[]> results = new List<string[]>();
            string sql = "SELECT ANS_ID, QUE_ID, DESCRIPTION FROM ANSWER WHERE ";
            foreach (int i in questionIDList)
            {
                // Skip adding "OR" if first time
                if (i == questionIDList.First())
                    sql += "QUE_ID = '" + i + "'";
                else
                    sql += " OR QUE_ID = '" + i + "'";
            }
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);
                while (dataReader.Read())
                {
                    string[] singleResult = new string[3];
                    singleResult[0] = dataReader.GetValue(0).ToString();
                    singleResult[1] = dataReader.GetValue(1).ToString();
                    singleResult[2] = dataReader.GetValue(2).ToString();
                    results.Add(singleResult);
                }
                dataReader.Close();
            }
            catch (System.Exception e)
            {
            }
            // Then we get the quizzes for each of those courses. We return this in a List of string arrays example row in the list: ans_id 1, que_id 2, description what is an apple?
            return results;
        }

        /// <summary>
        /// gets a list of the correct answer strings for a given quiz_id
        /// </summary>
        /// <param name="quiz_ID">The quiz_ID of the quiz we want the answers for</param>
        /// <returns> A list of the correct answer strings for a given quiz</returns>
        public List<string> getAnswersDB(int quiz_ID)
        {

            List<string> results = new List<string>();
            string sql = "SELECT QUESTION_ANSWER FROM QUESTION WHERE QUI_ID ='" + quiz_ID +"';";

            try
            {

                SqlDataReader dataReader = GetDataReader(sql);
                while (dataReader.Read())
                {
                    results.Add(dataReader.GetValue(0).ToString());
                }
                dataReader.Close();

            }
            catch (System.Exception e)
            {
            }
            // Then we get the quizzes for each of those courses. We return this in a List of string arrays example row in the list: ans_id 1, que_id 2, description what is an apple?
            return results;
        }

        /// <summary>
        /// This method is called when a user submits a quiz and attempts to automatically grade the quiz and return the point value they got. It will also flag the grade for further review if it contained a short answer question or a fill in the blank question that did not match the saved answer(to check for spelling errors or alternatively worded answers)
        /// </summary>
        /// <param name="quiz_ID">The quiz_Id of the quiz being graded</param>
        /// <param name="responseList">The list of responses the user is submitting for grading</param>
        /// <param name="user_ID"> The user who is submitting their quiz</param>
        /// <param name="course_Quiz_ID"> the course_quiz_id of the given quiz</param>
        /// <returns> An integer of the point value they received from the automatic grading. </returns>
        public int gradeQuizDB(int quiz_ID, List<string> responseList, int user_ID, int course_Quiz_ID)
        {
            bool markForInstructorGrading = false;
            int grade = 0;
            List<string[]> questionList = getQuestionDataDB(quiz_ID);
            // List of {Que_ID, Response}
            List<string[]> responseQuestionList = new List<string[]>();
            string type_ID;
            string correctAnswer;

            int i = 0;
            foreach (string[] q in questionList)
            {
                string response = responseList[i];
                string que_ID = q[0];                
                type_ID = q[2];
                correctAnswer = q[3];

                // Depending on the question type we will attempt to grade differently
                // Short answer and fill in the blank are stored as normal strings of the answer, short answer doesn't need to be stored as this should be manually graded and will be different for each user.
                // Multiple Choice Radio and Multiple Choice Check questions will be compared against a answer stored like (a,b) or (a). 
                if (type_ID.Contains("MCR"))
                {
                    if (response == correctAnswer)
                        grade++;
                }
                if (type_ID.Contains("SA"))
                {
                    // Short answers will have to be manually graded by instructor
                    markForInstructorGrading = true;
                }
                if (type_ID.Contains("FB"))
                {
                    // Fill in the blanks can be attempted to auto grade, but mark them for grading if they don't match.
                    if (response == correctAnswer)
                        grade++;
                    else
                        markForInstructorGrading = true;
                }
                if (type_ID.Contains("MCC"))
                {
                    // Since multiple choice allows partial credit we will add a point for every correct char existing and subtract for every incorrect attempt(Example: Answer is ab, response was abc they would get 2 points - 1 points for a total of 1)
                    // It is like checking if two strings are anagrams except there can't be duplicate characters so we don't need to remove the char from the charArray(string) we are checking against
                    foreach (char c in response)
                    {
                        // So if the response char is not a comma we see if the correctAnswer contained it 
                        //Example(Correctanswer = a,b,c response= a,b) would give a grade total for this section of 2 points of 3 possible. If they had answered a,b,d they would get 1 point as d would not have been in the correct answer and they will be penalised for putting a incorrect answer.
                        if (correctAnswer.Contains(c) && c != ',')
                            grade++;
                        else if(c != ',')
                            grade--;
                    }
                }
                // Each response is stored and will be sent to the response table.
                responseQuestionList.Add(new string[] { que_ID, response });
                i++;
            }

            // Users can't have negative grades, but they can lose points for putting in incorrect mcc responses so we floor the grade to 0.
            if (grade < 0)
                grade = 0;

            // After grades are done we can add the data to the response table and the grade table
            AddResponseDB(user_ID, course_Quiz_ID, responseQuestionList);
            AddGradeDB( user_ID, course_Quiz_ID, grade ,markForInstructorGrading);
            return grade;
        }

        /// <summary>
        /// Adds a list of responses to the response table
        /// </summary>
        /// <param name="user_ID">The user who submitted the responses</param>
        /// <param name="course_Quiz_ID"> The course_quiz the response was for</param>
        /// <param name="responseQuestionList"> an array containing each response and the que_id it was a response to</param>
        public void AddResponseDB(int user_ID, int course_Quiz_ID, List<string[]> responseQuestionList)
        {

            string sql = "INSERT INTO RESPONSE (QUE_ID, USER_ID, COURSE_QUI_ID, COMMENTS) VALUES";
            foreach(string[] s in responseQuestionList)
            {
                if(responseQuestionList.Last() != s)
                    sql += "('" + s[0] + "', '" + user_ID + "', '" + course_Quiz_ID + "', '" + s[1] + "'), ";
                else
                    sql += "('" + s[0] + "', '" + user_ID + "', '" + course_Quiz_ID + "', '" + s[1] + "');";
            }
            
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();

            }
            catch (System.Exception e)
            {

            }
        }

        /// <summary>
        /// Adds our grade total to the grade table as well as marking the grade for review if needed
        /// </summary>
        /// <param name="user_ID"> The user the grade is for </param>
        /// <param name="course_Quiz_ID"> The course_quiz the grade is for </param>
        /// <param name="grade"> The grade value they received </param>
        /// <param name="markForGrading"> A bool marking whether the grade needs to be checked by an instructor</param>
        public void AddGradeDB(int user_ID, int course_Quiz_ID, int grade, bool markForGrading)
        {
            // convert our bool into a bit if needed.
            int mark = 0;
            if (markForGrading)
                mark = 1;

            string sql = "INSERT INTO GRADE (USER_ID, COURSE_QUI_ID, GRA_GRADE, GRA_NEEDSGRADING) VALUES('" + user_ID + "', '" + course_Quiz_ID + "', '" + grade +"', '" + mark + "');";
            try
            {
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(sql, databaseConnection);
                command.ExecuteNonQuery();

            }
            catch (System.Exception e)
            {

            }
        }

        /// <summary>
        /// Gets the course_quizID for a given user's given quiz
        /// </summary>
        /// <param name="quiz_ID">The quiz id that we are gettting the course_Quiz for</param>
        /// <param name="user_ID">The user Id that will determine the specific course_quiz</param>
        /// <returns> An int of of course_quiz for the given user and quiz</returns>
        public int getCourseQuizIDDB(int quiz_ID, int user_ID)
        {
            int result = 0;
            // Get all course id's that the user is in.
            List<int> courseIDList = new List<int>();
            string sql = "SELECT COURSE_ID FROM ROSTER WHERE USER_ID = '" + user_ID + "'";
            try
            {
                SqlDataReader dataReader = GetDataReader(sql);

                while (dataReader.Read())
                {
                    courseIDList.Add(Convert.ToInt32(dataReader.GetValue(0)));
                }
                dataReader.Close();
            }
            catch (System.Exception e)
            {
            }
            
            sql = "SELECT COURSE_QUI_ID FROM COURSE_QUIZ WHERE QUI_ID = '" + quiz_ID + "' AND (";
            foreach (int i in courseIDList)
            {
                // Skip adding "OR" if first time
                if (i == courseIDList.First())
                    sql += "COURSE_ID = '" + i + "'";
                else
                    sql += " OR COURSE_ID = '" + i + "'";
            }
            sql += ");";
            try
            { 
                // attempt to connect to our database
                SqlConnection databaseConnection;
                databaseConnection = new SqlConnection(dbLocationNetwork);
                databaseConnection.Open();

                // build our sql command                
                SqlCommand command = new SqlCommand(sql, databaseConnection);

                // No need to iterate through datareader as we only expect one return value, so we can executescalar instead
                result = Convert.ToInt32(command.ExecuteScalar());
                databaseConnection.Close();

            }
            catch (System.Exception e)
            {
            }

            // Get the course_quiz id where qui_id and course_id.list
            return result;
        }

        /// <summary>
        /// Returns a SqlDataReader object to be used by DatabaseCreator.cs to connect to the database and be able to read the results of a given sql select statement
        /// </summary>
        /// <param name="sql"> The sql select query to return the object for reading of</param>
        /// <returns> A SqlDataReader object for a given sql command</returns>
        private SqlDataReader GetDataReader(string sql)
        {
            SqlConnection databaseConnection;
            databaseConnection = new SqlConnection(dbLocationNetwork);
            databaseConnection.Open();
            SqlCommand command = new SqlCommand(sql, databaseConnection);
            SqlDataReader dataReader = command.ExecuteReader();
            return dataReader;
        }
    }
}
