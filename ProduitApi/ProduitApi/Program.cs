using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RitegeQueueManager.Interface;
using RabbitMQ.Client;
using RitegeQueueManager.Model;
using System.Threading;
using RitegeQueueManager.Implement;
using Newtonsoft.Json;
using ProduitApi.Models;
using System.Diagnostics;

namespace ProduitApi
{
    public class Program
    {
        static RabbitMQManager rabbitMQManager;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            rabbitMQManager = new RabbitMQManager(connection.Result.CreateModel());
            rabbitMQManager.OnConsommerMessage += new Traitement().TraiterMessageConsommer;
            rabbitMQManager.ConsommerMessage(RitegeQueueManager.Interface.Type.BD);
                Debug.WriteLine("receiver");

          


        }


     

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
