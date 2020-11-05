using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenteApi.Models;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using Newtonsoft.Json;

namespace VenteApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VenteItemsController : ControllerBase
    {
        private readonly VenteContext _context;

        public VenteItemsController(VenteContext context)
        {
            _context = context;
        }

        // GET: api/VenteItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenteItem>>> GetProduitItems()
        {
            return await _context.ProduitItems.ToListAsync();
        }

        // GET: api/VenteItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenteItem>> GetVenteItem(int id)
        {
            var venteItem = await _context.ProduitItems.FindAsync(id);

            if (venteItem == null)
            {
                return NotFound();
            }

            return venteItem;
        }

        // PUT: api/VenteItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        

        // POST: api/VenteItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VenteItem>> PostVenteItem(VenteItem venteItem)
        {
            int a = 0;

            Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            IModel channel = connection.Result.CreateModel();


            channel.QueueDeclare("mycompany.queues.rpc", true, false, false, null);
            //SendRpcMessagesBackAndForth(channel);


            string rpcResponseQueue = channel.QueueDeclare().QueueName;

            string correlationId = Guid.NewGuid().ToString();
            string responseFromConsumer = null;

            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.ReplyTo = rpcResponseQueue;
            basicProperties.CorrelationId = correlationId;

            string message = JsonConvert.SerializeObject(venteItem);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "mycompany.queues.rpc", basicProperties, messageBytes);

            EventingBasicConsumer rpcEventingBasicConsumer = new EventingBasicConsumer(channel);
            rpcEventingBasicConsumer.Received +=(sender, basicDeliveryEventArgs) =>
            {
                IBasicProperties props = basicDeliveryEventArgs.BasicProperties;
                if (props != null
                    && props.CorrelationId == correlationId)
                {
                    string response = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                    responseFromConsumer = response;
                }
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);

                if (responseFromConsumer.Equals("Valide"))
                {
                    
                    a = 1;
                }
              
                   };
            channel.BasicConsume(rpcResponseQueue, false, rpcEventingBasicConsumer);


            channel.Close();
            connection.Result.Close();

            if (a == 1)
            {
                _context.ProduitItems.Add(venteItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetVenteItem", new { id = venteItem.Id }, venteItem);

            }
            else
            {
                return null;
            }
            




            //end

        }

       
        private bool VenteItemExists(int id)
        {
            return _context.ProduitItems.Any(e => e.Id == id);
        }
    }
}
