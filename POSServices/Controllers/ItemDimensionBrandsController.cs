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
    public class ItemDimensionBrandsController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionBrandsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        /*        [HttpGet]
                public async Task<ActionResult<IEnumerable<ItemDimensionBrand>>> GetItemDimensionBrand()
                {
                    return await _context.ItemDimensionBrand.ToListAsync();
                } 
                
             
             public async Task<IActionResult> PutItemDimensionBrand(int id, ItemDimensionBrand itemDimensionBrand)
        {
            if (id != itemDimensionBrand.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionBrandExists(id))
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
             
             */
                                              
        // GET: api/ItemDimensionBrands
        [HttpGet]
        public ActionResult GetItemDimensionBrand()
        {
            return Ok(from param in _context.ItemDimensionBrand.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionBrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionBrand>> GetItemDimensionBrand(int id)
        {
            var param = await _context.ItemDimensionBrand.FindAsync(id);

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description                
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionBrands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionBrand(int id, ItemDimensionBrand itemDimensionBrand)
        {
            if (id != itemDimensionBrand.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionBrandExists(id))
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

        // POST: api/ItemDimensionBrands
        [HttpPost]
        public async Task<ActionResult<ItemDimensionBrand>> PostItemDimensionBrand(ItemDimensionBrand itemDimensionBrand)
        {
            _context.ItemDimensionBrand.Add(itemDimensionBrand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionBrand", new { id = itemDimensionBrand.Id }, itemDimensionBrand);
        }

        // DELETE: api/ItemDimensionBrands/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionBrand>> DeleteItemDimensionBrand(int id)
        {
            var itemDimensionBrand = await _context.ItemDimensionBrand.FindAsync(id);
            if (itemDimensionBrand == null)
            {
                return NotFound();
            }

            _context.ItemDimensionBrand.Remove(itemDimensionBrand);
            await _context.SaveChangesAsync();

            return itemDimensionBrand;
        }

        private bool ItemDimensionBrandExists(int id)
        {
            return _context.ItemDimensionBrand.Any(e => e.Id == id);
        }
    }
}
