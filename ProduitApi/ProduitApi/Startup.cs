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
using RabbitMQManager.Models;
using RabbitMQManager.Implement;
using MessageConsommer.Implement;
using MessageConsommer.Models;
using MySql.Data;

namespace ProduitApi
{
    public class Startup
    {
        static RabbitMQContext rabbitMQContext;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

           Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            rabbitMQContext = new RabbitMQContext(connection.Result.CreateModel());
            rabbitMQContext.OnConsommerMessage += TraiterReq;
            rabbitMQContext.ConsommerMessage();

        }

        private void TraiterReq(object sender, MessageBodyEvent e)
        {

            VenteItem vente = JsonConvert.DeserializeObject<VenteItem>(e.Message);
           string res = Traitement.GérerStock(vente).Result;
            rabbitMQContext.publierReponse(e, res);
        
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
