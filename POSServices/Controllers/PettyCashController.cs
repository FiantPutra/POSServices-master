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
using POSServices.WebAPIInforController;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/PettyCash")]
    [ApiController]
    public class PettyCashController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public PettyCashController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        // POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] PettyCash transactionApi)
        {
            int orno = 0;
            //insert storeCode
            bool isStoreExist = _context.ExpenseStore.Any(c => c.StoreCode == transactionApi.storeCode);
            if (!isStoreExist)
            {
                ExpenseStore expenseStore = new ExpenseStore();
                expenseStore.StoreCode = transactionApi.storeCode;
                expenseStore.RemaingBudget = 0;
                expenseStore.TotalExpense = 0;
                expenseStore.Year = DateTime.Now.Year;
                _context.ExpenseStore.Add(expenseStore);
            }
            APIResponse response = new APIResponse();
            try
            {
                //perbaiki transaksi petty cash
                //add more unit;
                orno = _context.Expense.Count() + transactionApi.pettyCashLine.Count();
                InforAPIPettyCash inforAPIPettyCash = new InforAPIPettyCash(_context);
                String itrn = inforAPIPettyCash.getRoundNumber();
                foreach (PettyCashLine pl in transactionApi.pettyCashLine)
                {
                    Expense expense = new Expense();
                    expense.Amount = (pl.price * pl.quantity);
                    expense.CostCategoryId = transactionApi.expenseCategoryId;
                    expense.CostCategoryName = transactionApi.expenseCategory;
                    expense.ExpenseName = pl.expenseName;
                    expense.StoreCode = transactionApi.storeCode;
                    expense.Price = pl.price;
                    expense.Qty = pl.quantity;
                    expense.TypeId = Config.RetailEnum.expenseExpense;
                    expense.TypeName = "Expense";
                    expense.Orno = "PC" + orno;
                    expense.Itrn = itrn;
                    try
                    {
                        expense.TransactionDate = DateTime.ParseExact(transactionApi.timeStamp, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        expense.TransactionDate = DateTime.ParseExact(transactionApi.date + transactionApi.time, "yyyy-MM-dd" + "H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    _context.Add(expense);
                    _context.SaveChanges();

                }
                //log record

                this.updateExpense(transactionApi.storeCode);
                //post to infor
                inforAPIPettyCash.postPettyCash(transactionApi, orno, itrn).Wait();

                response.code = "1";
                response.message = "Sucess Add Data";

                this.sequenceNumber(transactionApi);
            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }
            LogRecord log = new LogRecord();
            log.TimeStamp = DateTime.Now;
            log.Tag = "Petty Cash";
            log.Message = JsonConvert.SerializeObject(transactionApi);
            _context.LogRecord.Add(log);
            _context.SaveChanges();

            return Ok(response);
        }

        private void sequenceNumber(PettyCash transactionApi)
        {
            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.pettyCashId;
            log.Date = DateTime.Now;
            log.TransactionType = "Petty Cash";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
        private void updateExpense(String storeCode)
        {

            ExpenseStore store = _context.ExpenseStore.Where(c => c.StoreCode == storeCode).First();
            store.TotalExpense = _context.Expense.
                                         Where(c => c.StoreCode == storeCode && c.TypeId == Config.RetailEnum.expenseExpense)
                                        .Select(c => c.Amount)
                                        .DefaultIfEmpty()
                                        .Sum();
            store.RemaingBudget = store.TotalBudget - store.TotalExpense;
            _context.ExpenseStore.Update(store);
            // _context.SaveChanges();
        }
    }
}