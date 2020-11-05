using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MessageConsommer.Implement
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
            await Task.Yield();

            if (_connection == null)
            {
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

                _connection = new MySqlConnection(connectionString);
            }
           

            return _connection;

        }
    }
}
