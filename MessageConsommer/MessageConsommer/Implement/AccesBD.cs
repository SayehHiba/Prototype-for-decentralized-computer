using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MessageConsommer.Implement
{
    class AccesBD
    {

        public static async Task<bool> OpenConnection(MySqlConnection connection)
        {
            await Task.Yield();
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Debug.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Debug.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public static async Task CloseConnection(MySqlConnection connection)
        {
            await Task.Yield();
            try
            {
                connection.Close();
   
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
       
            }
        }
    }
}
