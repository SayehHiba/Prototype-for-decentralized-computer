using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProduitApi.Models;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ProduitApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

           Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            IModel channel = connection.Result.CreateModel();


            channel.BasicQos(0, 1, false);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received +=  (sender, basicDeliveryEventArgs) =>
            {
               // string response;
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                VenteItem vente = JsonConvert.DeserializeObject<VenteItem>(message);
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);


                TraiterMessageConsommer.Traitement(vente);
                Debug.WriteLine("\n\n fin \n\n");
                /* Debug.WriteLine("\n\n"+produitItem.Result.ToString()+ "\n\n");
                  while (test == null) { }
                  ProduitItem item = JsonConvert.DeserializeObject<ProduitItem>(ProduitItemsController.test);
                  Debug.WriteLine("\n\n" + " item: " + item.Id + " / " + item.Nom + " / " + item.Stock + " / " + "\n\n");
                  if (item.Stock >= vente.Quantite)
                  {
                      item.Stock -= vente.Quantite;
                      Task<IActionResult> pp = PutProduitItem(item.Id, item);
                      Debug.WriteLine("\n\n" + pp.ToString() + "\n\n");
                      if (pp.ToString().Equals("Microsoft.AspNetCore.Mvc.NoContentResult"))
                      {
                          response = "Valide";
                      }
                      else { response = "Non"; }

                  }
                  else { response = "Non"; }




                  Debug.WriteLine("\n\n" + " Message: " + message + " Enter your response: " + response + "\n\n");
                  IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
                  replyBasicProperties.CorrelationId = basicDeliveryEventArgs.BasicProperties.CorrelationId;
                  byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                  channel.BasicPublish("", basicDeliveryEventArgs.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
             */
            };
               

            channel.BasicConsume("mycompany.queues.rpc", false, eventingBasicConsumer);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<ProduitContext>
            (o => o.UseMySQL(Configuration.
             GetConnectionString("MyDb")));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "Produit Api ", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Produit API 1");
            });
         
        }
    }
}
