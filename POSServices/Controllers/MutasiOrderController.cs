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
    [Route("api/MutasiOrder")]
    [ApiController]
    public class MutasiOrderController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public MutasiOrderController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        private int indexttrans(int id, List<InventoryTransaction> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexttransline(long id, List<InventoryTransactionLines> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indextitems(string id, List<Item> indexlist)
        {
            int value = 0;
            for (int i = 0; i < indexlist.Count; i++)
            {
                if (indexlist[i].ItemIdAlias == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        [HttpGet]
        public IActionResult getMutasi(String StoreCode)
        {
            List<InventoryTransactionLines> listtransline = _context.InventoryTransactionLines.ToList();
            List<Item> listitem = _context.Item.ToList();
            List<MutasiOrder> deliveryOrders = new List<MutasiOrder>();
            List<InventoryTransaction> dos = _context.InventoryTransaction.Where(c => c.WarehouseOriginal == StoreCode && c.TransactionTypeId == RetailEnum.MutasiOrderOut && c.InforBypass == true).ToList();

            foreach (InventoryTransaction intrans in dos)
            {
                // tambah ini

                InventoryTransaction inventory = intrans;
                //========
                //Select For Header
                MutasiOrder doObj = new MutasiOrder
                {

                    date = DateTime.Now.ToString("yyyy-MM-dd"),
                    id = inventory.Id,
                    mutasiOrderId = inventory.TransactionId,

                    status = inventory.StatusId.HasValue ? inventory.StatusId.Value : 0,
                    totalQty = inventory.TotalQty.HasValue ? inventory.TotalQty.Value : 0,
                    mutasiFromWarehouse = inventory.WarehouseOriginal,
                    mutasiToWarehouse = inventory.WarehouseDestination,
                    isPbmPbk = inventory.InforBypass.HasValue ? inventory.InforBypass.Value : false,
                    totalAmount = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == inventory.Id).Sum(d => d.ValueSalesPrice),

                };

                List<MutasiOrderLine> deliveryOrderLines = new List<MutasiOrderLine>();
                int sumLines = 0;
                //  List<InventoryTransactionLines> dosLines = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == intrans.Id).ToList();
                List<InventoryTransactionLines> dosLines = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == inventory.Id).ToList();


                foreach (InventoryTransactionLines doObjLines in dosLines)
                {
                    //
                    int idx2 = indexttransline(doObjLines.Id, listtransline);
                    InventoryTransactionLines invenlines = listtransline[idx2];
                    int idx3 = indextitems(invenlines.ArticleId, listitem);
                    Item item = listitem[idx3];



                    MutasiOrderLine doLine = new MutasiOrderLine
                    {
                        id = invenlines.Id,
                        article = new Article
                        {
                            articleId = item.ItemId,
                            articleIdAlias = item.ItemIdAlias,
                            articleName = invenlines.ArticleName
                        },
                        articleIdFk = item.Id,
                        mutasiOrderIdFk = inventory.Id,
                        mutasiOrderId = inventory.TransactionId,
                        quantity = invenlines.Qty.Value


                    };
                    deliveryOrderLines.Add(doLine);
                }

                doObj.totalQty = inventory.TotalQty.Value;
                doObj.mutasiOrderLines = deliveryOrderLines;
                deliveryOrders.Add(doObj);
            }
            return Ok(deliveryOrders);
        }

        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] MutasiOrder transactionApi)
        {
            APIResponse response = new APIResponse();
            try
            {
                if (transactionApi.mutasiFromWarehouse == "" || transactionApi.mutasiFromWarehouse == null || transactionApi.mutasiToWarehouse == "" || transactionApi.mutasiFromWarehouse == null)
                {
                    response.code = "0";
                    response.message = "No Warehouse Origin / Destination, Make sure to fill it.";
                    return BadRequest(response);
                }
                bool exist = _context.InventoryTransaction.Any(c => c.TransactionId == transactionApi.mutasiOrderId);
                if (exist == false)
                {
                    LogRecord log = new LogRecord();
                    log.TimeStamp = DateTime.Now;
                    log.Tag = "Mutasi Order";
                    log.Message = JsonConvert.SerializeObject(transactionApi);
                    log.TransactionId = transactionApi.mutasiOrderId;
                    await _context.LogRecord.AddAsync(log);
                    await _context.SaveChangesAsync();

                    DateTime asd;// = DateTime.Now;
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
                    transaction.TransactionId = transactionApi.mutasiOrderId;
                    transaction.StoreCode = transactionApi.storeCode;
                    transaction.Remarks = transactionApi.remarks;
                    transaction.StoreName = store.Name;
                    transaction.TransactionTypeId = RetailEnum.mutasiTransaction;
                    transaction.TransactionTypeName = "Mutasi";
                    transaction.Status = null;
                    transaction.StatusId = null;
                    transaction.WarehouseOriginal = transactionApi.mutasiFromWarehouse;
                    transaction.WarehouseDestination = transactionApi.mutasiToWarehouse;
                    transaction.RequestDeliveryDate = DateTime.Now;
                    transaction.Sjlama = transactionApi.oldSJ;
                    transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    try
                    {
                        transaction.TransactionDate = DateTime.Parse(transactionApi.date);

                    }
                    catch
                    {

                    }
                    try
                    {

                        transaction.RequestDeliveryDate = DateTime.Parse(transactionApi.requestDeliveryDate);
                    }
                    catch
                    {

                    }
                    transaction.TotalQty = transactionApi.totalQty;
                    transaction.Remarks = "";
                    try
                    {
                        transaction.EmployeeCode = transactionApi.employeeId;
                        transaction.EmployeeName = transactionApi.employeeName;
                    }
                    catch
                    {

                    }
                    //add by frank for mutasi matrix
                    try
                    {

                        Store storeMaster = _context.Store.Where(c => c.Code == transactionApi.mutasiFromWarehouse).First();
                        List<MutasiApproverMatrix> mutasiApproverMatrixs = _context.MutasiApproverMatrix.Where(c => c.StoreMatrix == storeMaster.Id).ToList();
                        MutasiApproverMatrix ms = mutasiApproverMatrixs.Where(c => c.StoreMatrix == storeMaster.Id).First();
                        if (transactionApi.mutasiType == 1)
                        {
                            transaction.EmployeeToApprove = ms.ApproverCity;
                        }
                        else if (transactionApi.mutasiType == 2)
                        {
                            transaction.EmployeeToApprove = ms.ApproverRegional;
                        }
                        else
                        {
                            transaction.EmployeeToApprove = ms.ApproverNational;
                        }

                    }
                    catch
                    {
                        transaction.EmployeeToApprove = "admin123";
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    //log record

                    _context.InventoryTransaction.Add(transaction);
                    _context.SaveChanges();

                    //save Lines 
                    for (int i = 0; i < transactionApi.mutasiOrderLines.Count; i++)
                    {
                        Models.InventoryTransactionLines transactionLines = new Models.InventoryTransactionLines();
                        transactionLines.InventoryTransactionId = transaction.Id;
                        transactionLines.ArticleId = transactionApi.mutasiOrderLines[i].article.articleIdAlias;
                        transactionLines.ArticleName = transactionApi.mutasiOrderLines[i].article.articleName;
                        transactionLines.Qty = transactionApi.mutasiOrderLines[i].quantity;
                        try
                        {
                            transactionLines.ValueSalesPrice = transactionLines.Qty * _context.PriceList.Where(c => c.ItemId == transactionLines.ArticleId).First().SalesPrice;
                        }
                        catch
                        {
                            transactionLines.ValueSalesPrice = 0;
                        }
                        _context.InventoryTransactionLines.Add(transactionLines);
                        _context.SaveChanges();
                    }

                    response.code = "1";
                    response.message = "Sucess Add Data";
                    //Duplace karena di POS sudah kurang, jika rect qty akan bertambah, jika approve tidak perlu kalkulaso
                    this.insertAndCalculateDO(transactionApi);
                    this.sequenceNumber(transactionApi);
                }
                else
                {
                    response.code = "1";
                    response.message = "Transaction Already Exist";
                    LogRecord log = new LogRecord();
                    log.TimeStamp = DateTime.Now;
                    log.Tag = "Mutasi Order";
                    log.Message = transactionApi.mutasiOrderId + "Aleader Exist";
                    _context.LogRecord.Add(log);
                    _context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                response.code = "1";
                response.message = ex.ToString();
            }


            // await _context.SaveChangesAsync();
            return Ok(response);
            //     return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        private void sequenceNumber(MutasiOrder transactionApi)
        {

            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.mutasiOrderId;
            log.Date = DateTime.Now;
            log.TransactionType = "Mutasi Order";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
        private void insertAndCalculateDO(MutasiOrder transactionApi)
        {
            //Remark Sementara
            //for (int i = 0; i < transactionApi.mutasiOrderLines.Count; i++)
            //{
            //    InventoryLines inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.mutasiFromWarehouse && c.ItemId == transactionApi.mutasiOrderLines[i].article.id).First();
            //    InventoryTransactionLines transaction = new InventoryTransactionLines();
            //    transaction.TransactionTypeId = RetailEnum.MutasiOrderOut;
            //    transaction.TransactionTypeName = "Mutasi Out";
            //    transaction.TransRefId = transactionApi.mutasiOrderId;
            //    transaction.Qty = -1 * transactionApi.mutasiOrderLines[i].quantity;
            //    transaction.TransactionLinesId = inventoryLines.Id;
            //    transaction.TransactionDate = DateTime.Now;//DateTime.ParseExact(transactionApi.date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //    _context.Add(transaction);
            //    _context.SaveChanges();

            //    //update qty
            //    inventoryLines.Qty = _context.InventoryTransactionLines.
            //                         Where(c => c.TransactionLinesId == inventoryLines.Id)
            //                        .Select(c => c.Qty)
            //                        .DefaultIfEmpty()
            //                        .Sum();
            //    _context.InventoryLines.Update(inventoryLines);
            //    _context.SaveChanges();
            //}
        }
    }
}