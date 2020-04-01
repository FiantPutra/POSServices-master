using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/RequestOrder")]
    [ApiController]
    public class RequestOrderController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public RequestOrderController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] RequestOrder transactionApi)
        {
            APIResponse response = new APIResponse();
            try
            {
                Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First();
                Models.InventoryTransaction transaction = new Models.InventoryTransaction();
                transaction.TransactionId = transactionApi.requestOrderId;
                transaction.StoreCode = transactionApi.storeCode;
                transaction.Remarks = "";
                transaction.StoreName = store.Name;
                transaction.TransactionTypeId = RetailEnum.requestTransaction;
                transaction.TransactionTypeName = "Request";
                transaction.WarehouseOriginal = store.WarehouseId;
                transaction.RequestDeliveryDate = DateTime.Now;
                transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                transaction.TotalQty = transactionApi.totalQty;
                transaction.Remarks = "";
                transaction.Sjlama = transactionApi.oldSJ;
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
                log.Tag = "Request Order";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                _context.LogRecord.Add(log);
                _context.InventoryTransaction.Add(transaction);
                await _context.SaveChangesAsync();

                //save Lines 
                for (int i = 0; i < transactionApi.requestOrderLines.Count; i++)
                {
                    Models.InventoryTransactionLines transactionLines = new Models.InventoryTransactionLines();
                    transactionLines.InventoryTransactionId = transaction.Id;
                    transactionLines.ArticleId = transactionApi.requestOrderLines[i].article.articleId;
                    transactionLines.ArticleName = transactionApi.requestOrderLines[i].article.articleName;
                    // transactionLines.price = transactionApi.transactionLines[i].price;
                    transactionLines.Qty = transactionApi.requestOrderLines[i].quantity;
                    // transactionLines.Amount = transactionApi.transactionLines[i].subtotal;
                    _context.InventoryTransactionLines.Add(transactionLines);
                    await _context.SaveChangesAsync();
                }
                response.code = "1";
                response.message = "Sucess Add Data";
                // this.insertAndCalculateRO(transactionApi).Wait();

                WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                inforAPIController.postRequestOrder(transactionApi, transaction.Id).Wait();
                this.sequenceNumber(transactionApi);
            }

            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }
            return Ok(response);
            //     return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        private void sequenceNumber(RequestOrder transactionApi)
        {


            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.requestOrderId;
            log.Date = DateTime.Now;
            log.TransactionType = "Request Order";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
        private async Task insertAndCalculateRO(RequestOrder transactionApi)
        {
            //Remark Sementara
            //for (int i = 0; i < transactionApi.requestOrderLines.Count; i++)
            //{
            //    InventoryLines inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.storeCode && c.ItemId == transactionApi.requestOrderLines[i].article.id).First();
            //    InventoryLinesTransaction transaction = new InventoryLinesTransaction();
            //    transaction.TransactionTypeId = RetailEnum.requestTransaction;
            //    transaction.TransactionTypeName = "Request Order";
            //    transaction.TransRefId = transactionApi.requestOrderId;
            //    transaction.Qty = transactionApi.requestOrderLines[i].quantity;
            //    transaction.TransactionLinesId = inventoryLines.Id;
            //    _context.Add(transaction);
            //    await _context.SaveChangesAsync();

            //    //update qty
            //    inventoryLines.Qty = _context.InventoryLinesTransaction.
            //                         Where(c => c.TransactionLinesId == inventoryLines.Id)
            //                        .Select(c => c.Qty)
            //                        .DefaultIfEmpty()
            //                        .Sum();
            //    _context.InventoryLines.Update(inventoryLines);
            //    await _context.SaveChangesAsync();
            //}
        }
    }
}