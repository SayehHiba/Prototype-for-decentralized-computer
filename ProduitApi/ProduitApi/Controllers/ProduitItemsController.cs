using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProduitApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProduitApi
{
    [Route("[controller]")]
    [ApiController]
    public class ProduitItemsController : ControllerBase
    {
        private readonly ProduitContext _context;
        private static  ProduitContext _contexttest;

        public ProduitItemsController(ProduitContext context)
        {
            _context = context;
            _contexttest = _context;
        }


        // GET: /ProduitItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProduitItem>>> GetProduitItems()
        {
            return await _context.ProduitItems.ToListAsync();
        }
        
        // GET: /ProduitItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProduitItem>> GetProduitItem(int id)
        {
            var produitItem = await _context.ProduitItems.FindAsync(id);

            if (produitItem == null)
            {
                return NotFound();
            }
            return produitItem;
        }

        // PUT: api/ProduitItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduitItem(int id, ProduitItem produitItem)
        {
            if (id != produitItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(produitItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                if (!ProduitItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            NoContentResult res= NoContent();
            Debug.WriteLine("\n\n" + res.ToString()+ "\n\n");
           
            return res;
        }

        // POST: api/ProduitItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProduitItem>> PostProduitItem(ProduitItem produitItem)
        {
            _context.ProduitItems.Add(produitItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduitItem", new { id = produitItem.Id }, produitItem);
        }

        // DELETE: api/ProduitItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProduitItem>> DeleteProduitItem(int id)
        {
            var produitItem = await _context.ProduitItems.FindAsync(id);
            if (produitItem == null)
            {
                return NotFound();
            }

            _context.ProduitItems.Remove(produitItem);
            await _context.SaveChangesAsync();

            return produitItem;
        }

        private bool ProduitItemExists(int id)
        {
            return _context.ProduitItems.Any(e => e.Id == id);
        }
       
    }
    }
