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
using RabbitMQManager.Models;
using RabbitMQManager.Implement;

namespace VenteApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VenteItemsController : ControllerBase
    {
        private readonly VenteContext _context;
        static string reponse;
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
          
            reponse = "";

            Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            RabbitMQContext rabbitMQContext = new RabbitMQContext(connection.Result.CreateModel());
            string message = JsonConvert.SerializeObject(venteItem);
            rabbitMQContext.OnRecupererReponse += TraiterReponse;
            rabbitMQContext.PublierMessage(message);
            while (reponse == ""){  }
            if (reponse == "Valide")
            {
                _context.ProduitItems.Add(venteItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetVenteItem", new { id = venteItem.Id }, venteItem);

            }
            else { return null; }


        }

       private void TraiterReponse(object sender,MessageBodyEvent e)
        {
            if (e.Message.Equals("Valide"))
            {
                reponse = "Valide";
            }else { reponse = "nonValide"; }

        }
        private bool VenteItemExists(int id)
        {
            return _context.ProduitItems.Any(e => e.Id == id);
        }
    }
}
