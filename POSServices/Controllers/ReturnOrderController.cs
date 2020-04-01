using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Route("api/ReturnOrder")]
    [ApiController]
    public class ReturnOrderController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;


        public ReturnOrderController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }        
        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] ReturnOrder transactionApi)
        {
            APIResponse response = new APIResponse();
            bool exist = _context.InventoryTransaction.Any(c => c.TransactionId == transactionApi.returnOrderId);
            if (exist == false)
            {
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "Return Order";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                log.TransactionId = transactionApi.returnOrderId;
                _context.LogRecord.Add(log);
                _context.SaveChanges();


                DateTime asd;
                try
                {
                    // transaction.RequestDeliveryDate = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    asd = DateTime.ParseExact(transactionApi.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch
                {
                    asd = DateTime.ParseExact(transactionApi.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                DateTime maret10 = DateTime.ParseExact("2019-03-10", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (maret10 >= asd)
                {
                    response.code = "1";
                    response.message = "Sucess Add Data";
                    return Ok(response);
                }


                Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First();
                Models.InventoryTransaction transaction = new Models.InventoryTransaction();

                transaction.TransactionId = transactionApi.returnOrderId;
                transaction.StoreCode = transactionApi.storeCode;
                transaction.Remarks = "-";
                transaction.StoreName = store.Name;
                transaction.TransactionTypeId = RetailEnum.returnTransaction;
                transaction.TransactionTypeName = "Return";
                transaction.WarehouseOriginal = store.WarehouseId;
                transaction.WarehouseDestination = "210";
                transaction.RequestDeliveryDate = DateTime.Now;
                // transaction.SJlama = transactionApi.oldSJ;
                try
                {
                    transaction.TransactionDate = DateTime.Parse(transactionApi.date);
                }
                catch
                {

                }

                //DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                transaction.TotalQty = transactionApi.totalQty;
                transaction.Remarks = transactionApi.remark;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.InventoryTransaction.Add(transaction);
                await _context.SaveChangesAsync();

                // this.insertInventoryReturnOrder(transactionApi).Wait();
                //save Lines 
                for (int i = 0; i < transactionApi.returnOrderLines.Count; i++)
                {
                    Models.InventoryTransactionLines transactionLines = new Models.InventoryTransactionLines();
                    transactionLines.InventoryTransactionId = transaction.Id;
                    transactionLines.ArticleId = transactionApi.returnOrderLines[i].article.articleIdAlias;
                    transactionLines.ArticleName = transactionApi.returnOrderLines[i].article.articleName;
                    transactionLines.Qty = transactionApi.returnOrderLines[i].quantity;
                    // transactionLines.Amount = transactionApi.transactionLines[i].subtotal;
                    _context.InventoryTransactionLines.Add(transactionLines);
                    await _context.SaveChangesAsync();
                }

                response.code = "1";
                response.message = "Sucess Add Data";                
                this.sequenceNumber(transactionApi);
            }
            else
            {
                response.code = "0";
                response.message = "Transacion Number Already Exist";
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "Return Order";
                log.Message = transactionApi.returnOrderId + "Aleader Exist";
                _context.LogRecord.Add(log);
                _context.SaveChanges();
            }
            return Ok(response);
            //     return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        private void sequenceNumber(ReturnOrder transactionApi)
        {
            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.returnOrderId;
            log.Date = DateTime.Now;
            log.TransactionType = "Return Order";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
        private async Task insertInventoryReturnOrder(ReturnOrder transactionApi)
        {
            //Remark Sementara
            //for (int i = 0; i < transactionApi.returnOrderLines.Count; i++)
            //{
            //    InventoryLines inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.storeCode && c.ItemId == transactionApi.returnOrderLines[i].article.id).First();
            //    if (inventoryLines != null)
            //    {
            //        InventoryLinesTransaction transaction = new InventoryLinesTransaction();
            //        transaction.TransactionTypeId = RetailEnum.ReturnOrder;
            //        transaction.TransactionTypeName = "ReturnOrder";
            //        transaction.TransRefId = transactionApi.returnOrderId;
            //        transaction.Qty = -1 * transactionApi.returnOrderLines[i].quantity;
            //        transaction.TransactionLinesId = inventoryLines.Id;
            //        transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //        _context.Add(transaction);
            //        await _context.SaveChangesAsync();

            //        //update qty
            //        inventoryLines.Qty = _context.InventoryLinesTransaction.
            //                             Where(c => c.TransactionLinesId == inventoryLines.Id)
            //                            .Select(c => c.Qty)
            //                            .DefaultIfEmpty()
            //                            .Sum();
            //        _context.InventoryLines.Update(inventoryLines);
            //        await _context.SaveChangesAsync();
            //    }
            //}
        }
    }
}