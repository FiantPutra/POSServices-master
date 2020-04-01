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
    [Route("api/ItemDimensionSizes")]
    [ApiController]
    public class ItemDimensionSizesController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionSizesController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        /*
        // GET: api/ItemDimensionSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDimensionSize>>> GetItemDimensionSize()
        {
            return await _context.ItemDimensionSize.ToListAsync();
        }

        // GET: api/ItemDimensionSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionSize>> GetItemDimensionSize(int id)
        {
            var itemDimensionSize = await _context.ItemDimensionSize.FindAsync(id);

            if (itemDimensionSize == null)
            {
                return NotFound();
            }

            return itemDimensionSize;
        } */

        // GET: api/ItemDimensionSizes
        [HttpGet]
        public IActionResult GetItemDimensionSize()
        {
            return Ok(from param in _context.ItemDimensionSize.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionSizes/5
        [HttpGet("{id}")]
        public IActionResult GetItemDimensionSize(int id)
        {
            var param = _context.ItemDimensionSize.Where(x => x.Id == id).FirstOrDefault();

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionSizes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionSize(int id, ItemDimensionSize itemDimensionSize)
        {
            if (id != itemDimensionSize.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionSize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionSizeExists(id))
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

        // POST: api/ItemDimensionSizes
        [HttpPost]
        public async Task<ActionResult<ItemDimensionSize>> PostItemDimensionSize(ItemDimensionSize itemDimensionSize)
        {
            _context.ItemDimensionSize.Add(itemDimensionSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionSize", new { id = itemDimensionSize.Id }, itemDimensionSize);
        }

        // DELETE: api/ItemDimensionSizes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionSize>> DeleteItemDimensionSize(int id)
        {
            var itemDimensionSize = await _context.ItemDimensionSize.FindAsync(id);
            if (itemDimensionSize == null)
            {
                return NotFound();
            }

            _context.ItemDimensionSize.Remove(itemDimensionSize);
            await _context.SaveChangesAsync();

            return itemDimensionSize;
        }

        private bool ItemDimensionSizeExists(int id)
        {
            return _context.ItemDimensionSize.Any(e => e.Id == id);
        }
    }
}
