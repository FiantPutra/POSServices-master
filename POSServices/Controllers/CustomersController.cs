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
    public class CustomersController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;

        public CustomersController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        //{
        //    return await _context.Customer.ToListAsync();
        //}

        [HttpGet]
        public IActionResult GetCustomer()
        {
            return Ok(from param in _context.Customer.ToList()
                      select new CustomerAPIModel
                      {
                          Id = param.Id,
                          Address = param.Address,
                          Address2 = param.Address2,
                          Address3 = param.Address3,
                          Address4 = param.Address4,
                          CustGroupId = param.CustGroupId,
                          CustId = param.CustId,
                          DefaultCurr = param.DefaultCurr,
                          Email = param.Email,
                          Name = param.Name,
                          PhoneNumber = param.PhoneNumber,
                          StoreId = param.StoreId
                      });
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var param = await _context.Customer.FindAsync(id);

            var model = new CustomerAPIModel()
            {
                Id = param.Id,
                Address = param.Address,
                Address2 = param.Address2,
                Address3 = param.Address3,
                Address4 = param.Address4,
                CustGroupId = param.CustGroupId,
                CustId = param.CustId,
                DefaultCurr = param.DefaultCurr,
                Email = param.Email,
                Name = param.Name,
                PhoneNumber = param.PhoneNumber,
                StoreId = param.StoreId
            };

            return Ok(model);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
