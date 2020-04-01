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
    [Route("api/DeliveryOrder")]
    [ApiController]
    public class DeliveryOrderController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DeliveryOrderController(DB_BIENSI_POSContext context)
        {
            _context = context;

        }
        
        [HttpPut]
        public async Task<IActionResult> Update(DeliveryOrder transactionApi)
        {
            //add for log
            try
            {
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;

                log.Tag = "Pre Confirm DO";

                log.Message = JsonConvert.SerializeObject(transactionApi);
                log.TransactionId = transactionApi.deliveryOrderId;
                _context.LogRecord.Add(log);
                //   _context.SaveChanges();
            }
            catch
            {

            }
            //end log

            APIResponse response = new APIResponse();
            try
            {

                bool statusConfirmed = _context.InventoryTransaction.Any(c => c.TransactionId == transactionApi.deliveryOrderId
                && c.Status == "Confirmed");
                if (statusConfirmed)
                {
                    response.code = "0";
                    response.message = "DO has been confirmed";
                    return Ok(response);
                }

                bool invTransExist = _context.InventoryTransaction.Any(x => x.TransactionId == transactionApi.deliveryOrderId);                                     
                if (invTransExist)
                {
                    var transaction = _context.InventoryTransaction.Where(x => x.TransactionId == transactionApi.deliveryOrderId).First();
                    transaction.Status = "Confirmed";
                    transaction.StatusId = RetailEnum.doStatusConfirmed;
                    transaction.Id = transaction.Id;
                    transaction.SyncDate = DateTime.Now;   
                    transaction.EmployeeCode = transactionApi.employeeId;
                    transaction.EmployeeName = transactionApi.employeeName;
                    
                    try
                    {                        
                        transaction.RequestDeliveryDate = DateTime.ParseExact(transactionApi.deliveryTime, "yyyy-MM-dd", CultureInfo.InvariantCulture); // MPOS
                    }
                    catch
                    {                        
                        //POS                        
                        transaction.RequestDeliveryDate = DateTime.Now;
                    }
                    //log record

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    _context.InventoryTransaction.Update(transaction);
                    _context.SaveChanges();


                    //List<Article> listar = new List<Article>();
                    //save Lines 
                    for (int i = 0; i < transactionApi.deliveryOrderLines.Count; i++)
                    {

                        //var artikelaidifk = _context.Item.Where(x => x.Id == transactionApi.deliveryOrderLines[i].articleIdFk).First().ItemIdAlias;
                        //var itemanme = _context.Item.Where(x => x.Id == transactionApi.deliveryOrderLines[i].articleIdFk).First().Name;
                        //add  and remarkby frank 
                        // 7 april 2019
                        // untuk case yang double lines tidak ada packing number, referencenya beredsarkan id uniq di DO lins

                        /*Models.InventoryTransactionLines transactionLines = await _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == transaction.Id &&
                        c.ArticleId == artikelaidifk
                        && c.PackingNumber == transactionApi.deliveryOrderLines[i].packingNumber).FirstAsync();
                        */                        
                        //end of add end remark by frank

                        //transactionLines.ArticleName = itemanme;
                        //reamakr by frank 1 oktobr
                        // coa request
                        //   bool check = listar.Any(c => c.id == transactionApi.deliveryOrderLines[i].articleIdFk && c.articleIdAlias == transactionApi.deliveryOrderLines[i].packingNumber);
                        //   if (check)
                        //   {
                        //       transactionLines.RecieveQty = 0;
                        //   }
                        //   else
                        //  {                        
                        //  }
                        //end of remark 1 oktoer


                        //Article dupli = new Article
                        //{
                        //    id = transactionApi.deliveryOrderLines[i].articleIdFk,
                        //    articleIdAlias = transactionApi.deliveryOrderLines[i].packingNumber
                        //};
                        //listar.Add(dupli);
                        bool invTransLineExist = _context.InventoryTransactionLines.Any(x => x.Id == transactionApi.deliveryOrderLines[i].id);
                        if (invTransLineExist)
                        {
                            var transactionLines = _context.InventoryTransactionLines.Where(x => x.Id == transactionApi.deliveryOrderLines[i].id).First();
                            transactionLines.RecieveQty = transactionApi.deliveryOrderLines[i].qtyReceive;
                            _context.InventoryTransactionLines.Update(transactionLines);
                            _context.SaveChanges();
                        }                        
                    }

                    response.code = "1";
                    response.message = "Sucess Add Data";

                    //if (transaction.StatusId == RetailEnum.doStatusConfirmed)
                    //{
                    //    this.insertAndCalculateDO(transactionApi);
                    //}
                    //DateTime maret20 = DateTime.ParseExact("2019-02-28", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //if (transaction.TransactionDate >= maret20)
                    //{
                    //    if (transaction.InforBypass == true)
                    //    {


                    //    }
                    //    else
                    //    {
                    //        //   WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                    //        //   inforAPIController.RecieveRequestOrder(transactionApi, transaction.Id).Wait();
                    //    }

                    //    if (transaction.StatusId == RetailEnum.doStatusConfirmed)
                    //    {
                    //        this.insertAndCalculateDO(transactionApi);
                    //    }
                    //}

                }


            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();

                return BadRequest(response);
            }

            return Ok(response);            
        }

        private void insertAndCalculateDO(DeliveryOrder transactionApi)
        {
            for (int i = 0; i < transactionApi.deliveryOrderLines.Count; i++)
            {
                try
                {
                    InventoryLines inventoryLines = new InventoryLines();
                    bool exist = _context.InventoryLines.Any(c => c.WarehouseId == transactionApi.warehouseTo && c.ItemId == transactionApi.deliveryOrderLines[i].articleIdFk);
                    if (exist)
                    {
                        inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.warehouseTo && c.ItemId == transactionApi.deliveryOrderLines[i].articleIdFk).First();
                    }
                    else
                    {
                        inventoryLines = this.createdInventory(transactionApi, i);
                    }

                    //Remark sementara
                    //InventoryTransactionLines transaction = new InventoryTransactionLines();
                    //transaction.TransactionTypeId = RetailEnum.DeliveryOrder;
                    //transaction.TransactionTypeName = "DeliveryOrder";
                    //transaction.TransRefId = transactionApi.deliveryOrderId;
                    //transaction.Qty = transactionApi.deliveryOrderLines[i].qtyReceive;
                    //transaction.TransactionLinesId = inventoryLines.Id;
                    //transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    //_context.Add(transaction);
                    //_context.SaveChanges();

                    ////update qty
                    //inventoryLines.Qty = _context.InventoryTransactionLines.
                    //                     Where(c => c.InventoryTransactionId == inventoryLines.Id)
                    //                    .Select(c => c.Qty)
                    //                    .DefaultIfEmpty()
                    //                    .Sum();
                    //_context.InventoryLines.Update(inventoryLines);
                    //_context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private InventoryLines createdInventory(DeliveryOrder transactionApi, int i)
        {
            InventoryLines inventory = new InventoryLines();
            inventory.WarehouseId = transactionApi.warehouseTo;
            inventory.Qty = 0;
            inventory.ItemId = transactionApi.deliveryOrderLines[i].articleIdFk;
            _context.Add(inventory);
            _context.SaveChanges();
            return inventory;
        }

        [HttpPost]
        public IActionResult Reject([FromBody] DeliveryOrder transactionApi, string status)
        {
            return Ok();
        }
    }
}