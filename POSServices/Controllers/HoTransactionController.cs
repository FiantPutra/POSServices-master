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
    public class HoTransactionController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        private readonly DB_BIENSI_POSContext _context2;

        public HoTransactionController(DB_BIENSI_POSContext context)
        {
            _context = context;

            _context2 = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            int total = _context.InventoryTransaction.Where(n => n.TransactionTypeId == RetailEnum.hoTransaction).Count();

            var listModel = _context.InventoryTransaction.Where(n => n.TransactionTypeId == RetailEnum.hoTransaction)
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

            var header = _context.InventoryTransaction.Where(n => n.Id == id).FirstOrDefault<InventoryTransaction>();

            if (header.Id == 0)
            {
                response.code = "0";
                response.message = "Transaction does not exists";

                return Ok(response);
            }

            var lines = _context2.InventoryTransactionLines.Where(n => n.InventoryTransactionId == header.Id).ToList();

            return Ok(new
            {
                header = header,
                lines = lines
            });
        }

        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] HOTransaction transactionApi)
        {
            APIResponse response = new APIResponse();
            try
            {
                Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First();
                Models.InventoryTransaction transaction = new Models.InventoryTransaction();
                transaction.TransactionId = transactionApi.transactionId;
                transaction.StoreCode = transactionApi.storeCode;
                transaction.Remarks = "";
                transaction.StoreName = store.Name;
                transaction.TransactionTypeId = RetailEnum.HOTransaction;
                transaction.TransactionTypeName = "HOTransaction";
                transaction.WarehouseOriginal = store.WarehouseId;
                transaction.RequestDeliveryDate = DateTime.Now;
                transaction.TransactionDate = DateTime.Now;

                transaction.Status = "Pending";
                transaction.StatusId = RetailEnum.doStatusPending;

                try
                {
                    transaction.EmployeeCode = transactionApi.employeeId;
                    transaction.EmployeeName = transactionApi.employeeName;
                }
                catch { }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //log record
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "HO Transaction";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                _context.LogRecord.Add(log);
                _context.InventoryTransaction.Add(transaction);
                await _context.SaveChangesAsync();
                /*
                //save Lines 
                for (int i = 0; i < transactionApi.hoTransactionLines.Count; i++)
                {
                    Models.InventoryTransactionLines transactionLines = new Models.InventoryTransactionLines();
                    transactionLines.InventoryTransactionId = transaction.Id;
                    transactionLines.ArticleId = transactionApi.hoTransactionLines[i].article.articleId;
                    transactionLines.ArticleName = transactionApi.hoTransactionLines[i].article.articleName;
                    transactionLines.Qty = transactionApi.hoTransactionLines[i].quantity;
                    _context.InventoryTransactionLines.Add(transactionLines);
                    await _context.SaveChangesAsync();
                }
                */
                response.code = "1";
                response.message = "Sucess Add Data";

                //   WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                //  inforAPIController.postRequestOrder(transactionApi, transaction.Id).Wait();
                this.sequenceNumber(transactionApi);
            }

            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }
            return Ok(response);
        }

        private void sequenceNumber(HOTransaction transactionApi)
        {
            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.transactionId;
            log.Date = DateTime.Now;
            log.TransactionType = "HO Transaction";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InventoryTransaction model)
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
            return _context.InventoryTransaction.Any(e => e.Id == id);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var model = await _context.InventoryTransaction.FindAsync(id);
                if (model == null)
                {
                    return NotFound();
                }

                _context.InventoryTransaction.Remove(model);
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
