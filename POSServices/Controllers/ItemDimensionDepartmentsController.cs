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
    public class ItemDimensionDepartmentsController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public ItemDimensionDepartmentsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        /*
        // GET: api/ItemDimensionDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDimensionDepartment>>> GetItemDimensionDepartment()
        {
            return await _context.ItemDimensionDepartment.ToListAsync();
        }

        // GET: api/ItemDimensionDepartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionDepartment>> GetItemDimensionDepartment(int id)
        {
            var itemDimensionDepartment = await _context.ItemDimensionDepartment.FindAsync(id);

            if (itemDimensionDepartment == null)
            {
                return NotFound();
            }

            return itemDimensionDepartment;
        }*/

        // GET: api/ItemDimensionDepartments
        [HttpGet]
        public ActionResult GetItemDimensionDepartment()
        {
            return Ok(from param in _context.ItemDimensionDepartment.ToList()
                      select new ItemDimensionAPIModel
                      {
                          Id = param.Id,
                          Code = param.Code,
                          Description = param.Description
                      });
        }

        // GET: api/ItemDimensionDepartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDimensionDepartment>> GetItemDimensionDepartment(int id)
        {
            var param = await _context.ItemDimensionDepartment.FindAsync(id);

            var model = new ItemDimensionAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };

            return Ok(model);
        }

        // PUT: api/ItemDimensionDepartments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDimensionDepartment(int id, ItemDimensionDepartment itemDimensionDepartment)
        {
            if (id != itemDimensionDepartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDimensionDepartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDimensionDepartmentExists(id))
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

        // POST: api/ItemDimensionDepartments
        [HttpPost]
        public async Task<ActionResult<ItemDimensionDepartment>> PostItemDimensionDepartment(ItemDimensionDepartment itemDimensionDepartment)
        {
            _context.ItemDimensionDepartment.Add(itemDimensionDepartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDimensionDepartment", new { id = itemDimensionDepartment.Id }, itemDimensionDepartment);
        }

        // DELETE: api/ItemDimensionDepartments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDimensionDepartment>> DeleteItemDimensionDepartment(int id)
        {
            var itemDimensionDepartment = await _context.ItemDimensionDepartment.FindAsync(id);
            if (itemDimensionDepartment == null)
            {
                return NotFound();
            }

            _context.ItemDimensionDepartment.Remove(itemDimensionDepartment);
            await _context.SaveChangesAsync();

            return itemDimensionDepartment;
        }

        private bool ItemDimensionDepartmentExists(int id)
        {
            return _context.ItemDimensionDepartment.Any(e => e.Id == id);
        }
    }
}
