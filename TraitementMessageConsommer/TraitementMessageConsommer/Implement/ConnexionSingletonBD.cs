using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TraitementMessageConsommer.Interface;

namespace TraitementMessageConsommer.Implement
{
    class ConnexionSingletonBD 
    {
        private static MySqlConnection _connection;
        /// <summary>
        /// cree une connexion singleton BD
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="uid"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public static async Task<MySqlConnection> Connexion(string server, string database, string uid, string password)
        {

            if (_connection == null)
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
            await Task.Yield();

            return _connection;

        }
    }
}
