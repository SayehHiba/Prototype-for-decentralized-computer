using RabbitMQ.Client;
using System.Threading.Tasks;

namespace RabbitMQManager.Models
{
    public class ConnexionSingleton
    {
        private static IConnection _connection;
        /// <summary>
        /// cree une connexion singleton
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="connectionFactory"></param>
        /// <returns></returns>
        public static async Task<IConnection> Connexion(string hostName, string userName, string password, ConnectionFactory connectionFactory)
        {

            if (_connection == null)
            {
                connectionFactory.HostName = hostName;
                connectionFactory.UserName = userName;
                connectionFactory.Password = password;

                _connection = connectionFactory.CreateConnection();
            }
            await Task.Yield();

            return _connection;

        }
    }
}
