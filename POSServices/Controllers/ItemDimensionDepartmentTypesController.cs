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
    public class ItemDimensionDepartmentTypesController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionDepartmentTypesController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        /*
        // GET: api/ItemDimensionDepartmentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDimensionDepartmentType>>> GetItemDimensionDepartmentType()
        {
            return await _context.ItemDimensionDepartmentType.ToListAsync();
        }

        // GET: api/ItemDimensionDepartmentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionDepartmentType>> GetItemDimensionDepartmentType(int id)
        {
            var itemDimensionDepartmentType = await _context.ItemDimensionDepartmentType.FindAsync(id);

            if (itemDimensionDepartmentType == null)
            {
                return NotFound();
            }

            return itemDimensionDepartmentType;
        }
        */

        // GET: api/ItemDimensionDepartmentTypes
        [HttpGet]
        public ActionResult GetItemDimensionDepartmentType()
        {
            return Ok(from param in _context.ItemDimensionDepartmentType.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionDepartmentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionDepartment>> GetItemDimensionDepartmentType(int id)
        {
            var param = await _context.ItemDimensionDepartmentType.FindAsync(id);

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionDepartmentTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionDepartmentType(int id, ItemDimensionDepartmentType itemDimensionDepartmentType)
        {
            if (id != itemDimensionDepartmentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionDepartmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionDepartmentTypeExists(id))
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

        // POST: api/ItemDimensionDepartmentTypes
        [HttpPost]
        public async Task<ActionResult<ItemDimensionDepartmentType>> PostItemDimensionDepartmentType(ItemDimensionDepartmentType itemDimensionDepartmentType)
        {
            _context.ItemDimensionDepartmentType.Add(itemDimensionDepartmentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionDepartmentType", new { id = itemDimensionDepartmentType.Id }, itemDimensionDepartmentType);
        }

        // DELETE: api/ItemDimensionDepartmentTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionDepartmentType>> DeleteItemDimensionDepartmentType(int id)
        {
            var itemDimensionDepartmentType = await _context.ItemDimensionDepartmentType.FindAsync(id);
            if (itemDimensionDepartmentType == null)
            {
                return NotFound();
            }

            _context.ItemDimensionDepartmentType.Remove(itemDimensionDepartmentType);
            await _context.SaveChangesAsync();

            return itemDimensionDepartmentType;
        }

        private bool ItemDimensionDepartmentTypeExists(int id)
        {
            return _context.ItemDimensionDepartmentType.Any(e => e.Id == id);
        }
    }
}
