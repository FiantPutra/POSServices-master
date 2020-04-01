using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/GETtransactionMPOS")]
    [ApiController]
    public class GETtransactionMPOSController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public GETtransactionMPOSController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<TransactionMposFor>> getTrans(string store)
        {
            var hariini = DateTime.Now.AddDays(1);
            var seminggu = DateTime.Now.AddDays(-7);
            List<TransactionMposFor> RETURNLIST = new List<TransactionMposFor>();
            List<Models.Transaction> getList = await _context.Transaction.Where(x => x.StoreCode == store && x.TransactionDate >= seminggu && x.TransactionDate <= hariini).ToListAsync();
            foreach (Models.Transaction trans in getList)
            {

                WebAPIModel.TransactionMposFor transactionapi = new WebAPIModel.TransactionMposFor
                {
                    transactionId = trans.TransactionId,
                    date = trans.TransactionDate.Value.ToString("yyyy-MM-dd"),
                    Bank1 = trans.Bank1,
                    Bank2 = trans.Bank2,
                    status = trans.Status.Value,
                    cash = trans.Cash.Value,
                    change = trans.Change.Value,
                    total = trans.TotalAmounTransaction.Value,
                    customerId = "",
                    edc1 = trans.Edc1.Value,
                    edc2 = trans.Edc2.Value,
                    employeeId = trans.EmployeeCode,
                    id = (int)trans.Id,
                    timeStamp = trans.TransactionDate.Value.ToString("MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture), //DateTime.ParseExact(trans.TransactionDate, "MMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    //time = trans.TransactionTime.ToString(),
                    NoRef1 = trans.NoRef1,
                    NoRef2 = trans.NoRef2,
                    currency = "IDR",
                    receiptId = trans.RecieptCode,
                    spgId = trans.Text1,
                    time = trans.TransactionDate.Value.ToString("HH:mm:ss"),
                    discount = trans.TotalDiscount.Value,
                    paymentType = Int32.Parse(trans.MethodOfPayment),
                    openShiftId = trans.ClosingShiftId,
                    openStoreId = trans.ClosingStoreId,
                    customerIdStore = trans.CustomerCode,
                    sequenceNumber = trans.TransactionId.Substring(12, 5), //TR/MCM-1807-00001
                    storeCode = trans.StoreCode,
                    transactionType = trans.TransactionType.Value,
                    voucher = 0
                    //trans.Voucher.ToString()



                };
                List<TransactionLine> translinesAPI = new List<TransactionLine>();
                List<TransactionLines> translines = await _context.TransactionLines.Where(x => x.TransactionId == trans.Id).ToListAsync();

                foreach (TransactionLines TL in translines)
                {
                    TransactionLine tlAPI = new TransactionLine
                    {
                        // articleName = TL.ArticleName,
                        article = new WebAPIModel.Article
                        {
                            articleId = TL.ArticleId,
                            id = _context.Item.Where(x => x.ItemId == TL.ArticleId).First().Id,
                            unit = "PCS",
                            articleIdAlias = TL.ArticleIdAlias,
                            articleName = TL.ArticleName,                            
                            price = TL.UnitPrice
                        },
                        articleIdFk = _context.Item.Where(x => x.ItemId == TL.ArticleId).First().Id,
                        subtotal = TL.Amount,
                        discount = TL.Discount.Value,
                        discountCode = TL.DiscountCode,
                        discountType = TL.DiscountType.Value,
                        id = (int)TL.Id,
                        transactionIdFk = (int)TL.TransactionId,
                        quantity = TL.Qty,
                        spgId = TL.Spgid,
                        transactionId = trans.TransactionId,
                        price = TL.UnitPrice



                    };
                    translinesAPI.Add(tlAPI);
                }

                transactionapi.transactionLines = translinesAPI;


                RETURNLIST.Add(transactionapi);
            }
            return RETURNLIST;
        }
    }
}