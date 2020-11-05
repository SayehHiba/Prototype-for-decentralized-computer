using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProduitApi.Models
{
    public class TraiterMessageConsommer
    {
        private static MySqlConnection connection;
        public static async void Traitement(VenteItem vente)
        {
            Initialize();

           await Select(vente);

            Debug.WriteLine(" \n\n fin traitement \n\n");
            
        }

        private static bool OpenConnection()
        {
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
        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task Select(VenteItem id)
        {
            string query = "SELECT * FROM produit where id="+id.IdProduit;

            //Create a list to store the result
            ProduitItem produit = new ProduitItem();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command

                MySqlDataReader reader;
                using (reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.WriteLine("\n\n"+String.Format("{0}", reader["nom"]));
                    }
                }
                //Read the data and store them in the list
               
                //close Data Reader
                reader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                
            }
            else
            {
                Debug.WriteLine("\n\n probleme con" );
            }
            await Task.Yield();
        }
        private static void Initialize()
        {
            string server = "localhost";
            string database = "produitbase";
            string uid = "root";
            string password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
    }
}
