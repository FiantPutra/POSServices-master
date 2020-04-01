using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/Transaction")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;


        public TransactionController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }




        // POST: api/PostTransaction
        [HttpPost]
        public IActionResult PostTransaction([FromBody] WebAPIModel.Transaction transactionApi)
        {


            APIResponse response = new APIResponse();
            response.code = "5";
            response.message = "Data Already exist";
            bool exist = _context.Transaction.Any(x => x.TransactionId == transactionApi.transactionId);
            if (!exist)
            {
                try
                {
                    try
                    {
                        LogRecord log = new LogRecord();
                        log.TimeStamp = DateTime.Now;
                        if (transactionApi.status == 1)
                        {
                            log.Tag = "Transaction Pre";
                        }
                        else
                        {
                            log.Tag = "Transaction Pre Void";
                        }
                        log.Message = JsonConvert.SerializeObject(transactionApi);
                        log.TransactionId = transactionApi.transactionId;
                        _context.LogRecord.Add(log);
                        //   _context.SaveChanges();
                    }
                    catch
                    {
                        response.code = "1";
                        response.message = "Data Already Exist";

                        return Ok(response);
                    }


                    //List<Store> liststore = _context.Store.ToList();

                    //var idx1 = storeindex(transactionApi.storeCode, liststore);
                    //Store store = liststore[idx1];


                    Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First();
                    StoreType storetype = _context.StoreType.Where(s => s.Id == store.StoreTypeId).FirstOrDefault();
                    Models.Transaction transaction = new Models.Transaction();



                    transaction.CustomerId = 1;
                    try
                    {
                        Employee emp = _context.Employee.Where(c => c.EmployeeCode == transactionApi.employeeId).First();
                        transaction.EmployeeId = emp.Id;
                        transaction.EmployeeCode = emp.EmployeeCode;
                        transaction.EmployeeName = emp.EmployeeName;
                    }
                    catch
                    {
                        transaction.EmployeeId = 2643;
                    }
                    transaction.MarginTransaction = 0;
                    transaction.MethodOfPayment = transactionApi.paymentType.ToString();
                    transaction.Qty = 0;
                    transaction.RecieptCode = transactionApi.receiptId;
                    transaction.Spgid = 0;
                    transaction.StoreCode = transactionApi.storeCode;
                    transaction.StoreId = store.Id;
                    transaction.Status = transactionApi.status;
                    transaction.ClosingShiftId = transactionApi.openShiftId;
                    transaction.ClosingStoreId = transactionApi.openStoreId;
                    transaction.TransactionId = transactionApi.transactionId;
                    transaction.TotalAmounTransaction = transactionApi.total;
                    transaction.TotalDiscount = transactionApi.discount;
                    transaction.TransactionId = transactionApi.transactionId;
                    transaction.Cash = transactionApi.cash;
                    transaction.Edc1 = transactionApi.Edc1;
                    transaction.Edc2 = transactionApi.Edc2;
                    transaction.Bank1 = transactionApi.Bank1;
                    transaction.Bank2 = transactionApi.Bank2;
                    transaction.NoRef1 = transactionApi.NoRef1;
                    transaction.NoRef2 = transactionApi.NoRef2;
                    transaction.Change = transactionApi.change;
                    try
                    {
                        transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        /*
                        try
                        {
                            transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);

                        }
                        catch {

                            transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                        }
                        */
                        transaction.TransactionDate = DateTime.Now;



                    }
                    DateTime maret19 = DateTime.ParseExact("2019-03-20", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (transaction.TransactionDate < maret19)
                    {
                        response.code = "1";
                        response.message = "Sucess Add Data";
                        return Ok();
                    }
                    try
                    {
                        var tglclosing = _context.ClosingStore.OrderByDescending(x => x.ClosingTimeStamp).Where(x => x.StoreCode == transactionApi.storeCode).First().ClosingTimeStamp;
                        if (tglclosing != DateTime.Now)
                        {
                            transaction.TransDateStore = tglclosing;
                        }
                        else
                        {
                            try
                            {
                                transaction.TransDateStore = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                            }
                            catch
                            {
                                /*
                                try
                                {
                                    transaction.TransDateStore = DateTime.ParseExact(transactionApi.timeStamp, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);

                                }
                                catch
                                {

                                    transaction.TransDateStore = DateTime.ParseExact(transactionApi.timeStamp, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                                }
                                */
                                transaction.TransDateStore = DateTime.Now;
                            }
                        }
                    }
                    catch
                    {

                    }



                    if (storetype.StoreInStore.Value == true)
                    {
                        transaction.TransactionType = Config.RetailEnum.transactionStoreinStore; //true
                    }
                    else
                    {
                        transaction.TransactionType = Config.RetailEnum.transactionStore; //false
                    }
                    try
                    {
                        bool employeeInMaster = _context.Employee.Any(c => c.EmployeeCode == transactionApi.customerId);
                        if (employeeInMaster)
                        {
                            transaction.Text2 = transactionApi.customerId;
                            transaction.TransactionType = Config.RetailEnum.transactionEmployee;
                            transaction.Text3 = transactionApi.customerIdStore;

                        }
                    }
                    catch
                    {

                    }


                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    _context.Transaction.Add(transaction);


                    _context.SaveChanges();

                    // sequenceNumber(transactionApi);
                    //log record


                    //save Lines 
                    for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                    {
                        Models.TransactionLines transactionLines = new Models.TransactionLines();
                        transactionLines.TransactionId = transaction.Id;
                        transactionLines.ArticleId = transactionApi.transactionLines[i].article.articleId;
                        transactionLines.ArticleIdAlias = transactionApi.transactionLines[i].article.articleIdAlias;
                        transactionLines.ArticleName = transactionApi.transactionLines[i].article.articleName;
                        transactionLines.UnitPrice = transactionApi.transactionLines[i].price;
                        transactionLines.Amount = transactionApi.transactionLines[i].subtotal;
                        transactionLines.Discount = transactionApi.transactionLines[i].discount;
                        transactionLines.DiscountCode = transactionApi.transactionLines[i].discountCode;
                        transactionLines.DiscountType = transactionApi.transactionLines[i].discountType;
                        transactionLines.Qty = transactionApi.transactionLines[i].quantity;
                        transactionLines.Spgid = transactionApi.transactionLines[i].spgId;
                        _context.TransactionLines.Add(transactionLines);
                        _context.SaveChanges(); //7k ms


                    }
                    // _context.SaveChanges(); //5k ms
                    response.code = "1";
                    response.message = "Sucess Add Data";

                    inventory(transactionApi).Wait();
                    sequenceNumber(transactionApi);
                    //validate tansaction ke invent
                    /*
                    DateTime maret10 = DateTime.ParseExact("2019-03-20", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (transaction.TransactionDate > maret10)
                    {
                        inventory(transactionApi).Wait();
                        sequenceNumber(transactionApi);
                    }

    

                    //add for infor
                    //remove by frank 
                    //5 september 2019
                    if (transaction.TransactionType == Config.RetailEnum.transactionStoreinStore)
                    {
                        //  WebAPIInforController.InforAPITransactionInStoreController inforAPIController = new WebAPIInforController.InforAPITransactionInStoreController(_context);
                        //  inforAPIController.AddBatchHead(transactionApi, transaction.Id).Wait();
                    }
                    else
                    {
                        //  WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                        //  inforAPIController.postOrder(transactionApi, transaction.Id).Wait();

                    }
                    */
                }
                catch (Exception ex)
                {
                    response.code = "0";
                    response.message = ex.ToString();
                }
            }




            return Ok(response);
            //     return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }


        [HttpPost]
        [Route("post2")]
        public IActionResult PostTransaction([FromBody] WebAPIModel.Transaction transactionApi, string transactionIds)
        {
            LogRecord log = new LogRecord();
            log.TimeStamp = DateTime.Now;
            log.Tag = "Transaction void";
            log.Message = JsonConvert.SerializeObject(transactionApi);
            _context.LogRecord.Add(log);
            _context.SaveChanges();
            APIResponse response = new APIResponse();
            try
            {
                //List<Store> liststore = _context.Store.ToList();

                //var idx1 = storeindex(transactionApi.storeCode, liststore);
                //Store store = liststore[idx1];


                Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First(); // coba
                StoreType storetype = _context.StoreType.Where(s => s.Id == store.StoreTypeId).FirstOrDefault();
                // Models.Transaction transaction = new Models.Transaction();
                Models.Transaction transaction = _context.Transaction.Where(x => x.TransactionId == transactionIds).First();
                transaction.CustomerId = 1;
                try
                {
                    Employee emp = _context.Employee.Where(c => c.EmployeeCode == transactionApi.employeeId).First();
                    transaction.EmployeeId = emp.Id;
                    transaction.EmployeeCode = emp.EmployeeCode;
                    transaction.EmployeeName = emp.EmployeeName;
                }
                catch
                {
                    transaction.EmployeeId = 2;
                }
                transaction.MarginTransaction = 0;
                transaction.MethodOfPayment = transactionApi.paymentType.ToString();
                transaction.Qty = 0;
                transaction.RecieptCode = transactionApi.receiptId;
                transaction.Spgid = 0;
                transaction.Text1 = transactionApi.spgId;
                transaction.StoreCode = transactionApi.storeCode;
                transaction.StoreId = store.Id;
                transaction.Status = transactionApi.status;
                try
                {
                    transaction.CustomerCode = transactionApi.customerIdStore;
                }
                catch
                {
                    transaction.CustomerCode = "";
                }

                transaction.ClosingShiftId = transactionApi.openShiftId;
                transaction.ClosingStoreId = transactionApi.openStoreId;
                transaction.TransactionId = transactionApi.transactionId;
                transaction.TotalAmounTransaction = transactionApi.total;
                transaction.TotalDiscount = transactionApi.discount;
                transaction.TransactionId = transactionApi.transactionId;
                transaction.Cash = transactionApi.cash;
                transaction.Edc1 = transactionApi.Edc1;
                transaction.Edc2 = transactionApi.Edc2;
                transaction.Bank1 = transactionApi.Bank1;
                transaction.Bank2 = transactionApi.Bank2;
                transaction.NoRef1 = transactionApi.NoRef1;
                transaction.NoRef2 = transactionApi.NoRef2;
                transaction.Change = transactionApi.change;
                try
                {
                    transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                }
                catch
                {
                    transaction.TransactionDate = DateTime.ParseExact(transactionApi.date + transactionApi.time, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);
                }
                try
                {
                    var tglclosing = _context.ClosingStore.OrderByDescending(x => x.ClosingTimeStamp).Where(x => x.StoreCode == transactionApi.storeCode).First().ClosingTimeStamp;
                    if (tglclosing != DateTime.Now)
                    {
                        transaction.TransDateStore = tglclosing;
                    }
                    else
                    {
                        try
                        {
                            transaction.TransDateStore = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            transaction.TransDateStore = DateTime.ParseExact(transactionApi.date + transactionApi.time, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);
                        }
                    }
                }
                catch
                {

                }


                if (storetype.StoreInStore.Value == true)
                {
                    transaction.TransactionType = Config.RetailEnum.transactionStoreinStore; //true
                }
                else
                {
                    transaction.TransactionType = Config.RetailEnum.transactionStore; //false
                }
                try
                {
                    bool employeeInMaster = _context.Employee.Any(c => c.EmployeeCode == transactionApi.customerId);
                    if (employeeInMaster)
                    {
                        transaction.Text2 = transactionApi.customerId;
                        transaction.TransactionType = Config.RetailEnum.transactionEmployee;
                    }
                }
                catch
                {

                }
                //if (transaction.StoreCode.Equals("MBA") )
                //{
                //    transaction.TransactionType = Config.RetailEnum.transactionStoreinStore;
                //}
                //else
                //{
                //    transaction.TransactionType = Config.RetailEnum.transactionStore;
                //}



                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Transaction.Update(transaction);
                _context.SaveChanges();
                // sequenceNumber(transactionApi);
                //log record


                //save Lines 
                for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                {
                    Models.TransactionLines transactionLines = new Models.TransactionLines();
                    transactionLines.TransactionId = transaction.Id;
                    transactionLines.ArticleId = transactionApi.transactionLines[i].article.articleId;
                    transactionLines.ArticleIdAlias = transactionApi.transactionLines[i].article.articleIdAlias;
                    transactionLines.ArticleName = transactionApi.transactionLines[i].article.articleName;
                    transactionLines.UnitPrice = transactionApi.transactionLines[i].price;
                    transactionLines.Amount = transactionApi.transactionLines[i].subtotal;
                    transactionLines.Discount = transactionApi.transactionLines[i].discount;
                    transactionLines.DiscountCode = transactionApi.transactionLines[i].discountCode;
                    transactionLines.DiscountType = transactionApi.transactionLines[i].discountType;
                    transactionLines.Qty = transactionApi.transactionLines[i].quantity;
                    transactionLines.Spgid = transactionApi.transactionLines[i].spgId;
                    _context.TransactionLines.Add(transactionLines);
                    _context.SaveChanges(); //7k ms
                }
                // _context.SaveChanges(); //5k ms
                response.code = "1";
                response.message = "Sucess Add Data";
                inventory(transactionApi).Wait();
                // inventory(transactionApi).ConfigureAwait(false);
                sequenceNumber(transactionApi);
                //add for infor
                if (transaction.TransactionType == Config.RetailEnum.transactionStoreinStore)
                {
                    //  WebAPIInforController.InforAPITransactionInStoreController inforAPIController = new WebAPIInforController.InforAPITransactionInStoreController(_context);
                    //  inforAPIController.AddBatchHead(transactionApi, transaction.Id).Wait();
                }
                else
                {
                    //  WebAPIInforController.InforAPIController inforAPIController = new WebAPIInforController.InforAPIController(_context);
                    //  inforAPIController.postOrder(transactionApi, transaction.Id).Wait();

                }

                //coba
                //for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                //{


                //}

            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }



            return Ok(response);
            //     return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        private void sequenceNumber(WebAPIModel.Transaction transactionApi)
        {


            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.SequenceNumber;
            log.LastTransId = transactionApi.transactionId;
            log.Date = DateTime.Now;
            log.TransactionType = "Transaction";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }

        private async Task inventory(WebAPIModel.Transaction transactionApi)
        {
            //transactionApi.storeCode = "AAB";
            for (int i = 0; i < transactionApi.transactionLines.Count; i++)
            {
                InventoryLines inventoryLines = _context.InventoryLines.Where(c => c.WarehouseId == transactionApi.storeCode && c.ItemId == transactionApi.transactionLines[i].article.id).First();
                if (inventoryLines != null)
                {
                    //Remark Sementara
                    //InventoryLinesTransaction transaction = new InventoryLinesTransaction();
                    //transaction.TransactionTypeId = RetailEnum.SalesTransaction;
                    //transaction.TransactionTypeName = "SalesTransaction";
                    //transaction.TransRefId = transactionApi.transactionId;
                    //transaction.Qty = -1 * transactionApi.transactionLines[i].quantity;
                    //transaction.TransactionLinesId = inventoryLines.Id;
                    //try
                    //{
                    //    transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    //}
                    //catch
                    //{
                    //    // transaction.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //    transaction.TransactionDate = DateTime.ParseExact(transactionApi.date + transactionApi.time, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);
                    //}

                    //_context.Add(transaction);
                    //await _context.SaveChangesAsync();
                    ////update qty
                    //inventoryLines.Qty = _context.InventoryLinesTransaction.
                    //                     Where(c => c.TransactionLinesId == inventoryLines.Id)
                    //                    .Select(c => c.Qty)
                    //                    .DefaultIfEmpty()
                    //                    .Sum();
                    //_context.InventoryLinesTransaction.Update(transaction);
                    //await _context.SaveChangesAsync();
                }
            }
        }
    }
}