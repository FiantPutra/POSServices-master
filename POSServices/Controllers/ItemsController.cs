using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/Items")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(from param in _context.Item.ToList()
                      select new ItemAPIModel
                      {
                          Id = param.Id,
                          //Brand = param.Brand,
                          //Color = param.Color,
                          //Department = param.Department,
                          //DepartmentType = param.DepartmentType,
                          //Gender = param.Gender,
                          IsServiceItem = param.IsServiceItem,
                          ItemGroup = param.ItemGroup,
                          //ItemGroupDesc = param.ItemGroupDesc,
                          ItemId = param.ItemId,
                          ItemIdAlias = param.ItemIdAlias,
                          Name = param.Name,
                          //Size = param.Size
                      });
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
