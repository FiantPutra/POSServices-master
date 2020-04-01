using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POSServices.Controllers
{
    [Route("api/[controller]")]
    public class HoTransactionLineController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public HoTransactionLineController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int headerId, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            int total = _context.InventoryTransactionLines.Where(n => n.InventoryTransactionId == headerId).Count();

            var listModel = _context.InventoryTransactionLines.Where(n => n.InventoryTransactionId == headerId)
                .OrderByDescending(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return Ok(new
            {
                Data = listModel,
                Paging = new
                {
                    Total = total,
                    Limit = 50,
                    Offset = 0,
                    Returned = listModel.Count
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            APIResponse response = new APIResponse();
            if (id == 0)
            {
                response.code = "0";
                response.message = "ID can not be empty";

                return Ok(response);
            }

            var header = _context.InventoryTransactionLines.Where(n => n.Id == id).FirstOrDefault<InventoryTransactionLines>();

            if (header.Id == 0)
            {
                response.code = "0";
                response.message = "Transaction does not exists";

                return Ok(response);
            }

            return Ok(header);
        }

        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] InventoryTransactionLines transactionApi)
        {
            APIResponse response = new APIResponse();
            try
            {
                Models.InventoryTransactionLines transactionLines = new Models.InventoryTransactionLines();
                transactionLines.InventoryTransactionId = transactionApi.InventoryTransactionId;
                transactionLines.ArticleId = transactionApi.ArticleId;
                transactionLines.ArticleName = transactionApi.ArticleName;
                transactionLines.Qty = transactionApi.Qty;
                _context.InventoryTransactionLines.Add(transactionLines);
                await _context.SaveChangesAsync();

                response.code = "1";
                response.message = "Sucess Add Data";
            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }
            return Ok(response);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InventoryTransactionLines model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdExists(id))
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

        private bool IdExists(int id)
        {
            return _context.InventoryTransactionLines.Any(e => e.Id == id);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var model = await _context.InventoryTransactionLines.FindAsync(id);
                if (model == null)
                {
                    return NotFound();
                }

                _context.InventoryTransactionLines.Remove(model);
                await _context.SaveChangesAsync();

                response.code = "1";
                response.message = "Sucess Delete Data";
            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }

            return Ok(response);
        }
    }
}
