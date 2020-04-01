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
    public class CustomerGroupsController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public CustomerGroupsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/CustomerGroups
        [HttpGet]
        public ActionResult GetCustomerGroup()
        {
            return Ok(from param in _context.CustomerGroup.ToList()
                select new CustomerGroupAPIModel
                {   
                    Id = param.Id,
                    Code = param.Code,
                    Description = param.Description
                }); 
        }

        // GET: api/CustomerGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGroup>> GetCustomerGroup(int id)
        {   
            var param = await _context.CustomerGroup.FindAsync(id);

            if (param == null)
            {
                return NotFound();
            }

            var model = new CustomerGroupAPIModel()
            {
                Id = param.Id,
                Code = param.Code,
                Description = param.Description
            };
            
            return Ok(model);
        }

        // PUT: api/CustomerGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerGroup(int id, CustomerGroup customerGroup)
        {
            if (id != customerGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerGroupExists(id))
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

        // POST: api/CustomerGroups
        [HttpPost]
        public async Task<ActionResult<CustomerGroup>> PostCustomerGroup(CustomerGroup customerGroup)
        {
            _context.CustomerGroup.Add(customerGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerGroup", new { id = customerGroup.Id }, customerGroup);
        }

        // DELETE: api/CustomerGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerGroup>> DeleteCustomerGroup(int id)
        {
            var customerGroup = await _context.CustomerGroup.FindAsync(id);
            if (customerGroup == null)
            {
                return NotFound();
            }

            _context.CustomerGroup.Remove(customerGroup);
            await _context.SaveChangesAsync();

            return customerGroup;
        }

        private bool CustomerGroupExists(int id)
        {
            return _context.CustomerGroup.Any(e => e.Id == id);
        }
    }
}
