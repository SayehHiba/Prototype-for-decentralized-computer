using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenteApi.Models;
using RabbitMQ.Client;
using RitegeQueueManager.Implement;
using RitegeQueueManager.Model;
using System.Diagnostics;

namespace VenteApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VenteItemsController : ControllerBase
    {
        static string reponse;
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


            reponse = "vide";

            Task<IConnection> connection = ConnexionSingleton.Connexion("localhost", "guest", "guest", new ConnectionFactory());
            RabbitMQManager rabbitMQManager = new RabbitMQManager(connection.Result.CreateModel());
            List<object> list = new List<object>();
            list.Add(venteItem.IdProduit);
            list.Add(venteItem.Quantite);
            DescriptionMessage descriptionMessage = new DescriptionMessage("Controllers", "ProduitItemsController", list);
            rabbitMQManager.OnRecupererReponse += TraiterReponseAuthentification;
            rabbitMQManager.PublierMessage(descriptionMessage, RitegeQueueManager.Interface.Type.BD);

            while (reponse == "vide") { }
            Debug.WriteLine(reponse.ToString());
            if(reponse=="Valide")

            { 

            _context.ProduitItems.Add(venteItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenteItem", new { id = venteItem.Id }, venteItem);
            }
            else
            {
                return null;
            }
        }

        private void TraiterReponseAuthentification(object sender, MessageBodyEvent e)
        {

            if (e.Message.Equals("null"))
            {
                reponse = null;
            }
            else { reponse = e.Message; }
        }

        private bool VenteItemExists(int id)
        {
            return _context.ProduitItems.Any(e => e.Id == id);
        }
    }
}
