using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenteApi.Models;

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
           
            _context.ProduitItems.Add(venteItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenteItem", new { id = venteItem.Id }, venteItem);
        }

       
        private bool VenteItemExists(int id)
        {
            return _context.ProduitItems.Any(e => e.Id == id);
        }
    }
}
