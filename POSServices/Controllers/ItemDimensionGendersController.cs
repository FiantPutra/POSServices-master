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
    public class ItemDimensionGendersController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionGendersController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        /*
        // GET: api/ItemDimensionGenders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDimensionGender>>> GetItemDimensionGender()
        {
            return await _context.ItemDimensionGender.ToListAsync();
        }

        // GET: api/ItemDimensionGenders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionGender>> GetItemDimensionGender(int id)
        {
            var itemDimensionGender = await _context.ItemDimensionGender.FindAsync(id);

            if (itemDimensionGender == null)
            {
                return NotFound();
            }

            return itemDimensionGender;
        } */

        [HttpGet]
        public ActionResult GetItemDimensionGender()
        {
            return Ok(from param in _context.ItemDimensionGender.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionDepartmentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionGender>> GetItemDimensionGender(int id)
        {
            var param = await _context.ItemDimensionGender.FindAsync(id);

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionGenders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionGender(int id, ItemDimensionGender itemDimensionGender)
        {
            if (id != itemDimensionGender.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionGender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionGenderExists(id))
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

        // POST: api/ItemDimensionGenders
        [HttpPost]
        public async Task<ActionResult<ItemDimensionGender>> PostItemDimensionGender(ItemDimensionGender itemDimensionGender)
        {
            _context.ItemDimensionGender.Add(itemDimensionGender);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionGender", new { id = itemDimensionGender.Id }, itemDimensionGender);
        }

        // DELETE: api/ItemDimensionGenders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionGender>> DeleteItemDimensionGender(int id)
        {
            var itemDimensionGender = await _context.ItemDimensionGender.FindAsync(id);
            if (itemDimensionGender == null)
            {
                return NotFound();
            }

            _context.ItemDimensionGender.Remove(itemDimensionGender);
            await _context.SaveChangesAsync();

            return itemDimensionGender;
        }

        private bool ItemDimensionGenderExists(int id)
        {
            return _context.ItemDimensionGender.Any(e => e.Id == id);
        }
    }
}
