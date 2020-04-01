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
    public class StoreTypesController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public StoreTypesController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/ItemGroups
        [HttpGet]
        public IActionResult GetStoreType()
        {
            return Ok(from param in _context.StoreType.ToList()
                      select new StoreTypeAPIModel
                      {
                          Id = param.Id,
                          InforOrderTypeNormal = param.InforOrderTypeNormal,
                          InforOrderTypeRetur = param.InforOrderTypeRetur,
                          InforXrcdnormal = param.InforXrcdnormal,
                          InforXrcdretur = param.InforXrcdretur,
                          Name = param.Name,
                          StoreInStore = param.StoreInStore,
                          TypeId = param.TypeId
                      });
        }

        // GET: api/ItemGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreType>> GetStoreType(int id)
        {
            var storeType = await _context.StoreType.FindAsync(id);

            if (storeType == null)
            {
                return NotFound();
            }

            return storeType;
        }

        // PUT: api/ItemGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoreType(int id, StoreType storeType)
        {
            if (id != storeType.Id)
            {
                return BadRequest();
            }

            _context.Entry(storeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreTypeExists(id))
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

        // POST: api/ItemGroups
        [HttpPost]
        public async Task<ActionResult<StoreType>> PostStoreType(StoreType storeType)
        {
            _context.StoreType.Add(storeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStoreType", new { id = storeType.Id }, storeType);
        }

        // DELETE: api/ItemGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StoreType>> DeleteStoreType(int id)
        {
            var storeType = await _context.StoreType.FindAsync(id);
            if (storeType == null)
            {
                return NotFound();
            }

            _context.StoreType.Remove(storeType);
            await _context.SaveChangesAsync();

            return storeType;
        }

        private bool StoreTypeExists(int id)
        {
            return _context.StoreType.Any(e => e.Id == id);
        }
    
    }
}