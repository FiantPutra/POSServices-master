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
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDimensionColorsController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionColorsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        /*
        // GET: api/ItemDimensionColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDimensionColor>>> GetItemDimensionColor()
        {
            return await _context.ItemDimensionColor.ToListAsync();
        }

        // GET: api/ItemDimensionColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionColor>> GetItemDimensionColor(int id)
        {
            var itemDimensionColor = await _context.ItemDimensionColor.FindAsync(id);

            if (itemDimensionColor == null)
            {
                return NotFound();
            }

            return itemDimensionColor;
        } */

        // GET: api/ItemDimensionColors
        [HttpGet]
        public ActionResult GetItemDimensionColor()
        {
            return Ok(from param in _context.ItemDimensionColor.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionColor>> GetItemDimensionColor(int id)
        {
            var param = await _context.ItemDimensionColor.FindAsync(id);

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionColors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionColor(int id, ItemDimensionColor itemDimensionColor)
        {
            if (id != itemDimensionColor.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionColorExists(id))
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

        // POST: api/ItemDimensionColors
        [HttpPost]
        public async Task<ActionResult<ItemDimensionColor>> PostItemDimensionColor(ItemDimensionColor itemDimensionColor)
        {
            _context.ItemDimensionColor.Add(itemDimensionColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionColor", new { id = itemDimensionColor.Id }, itemDimensionColor);
        }

        // DELETE: api/ItemDimensionColors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionColor>> DeleteItemDimensionColor(int id)
        {
            var itemDimensionColor = await _context.ItemDimensionColor.FindAsync(id);
            if (itemDimensionColor == null)
            {
                return NotFound();
            }

            _context.ItemDimensionColor.Remove(itemDimensionColor);
            await _context.SaveChangesAsync();

            return itemDimensionColor;
        }

        private bool ItemDimensionColorExists(int id)
        {
            return _context.ItemDimensionColor.Any(e => e.Id == id);
        }
    }
}
