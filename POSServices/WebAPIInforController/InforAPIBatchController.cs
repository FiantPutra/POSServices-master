using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIInforModel;
using POSServices.WebAPIModel;

namespace POSServices.WebAPIInforController
{    
    public class InforAPIBatchController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public InforAPIBatchController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        public String batchToInfor(string datestart, string dateend, string typetrans)
        // public String batchToInfor(string transId, string store)
        {
            String customerId = "";
            String tempEmployeeCustid = "";
            //List<Store> stores = new List<Store>();

            try
            {
                List<Store> storesList = _context.Store.ToList();
                List<Customer> customerList = _context.Customer.ToList();
                List<StoreType> storeTypesList = _context.StoreType.ToList();
                List<TransactionByPassx> transactionByPasses = _context.TransactionByPassx.ToList();


                for (int xa = Int32.Parse(datestart); xa <= Int32.Parse(dateend); xa++)
                {
                    var hahae = DateTime.ParseExact("11/" + xa.ToString() + "/2019", "M/d/yyyy", CultureInfo.InvariantCulture).ToShortDateString().ToString();
                    List<Customer> custmerList = _context.Customer.ToList();
                    //yang sudah OFO, 
                    List<Models.Transaction> listTransaction = new List<Models.Transaction>();
                    listTransaction = _context.Transaction.Where(c => c.IsSyncInfor != true
                    && c.TransactionType != Config.RetailEnum.transactionEmployee &&
                    c.TransactionDate.Value.ToString("M/d/yyyy") == hahae).ToList();
                    // listTransaction = _context.Transaction.Where(c => c.StoreCode=="OFO" && c.IsSyncInfor ==true).ToList();
                    // listTransaction = _context.Transaction.Where(c => c.TotalAmounTransaction<0 && c.IsSyncInfor != true).ToList();

                    if (listTransaction.Count > 0)
                    {

                        for (int i = 0; i < listTransaction.Count; i++)
                        {

                            WebAPIModel.Transaction transactionApi = new WebAPIModel.Transaction();
                            Models.Transaction t = listTransaction[i];
                            bool byPassTrans = transactionByPasses.Any(c => c.TransactionId == t.TransactionId);
                            if (byPassTrans == false)
                            {
                                var stores = storesList.Where(c => c.Code == t.StoreCode).First().StoreTypeId;
                                StoreType storeType = storeTypesList.Where(c => c.Id == stores).First();

                                transactionApi.storeCode = t.StoreCode;
                                transactionApi.transactionId = t.TransactionId;
                                transactionApi.id = int.Parse(t.Id + "");
                                transactionApi.currency = "IDR";
                                //transactionApi.customerIdStore = "RAAOLS0463";
                                try
                                {
                                    transactionApi.customerIdStore = custmerList.Where(c => c.StoreId == t.StoreId).First().CustId;
                                }
                                catch
                                {
                                    try
                                    {
                                    }
                                    catch
                                    {
                                        transactionApi.customerIdStore = "";
                                    }
                                }

                                transactionApi.total = t.TotalAmounTransaction.Value;
                                transactionApi.change = t.Change.Value;
                                transactionApi.cash = t.Cash.Value;
                                transactionApi.Edc1 = t.Edc1.Value;
                                transactionApi.Edc2 = t.Edc2.Value;
                                transactionApi.Bank1 = t.Bank1;
                                transactionApi.Bank2 = t.Bank2;
                                transactionApi.discount = t.TotalDiscount.Value;
                                transactionApi.transactionType = t.TransactionType.Value;
                                tempEmployeeCustid = t.Text2;

                                List<Models.TransactionLines> transLine = new List<TransactionLines>();
                                List<WebAPIModel.TransactionLine> transLineApi = new List<WebAPIModel.TransactionLine>();
                                transLine = _context.TransactionLines.Where(c => c.TransactionId == t.Id).ToList();
                                if (transLine.Count > 0)
                                {
                                    for (int j = 0; j < transLine.Count; j++)
                                    {
                                        Models.TransactionLines lines = transLine[j];
                                        WebAPIModel.TransactionLine transactionLine = new WebAPIModel.TransactionLine();
                                        transactionLine.subtotal = lines.Amount;
                                        WebAPIModel.Article article = new WebAPIModel.Article();
                                        article.articleId = lines.ArticleId;
                                        article.articleIdAlias = lines.ArticleIdAlias;
                                        article.articleName = lines.ArticleName;
                                        transactionLine.article = article;
                                        transactionLine.quantity = lines.Qty;
                                        transactionLine.discount = lines.Discount.HasValue ? lines.Discount.Value : 0;
                                        transactionLine.discountCode = lines.DiscountCode;
                                        transactionLine.discountType = lines.DiscountType.Value;
                                        transactionLine.price = lines.UnitPrice;
                                        //mofify by frank kareana item price dari pos / mpos masih salah
                                        //mofify per 12 november 2019
                                        /*
                                        if (priceLists.Any(c => c.ItemId == article.articleIdAlias))
                                        {
                                            transactionLine.price = priceLists.Where(c => c.ItemId == article.articleIdAlias).First().SalesPrice;
                                        }
                                        else
                                        {
                                            transactionLine.price = lines.UnitPrice;

                                        }
                                        */
                                        transLineApi.Add(transactionLine);
                                    }

                                    transactionApi.transactionLines = transLineApi;
                                    if (transactionApi.transactionType == Config.RetailEnum.transactionEmployee)
                                    {
                                        this.AddBatchHeadEmploye(transactionApi, transactionApi.id, storeType).Wait();
                                        t.Orno = transactionApi.id.ToString();
                                        t.IsSyncInfor = true;
                                        t.InforSyncDate = DateTime.Now;
                                        _context.Transaction.Update(t);
                                        _context.SaveChanges();
                                    }

                                    else
                                    {
                                        if (storeType.TypeId[0] == 'A' || storeType.TypeId[0] == 'B' || storeType.TypeId.Equals("DA1"))
                                        {
                                            String batchNumber = getRoundNumber();

                                            //ops transaction
                                            this.batchWithPosting(transactionApi, transactionApi.id, batchNumber, listTransaction.Count, i, t, storeType).Wait();
                                            t.Orno = batchNumber;
                                            t.IsSyncInfor = true;
                                            t.InforSyncDate = DateTime.Now;
                                            _context.Transaction.Update(t);
                                            _context.SaveChanges();
                                        }
                                        else if (storeType.TypeId[0] != 'A' || storeType.TypeId[0] != 'B' || !storeType.TypeId.Equals("DA1"))

                                        {
                                            //ois transaction

                                            this.AddBatchHead(transactionApi, transactionApi.id, storeType).Wait();
                                            t.Orno = transactionApi.id.ToString();
                                            t.IsSyncInfor = true;
                                            t.InforSyncDate = DateTime.Now;
                                            _context.Transaction.Update(t);
                                            _context.SaveChanges();

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {


            }



            return customerId;

        }

        public async Task<String> batchWithPosting([FromBody] WebAPIModel.Transaction transaction,
            long id, String roundnumber, int total, int current, Models.Transaction transactionDb, StoreType storeType)
        {
            current = current + 1;
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String roundNumber = "";
            var getdate = _context.Transaction.Where(x => x.TransactionId == transactionDb.TransactionId).First().TransactionDate;
            // var stringdate = getdate.Value.ToString("yyyyMMdd");
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    roundNumber = roundnumber;
                    InforObjPost inforObj = new InforObjPost();
                    inforObj.program = "OPS270MI";
                    List<WebAPIInforModel.Transaction> listTransaction = new List<WebAPIInforModel.Transaction>();
                    for (int i = 0; i < transaction.transactionLines.Count; i++)
                    {
                        WebAPIInforModel.Transaction t = new WebAPIInforModel.Transaction();
                        t.transaction = "AddSlsTicketLin";
                        Record record = new Record();
                        record.CONO = "770";
                        record.DIVI = "AAA";
                        //if total is minus then it's a return transaction
                        if (transaction.total < 0)
                        {
                            record.XRCD = storeType.InforXrcdretur;
                        }
                        else
                        {
                            record.XRCD = storeType.InforXrcdnormal;
                        }

                        record.ITRN = roundNumber;
                        if (transaction.storeCode == "RAA")
                        {
                            record.WHLO = "300";
                        }
                        else
                        {
                            record.WHLO = transaction.storeCode;
                        }
                        record.ORNO = id + 100 + "";
                        record.DLIX = "1";
                        record.PONR = (i + 1) + "";
                        record.POSX = "00";
                        record.CUCD = transaction.currency;
                        record.CUNO = transaction.customerIdStore;
                        record.ITNO = transaction.transactionLines[i].article.articleIdAlias;
                        record.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        record.TRTM = DateTime.Now.ToString("HHmmss");//"113000";
                        record.CUAM = Convert.ToInt32(transaction.transactionLines[i].subtotal) + "";
                        record.VTCD = "10";
                        record.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        record.PYCD = "CSH";
                        record.ALUN = "PCS";
                        record.IVQA = transaction.transactionLines[i].quantity + "";
                        //put discount condition
                        if (transaction.transactionLines[i].discount > 0 || transaction.transactionLines[i].discount < 0)
                        {
                            try
                            {
                                if (transaction.transactionLines[i].discountType == Config.RetailEnum.discountNormal)
                                {
                                    int? discountPercent = _context.DiscountRetail.Where(c => c.DiscountCode == transaction.transactionLines[i].discountCode).First().DiscountPercent;
                                    //  decimal total = transaction.transactionLines[i].price * transaction.transactionLines[i].quantity;
                                    if (discountPercent == 20)
                                    {
                                        record.DIA1 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 25)
                                    {
                                        record.DIA2 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 30)
                                    {
                                        record.DIA3 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 40)
                                    {
                                        record.DIA4 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 50)
                                    {
                                        record.DIA5 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                    else
                                    {
                                        record.DIA6 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                    }
                                }
                                else
                                {
                                    record.DIA6 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                                }
                            }
                            catch
                            {
                                record.DIA6 = record.DIA6 = Convert.ToInt32(Math.Abs(transaction.transactionLines[i].discount)).ToString();
                            }
                        }

                        record.IVNO = "00";
                        record.INYR = DateTime.Now.Year.ToString();
                        record.ARAT = "1";
                        record.CRTP = "1";
                        record.VTP1 = "10";
                        //   record.FACI = "001";
                        t.record = record;
                        listTransaction.Add(t);
                    }

                    //end of add sales ticket line
                    //add sales ticke pay
                    /*
                     * 1. Full Cash
                     * 2. Full EDC
                     * 3. Partial
                     * */
                    //1. Full cash

                    if (transaction.cash != 0 && transaction.Edc1 == 0 && transaction.Edc2 == 0)
                    {
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPay = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPay.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayCash = new Record();
                        recordAddSlsTicketPayCash.CONO = "770";
                        recordAddSlsTicketPayCash.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayCash.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayCash.WHLO = transaction.storeCode;

                        }

                        recordAddSlsTicketPayCash.ITRN = roundNumber;
                        recordAddSlsTicketPayCash.ORNO = id + 100 + "";
                        recordAddSlsTicketPayCash.DLIX = "1";
                        recordAddSlsTicketPayCash.PONR = (transaction.transactionLines.Count + 1) + "";
                        recordAddSlsTicketPayCash.CUCD = transaction.currency;
                        recordAddSlsTicketPayCash.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayCash.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";
                        recordAddSlsTicketPayCash.CUAM = Decimal.ToInt32(transaction.total).ToString().Replace(".", "").Replace(",", ""); // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");
                        recordAddSlsTicketPayCash.CSHC = "CSH";
                        recordAddSlsTicketPayCash.PYCD = "CSH";
                        recordAddSlsTicketPayCash.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayCash.ARAT = "1";
                        recordAddSlsTicketPayCash.CRTP = "1";
                        recordAddSlsTicketPayCash.XRCD = "1990";
                        //  recordAddSlsTicketPayCash.FACI = "001";
                        tRansactionAddSlsTicketPay.record = recordAddSlsTicketPayCash;
                        listTransaction.Add(tRansactionAddSlsTicketPay);
                    }
                    //2. Transaction Full EDC 1
                    else if (transaction.Edc1 != 0 && transaction.cash == 0 && transaction.Edc2 == 0)
                    {
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPay = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPay.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayEDC1 = new Record();
                        recordAddSlsTicketPayEDC1.CONO = "770";
                        recordAddSlsTicketPayEDC1.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayEDC1.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayEDC1.WHLO = transaction.storeCode;

                        }

                        recordAddSlsTicketPayEDC1.ITRN = roundNumber;
                        recordAddSlsTicketPayEDC1.ORNO = id + 100 + "";
                        recordAddSlsTicketPayEDC1.DLIX = "1";
                        recordAddSlsTicketPayEDC1.PONR = (transaction.transactionLines.Count + 1) + "";
                        recordAddSlsTicketPayEDC1.CUCD = transaction.currency;
                        recordAddSlsTicketPayEDC1.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC1.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";
                        recordAddSlsTicketPayEDC1.CUAM = Convert.ToInt32(transaction.Edc1).ToString().Replace(".", "").Replace(",", "") + ""; // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");
                        recordAddSlsTicketPayEDC1.PYCD = transaction.Bank1;
                        recordAddSlsTicketPayEDC1.CSHC = transaction.Bank1;
                        recordAddSlsTicketPayEDC1.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC1.ARAT = "1";
                        recordAddSlsTicketPayEDC1.CRTP = "1";
                        recordAddSlsTicketPayEDC1.XRCD = "1990";
                        // recordAddSlsTicketPayEDC1.FACI = "001";
                        tRansactionAddSlsTicketPay.record = recordAddSlsTicketPayEDC1;
                        listTransaction.Add(tRansactionAddSlsTicketPay);
                    }
                    //3. Transaction Partial cash dan EDC1
                    else if (transaction.Edc1 != 0 && transaction.cash != 0 && transaction.Edc2 == 0)
                    {
                        //add ticket pay for cash
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPay = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPay.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayEDC2 = new Record();
                        recordAddSlsTicketPayEDC2.CONO = "770";
                        recordAddSlsTicketPayEDC2.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayEDC2.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayEDC2.WHLO = transaction.storeCode;

                        }
                        recordAddSlsTicketPayEDC2.ITRN = roundNumber;
                        recordAddSlsTicketPayEDC2.ORNO = id + 100 + "";
                        recordAddSlsTicketPayEDC2.DLIX = "1";
                        recordAddSlsTicketPayEDC2.PONR = (transaction.transactionLines.Count + 1) + "";
                        recordAddSlsTicketPayEDC2.CUCD = transaction.currency;
                        recordAddSlsTicketPayEDC2.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC2.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";
                        recordAddSlsTicketPayEDC2.CUAM = Convert.ToInt32(transaction.cash) + "";


                        // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");
                        recordAddSlsTicketPayEDC2.PYCD = "CSH";
                        recordAddSlsTicketPayEDC2.CSHC = "CSH";
                        recordAddSlsTicketPayEDC2.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC2.ARAT = "1";
                        recordAddSlsTicketPayEDC2.CRTP = "1";
                        recordAddSlsTicketPayEDC2.XRCD = "1990";
                        // recordAddSlsTicketPayEDC2.FACI = "001";
                        tRansactionAddSlsTicketPay.record = recordAddSlsTicketPayEDC2;
                        listTransaction.Add(tRansactionAddSlsTicketPay);

                        //add ticket pay for edc1
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPay2 = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPay2.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayCash2 = new Record();
                        recordAddSlsTicketPayCash2.CONO = "770";
                        recordAddSlsTicketPayCash2.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayCash2.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayCash2.WHLO = transaction.storeCode;
                        }

                        recordAddSlsTicketPayCash2.ITRN = roundNumber;
                        recordAddSlsTicketPayCash2.ORNO = id + 100 + "";
                        recordAddSlsTicketPayCash2.DLIX = "1";
                        recordAddSlsTicketPayCash2.PONR = (transaction.transactionLines.Count + 2) + "";
                        recordAddSlsTicketPayCash2.CUCD = transaction.currency;
                        recordAddSlsTicketPayCash2.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayCash2.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";
                        recordAddSlsTicketPayCash2.CUAM = Decimal.ToInt32(transaction.Edc1).ToString().Replace(".", "").Replace(",", ""); // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");
                        recordAddSlsTicketPayCash2.CSHC = transaction.Bank1;
                        recordAddSlsTicketPayCash2.PYCD = transaction.Bank1;
                        recordAddSlsTicketPayCash2.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayCash2.ARAT = "1";
                        recordAddSlsTicketPayCash2.CRTP = "1";
                        recordAddSlsTicketPayCash2.XRCD = "1990";
                        //  recordAddSlsTicketPayCash.FACI = "001";
                        tRansactionAddSlsTicketPay2.record = recordAddSlsTicketPayCash2;
                        listTransaction.Add(tRansactionAddSlsTicketPay2);
                    }
                    //4. For ED1 dan EDC2
                    else if (transaction.cash == 0 && transaction.Edc1 != 0 && transaction.Edc2 != 0)
                    {
                        //add ticket pay for edc1
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPayEDC1 = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPayEDC1.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayEDC1 = new Record();
                        recordAddSlsTicketPayEDC1.CONO = "770";
                        recordAddSlsTicketPayEDC1.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayEDC1.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayEDC1.WHLO = transaction.storeCode;

                        }

                        recordAddSlsTicketPayEDC1.ITRN = roundNumber;
                        recordAddSlsTicketPayEDC1.ORNO = id + 100 + "";
                        recordAddSlsTicketPayEDC1.DLIX = "1";
                        recordAddSlsTicketPayEDC1.PONR = (transaction.transactionLines.Count + 1) + "";
                        recordAddSlsTicketPayEDC1.CUCD = transaction.currency;
                        recordAddSlsTicketPayEDC1.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC1.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";

                        recordAddSlsTicketPayEDC1.CUAM = Convert.ToInt32(transaction.Edc1).ToString().Replace(".", "").Replace(",", "") + ""; // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");


                        recordAddSlsTicketPayEDC1.PYCD = transaction.Bank1;
                        recordAddSlsTicketPayEDC1.CSHC = transaction.Bank1;
                        recordAddSlsTicketPayEDC1.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC1.ARAT = "1";
                        recordAddSlsTicketPayEDC1.CRTP = "1";
                        recordAddSlsTicketPayEDC1.XRCD = "1990";
                        // recordAddSlsTicketPayEDC1.FACI = "001";
                        tRansactionAddSlsTicketPayEDC1.record = recordAddSlsTicketPayEDC1;
                        listTransaction.Add(tRansactionAddSlsTicketPayEDC1);

                        //for edc 2
                        WebAPIInforModel.Transaction tRansactionAddSlsTicketPayEDC2 = new WebAPIInforModel.Transaction();
                        tRansactionAddSlsTicketPayEDC2.transaction = "AddSlsTicketPay";
                        Record recordAddSlsTicketPayEDC2 = new Record();
                        recordAddSlsTicketPayEDC2.CONO = "770";
                        recordAddSlsTicketPayEDC2.DIVI = "AAA";
                        if (transaction.storeCode == "RAA")
                        {
                            recordAddSlsTicketPayEDC2.WHLO = "300";
                        }
                        else
                        {
                            recordAddSlsTicketPayEDC2.WHLO = transaction.storeCode;

                        }

                        recordAddSlsTicketPayEDC2.ITRN = roundNumber;
                        recordAddSlsTicketPayEDC2.ORNO = id + 100 + "";
                        recordAddSlsTicketPayEDC2.DLIX = "1";
                        recordAddSlsTicketPayEDC2.PONR = (transaction.transactionLines.Count + 2) + "";
                        recordAddSlsTicketPayEDC2.CUCD = transaction.currency;
                        recordAddSlsTicketPayEDC2.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC2.TRTM = DateTime.Now.ToLocalTime().ToString("HHmmss");//"113000";

                        recordAddSlsTicketPayEDC2.CUAM = Convert.ToInt32(transaction.Edc2).ToString().Replace(".", "").Replace(",", "") + ""; // (transaction.total - transaction.discount) +"".Replace(".", "").Replace(",", "");


                        recordAddSlsTicketPayEDC2.PYCD = transaction.Bank2;
                        recordAddSlsTicketPayEDC2.CSHC = transaction.Bank2;
                        recordAddSlsTicketPayEDC2.DUDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordAddSlsTicketPayEDC2.ARAT = "1";
                        recordAddSlsTicketPayEDC2.CRTP = "1";
                        recordAddSlsTicketPayEDC2.XRCD = "1990";
                        // recordAddSlsTicketPayEDC1.FACI = "001";
                        tRansactionAddSlsTicketPayEDC2.record = recordAddSlsTicketPayEDC2;
                        listTransaction.Add(tRansactionAddSlsTicketPayEDC2);



                    }
                    //for auto batch
                    if (current == (total))
                    {
                        //  // transactionDb.VoucherCode = roundNumber;
                        //   _context.Transaction.Update(transactionDb);
                        //   _context.SaveChanges();

                        WebAPIInforModel.Transaction tRansactionAddBatch = new WebAPIInforModel.Transaction();
                        tRansactionAddBatch.transaction = "BchPrcRound";
                        Record recordRoundNumber = new Record();
                        recordRoundNumber.CONO = "770";
                        recordRoundNumber.ITRN = roundNumber;
                        recordRoundNumber.DIVI = "AAA";
                        recordRoundNumber.OPMM = "1";
                        recordRoundNumber.OPSS = "1";
                        recordRoundNumber.OPGL = "1";
                        recordRoundNumber.TRDT = getdate.Value.ToString("yyyyMMdd");//DateTime.Now.ToString("yyyyMMdd");
                        recordRoundNumber.ACDT = DateTime.Now.ToString("yyyyMMdd");
                        tRansactionAddBatch.record = recordRoundNumber;
                        listTransaction.Add(tRansactionAddBatch);
                    }
                    else
                    {

                        WebAPIInforModel.Transaction tRansactionAddBatch = new WebAPIInforModel.Transaction();
                        tRansactionAddBatch.transaction = "BchPrcRound";
                        Record recordRoundNumber = new Record();
                        recordRoundNumber.CONO = "770";
                        recordRoundNumber.ITRN = roundNumber;
                        recordRoundNumber.DIVI = "AAA";
                        recordRoundNumber.OPMM = "1";
                        recordRoundNumber.OPSS = "1";
                        recordRoundNumber.OPGL = "1";
                        recordRoundNumber.TRDT = getdate.Value.ToString("yyyyMMdd");//transaction.date;//DateTime.Now.ToString("yyyyMMdd");
                        recordRoundNumber.ACDT = DateTime.Now.ToString("yyyyMMdd");
                        tRansactionAddBatch.record = recordRoundNumber;
                        listTransaction.Add(tRansactionAddBatch);

                    }


                    inforObj.transactions = listTransaction;


                    HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, inforObj);
                    if (message.IsSuccessStatusCode)
                    {
                        status = message.ToString();
                        var serializer = new DataContractJsonSerializer(typeof(InforObjPostReturn));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforObjPostReturn resultData = serializer.ReadObject(stream) as InforObjPostReturn;
                        //  status = "Return : " + resultData.results[0].errorMessage + "Sukses "+ resultData.nrOfSuccessfullTransactions;
                        //IntegrationLog log = new IntegrationLog();
                        //log.Description = transaction.transactionId;
                        //log.ErrorMessage = resultData.results[0].errorMessage;
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.RefNumber = id + 100 + "";
                        //log.Json = JsonConvert.SerializeObject(inforObj);
                        //log.TransactionType = "OPS270MI Batch";
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }

                    //add sales ticket pay
                    else
                    {
                        status = "Failed : " + message.ToString();
                    }

                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }
            return status;
        }


        public String getRoundNumber()
        {
            String response = "";
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(Config.General.urlInfor + "/OPS270MI/AddRoundNumber").Result;
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(InforRoundNumber));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforRoundNumber resultData = serializer.ReadObject(stream) as InforRoundNumber;
                        try
                        {
                            response = resultData.results[0].records[0].ITRN;
                        }
                        catch (Exception ex)
                        {
                            //IntegrationLog log = new IntegrationLog();
                            //log.Description = "getRoundNumber";
                            //log.ErrorMessage = ex.ToString();
                            //log.NrOfFailedTransactions = 0;
                            //log.NrOfSuccessfullTransactions = 0;
                            //log.NumOfLineSubmited = 0;
                            //log.RefNumber = "No Ref";
                            //log.TransactionType = "Round Number";
                            //_context.IntegrationLog.Add(log);
                            //_context.SaveChanges();
                        }
                    }
                    else
                    {
                        response = "Fail";
                    }
                }
                catch (Exception ex)
                {
                    response = ex.ToString();
                }
                return response;
            }
        }

        //OIS for counter
        public async Task<String> AddBatchHead([FromBody] WebAPIModel.Transaction transactionAPI, long id, StoreType storeType)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String response = "";
            var getdate = _context.Transaction.Where(x => x.TransactionId == transactionAPI.transactionId).First().TransactionDate;
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    InforTransactionInStoreHeader inforObj = new InforTransactionInStoreHeader();
                    inforObj.program = "OIS100MI";

                    List<TransactionInStore> listTransactions = new List<TransactionInStore>();

                    TransactionInStore transactionInStore = new TransactionInStore();
                    transactionInStore.transaction = "AddBatchHead";

                    RecordInStore record = new RecordInStore();
                    record.CONO = "770";
                    record.CUNO = transactionAPI.customerIdStore;

                    //  record.CUNO = transactionAPI.customerIdStore;
                    if (transactionAPI.total < 0)
                    {
                        //  record.ORTP = "R02"; //change by frank as daksa request mappingan.
                        record.ORTP = storeType.InforOrderTypeRetur;
                    }
                    else
                    {
                        record.ORTP = storeType.InforOrderTypeNormal; //A02
                    }

                    record.FACI = "001";
                    record.MODL = "006";
                    record.TEDL = "CS";
                    record.CUOR = id + "";
                    record.ORDT = getdate.Value.ToString("yyyyMMdd");
                    record.CUDT = getdate.Value.ToString("yyyyMMdd");
                    record.RLDT = getdate.Value.ToString("yyyyMMdd");
                    record.RLDZ = getdate.Value.ToString("yyyyMMdd");
                    record.CUCD = transactionAPI.currency;
                    record.CRTP = "1";

                    transactionInStore.record = record;
                    listTransactions.Add(transactionInStore);
                    inforObj.transactions = listTransactions;

                    HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, inforObj);
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(InforTransactionInStoreResult));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforTransactionInStoreResult resultData = serializer.ReadObject(stream) as InforTransactionInStoreResult;

                        try
                        {

                            //IntegrationLog log = new IntegrationLog();
                            //log.Description = "AddBatchHeader - " + transactionAPI.transactionId;
                            //log.ErrorMessage = resultData.results[0].errorMessage;
                            //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                            //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                            //log.NumOfLineSubmited = 0;
                            //if (resultData.nrOfSuccessfullTransactions > 0)
                            //{
                            //    log.RefNumber = resultData.results[0].records[0].ORNO;
                            //    this.AddBatchLine(transactionAPI, id, log.RefNumber, 0, 0, getdate.Value).Wait();
                            //}
                            //log.TransactionType = "OIS100MI";
                            //log.Json = JsonConvert.SerializeObject(inforObj);
                            //_context.IntegrationLog.Add(log);


                            Models.Transaction trans1 = new Models.Transaction();
                            trans1 = _context.Transaction.Where(x => x.TransactionId == transactionAPI.transactionId).First();
                            trans1.Orno = resultData.results[0].records[0].ORNO;
                            _context.Transaction.Update(trans1);
                            //add line

                            _context.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            //IntegrationLog log = new IntegrationLog();
                            //log.Description = "AddBatchHeader";
                            //log.ErrorMessage = "Error While Getting ORNO - " + ex.ToString();
                            //log.NrOfFailedTransactions = 0;
                            //log.NrOfSuccessfullTransactions = 0;
                            //log.NumOfLineSubmited = 0;
                            //log.RefNumber = "No Ref";
                            //log.TransactionType = "OIS100MI";
                            //log.Json = JsonConvert.SerializeObject(inforObj);
                            //_context.IntegrationLog.Add(log);

                            //_context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }
            return status;


        }

        private async Task<String> AddBatchLine([FromBody] WebAPIModel.Transaction transactionAPI, long id, String orno, int total, int current, DateTime getdate)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String response = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    InforTransactionInStoreLine trans = new InforTransactionInStoreLine();
                    trans.program = "OIS100MI";
                    List<TransactionInStoreLine> transactions = new List<TransactionInStoreLine>();
                    for (int i = 0; i < transactionAPI.transactionLines.Count; i++)
                    {
                        TransactionInStoreLine transactionInStoreLine = new TransactionInStoreLine();
                        transactionInStoreLine.transaction = "AddBatchLine";
                        RecordTransactionInStoreLine recordLine = new RecordTransactionInStoreLine();
                        recordLine.CONO = "770";
                        recordLine.ORNO = orno;
                        if (transactionAPI.total < 0)
                        {
                            recordLine.WHSL = "FG0101";
                        }
                        else
                        {
                            recordLine.WHSL = "";
                        }
                        recordLine.ITNO = transactionAPI.transactionLines[i].article.articleIdAlias;
                        recordLine.POPN = transactionAPI.transactionLines[i].article.articleId;
                        recordLine.ORQT = transactionAPI.transactionLines[i].quantity + "";
                        recordLine.WHLO = transactionAPI.storeCode;
                        recordLine.SAPR = Math.Abs(transactionAPI.transactionLines[i].price) + "";
                        recordLine.DWDT = getdate.ToString("yyyyMMdd");
                        recordLine.DWDZ = getdate.ToString("yyyyMMdd");
                        recordLine.DWHZ = getdate.ToString("HHmm");
                        recordLine.CUOR = id + "";
                        recordLine.SPUN = "PCS";
                        if (transactionAPI.transactionLines[i].quantity < 0)
                        {
                            recordLine.EDFP = transactionAPI.transactionLines[i].price * transactionAPI.transactionLines[i].quantity * -1 + "";
                        }
                        else
                        {
                            recordLine.EDFP = transactionAPI.transactionLines[i].price * transactionAPI.transactionLines[i].quantity + "";

                        }
                        recordLine.LTYP = "0";
                        if (transactionAPI.transactionLines[i].discount > 0 || transactionAPI.transactionLines[i].discount < 0)
                        {
                            try
                            {
                                if (transactionAPI.transactionLines[i].discountType == Config.RetailEnum.discountNormal)
                                {
                                    int? discountPercent = _context.DiscountRetail.Where(c => c.DiscountCode == transactionAPI.transactionLines[i].discountCode).First().DiscountPercent;
                                    //  decimal total = transaction.transactionLines[i].price * transaction.transactionLines[i].quantity;
                                    if (discountPercent == 20)
                                    {
                                        recordLine.DIA1 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 25)
                                    {
                                        recordLine.DIA2 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 30)
                                    {
                                        recordLine.DIA3 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 40)
                                    {
                                        recordLine.DIA4 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 50)
                                    {
                                        recordLine.DIA5 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else
                                    {
                                        recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                }
                                else
                                {
                                    recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                }
                            }
                            catch
                            {
                                recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                            }
                        }

                        transactionInStoreLine.record = recordLine;
                        transactions.Add(transactionInStoreLine);
                    }
                    TransactionInStoreLine transactionInStoreAutoPost = new TransactionInStoreLine();
                    transactionInStoreAutoPost.transaction = "Confirm";

                    //will be deleted and change for regular time posted
                    RecordTransactionInStoreLine recordLineAutoPost = new RecordTransactionInStoreLine();
                    recordLineAutoPost.CONO = "770";
                    recordLineAutoPost.ORNO = orno;
                    recordLineAutoPost.CTYP = "Y";
                    transactionInStoreAutoPost.record = recordLineAutoPost;
                    transactions.Add(transactionInStoreAutoPost);
                    trans.transactions = transactions;

                    HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, trans);
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(InforTransactionInStoreResult));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforTransactionInStoreResult resultData = serializer.ReadObject(stream) as InforTransactionInStoreResult;
                        //IntegrationLog log = new IntegrationLog();
                        //log.Description = "AddBatchLine";
                        //if (resultData.nrOfFailedTransactions > 0)
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = (transactionAPI.transactionLines.Count + 1);
                        //log.RefNumber = orno;
                        //log.TransactionType = "OIS100MI";
                        //log.Json = JsonConvert.SerializeObject(trans);
                        //_context.IntegrationLog.Add(log);

                    }
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }
            return status;
        }


        //for employee
        //OIS for counter
        public async Task<String> AddBatchHeadEmploye([FromBody] WebAPIModel.Transaction transactionAPI, long id, StoreType storeType)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String response = "";
            var getdate = _context.Transaction.Where(x => x.TransactionId == transactionAPI.transactionId).First().TransactionDate;
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    InforTransactionInStoreHeader inforObj = new InforTransactionInStoreHeader();
                    inforObj.program = "OIS100MI";

                    List<TransactionInStore> listTransactions = new List<TransactionInStore>();

                    TransactionInStore transactionInStore = new TransactionInStore();
                    transactionInStore.transaction = "AddBatchHead";

                    RecordInStore record = new RecordInStore();
                    record.CONO = "770";
                    record.CUNO = transactionAPI.customerIdStore;
                    record.ORTP = "K02";
                    record.FACI = "001";
                    record.MODL = "006";
                    record.TEDL = "CS";
                    record.CUOR = id + "";
                    record.ORDT = getdate.Value.ToString("yyyyMMdd");
                    record.CUDT = getdate.Value.ToString("yyyyMMdd");
                    record.RLDT = getdate.Value.ToString("yyyyMMdd");
                    record.RLDZ = getdate.Value.ToString("yyyyMMdd");
                    record.CUCD = transactionAPI.currency;
                    record.CRTP = "1";

                    transactionInStore.record = record;
                    listTransactions.Add(transactionInStore);
                    inforObj.transactions = listTransactions;

                    HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, inforObj);
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(InforTransactionInStoreResult));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforTransactionInStoreResult resultData = serializer.ReadObject(stream) as InforTransactionInStoreResult;

                        try
                        {

                            //IntegrationLog log = new IntegrationLog();
                            //log.Description = "AddBatchHeader - " + transactionAPI.transactionId;
                            //log.ErrorMessage = resultData.results[0].errorMessage;
                            //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                            //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                            //log.NumOfLineSubmited = 0;
                            //if (resultData.nrOfSuccessfullTransactions > 0)
                            //{
                            //    log.RefNumber = resultData.results[0].records[0].ORNO;
                            //    this.AddBatchLineEmployee(transactionAPI, id, log.RefNumber, 0, 0, getdate.Value).Wait();
                            //}
                            //log.TransactionType = "OIS100MI";
                            //log.Json = JsonConvert.SerializeObject(inforObj);
                            //_context.IntegrationLog.Add(log);


                            Models.Transaction trans1 = new Models.Transaction();
                            trans1 = _context.Transaction.Where(x => x.TransactionId == transactionAPI.transactionId).First();
                            trans1.Orno = resultData.results[0].records[0].ORNO;
                            _context.Transaction.Update(trans1);
                            //add line

                            _context.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            //IntegrationLog log = new IntegrationLog();
                            //log.Description = "AddBatchHeader Employee";
                            //log.ErrorMessage = "Error While Getting ORNO - " + ex.ToString();
                            //log.NrOfFailedTransactions = 0;
                            //log.NrOfSuccessfullTransactions = 0;
                            //log.NumOfLineSubmited = 0;
                            //log.RefNumber = "No Ref";
                            //log.TransactionType = "OIS100MI";
                            //log.Json = JsonConvert.SerializeObject(inforObj);
                            //_context.IntegrationLog.Add(log);

                            //_context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }
            return status;


        }

        private async Task<String> AddBatchLineEmployee([FromBody] WebAPIModel.Transaction transactionAPI, long id, String orno, int total, int current, DateTime getdate)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String response = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    InforTransactionInStoreLine trans = new InforTransactionInStoreLine();
                    trans.program = "OIS100MI";
                    List<TransactionInStoreLine> transactions = new List<TransactionInStoreLine>();
                    for (int i = 0; i < transactionAPI.transactionLines.Count; i++)
                    {
                        TransactionInStoreLine transactionInStoreLine = new TransactionInStoreLine();
                        transactionInStoreLine.transaction = "AddBatchLine";
                        RecordTransactionInStoreLine recordLine = new RecordTransactionInStoreLine();
                        recordLine.CONO = "770";
                        recordLine.ORNO = orno;
                        if (transactionAPI.total < 0)
                        {
                            recordLine.WHSL = "FG0101";
                        }
                        else
                        {
                            recordLine.WHSL = "";
                        }
                        recordLine.ITNO = transactionAPI.transactionLines[i].article.articleIdAlias;
                        recordLine.POPN = transactionAPI.transactionLines[i].article.articleId;
                        recordLine.ORQT = transactionAPI.transactionLines[i].quantity + "";
                        recordLine.WHLO = transactionAPI.storeCode;
                        recordLine.SAPR = Math.Abs(transactionAPI.transactionLines[i].price) + "";
                        recordLine.DWDT = getdate.ToString("yyyyMMdd");
                        recordLine.DWDZ = getdate.ToString("yyyyMMdd");
                        recordLine.DWHZ = getdate.ToString("HHmm");
                        recordLine.CUOR = id + "";
                        recordLine.SPUN = "PCS";
                        if (transactionAPI.transactionLines[i].quantity < 0)
                        {
                            recordLine.EDFP = transactionAPI.transactionLines[i].price * transactionAPI.transactionLines[i].quantity * -1 + "";
                        }
                        else
                        {
                            recordLine.EDFP = transactionAPI.transactionLines[i].price * transactionAPI.transactionLines[i].quantity + "";

                        }
                        recordLine.LTYP = "0";
                        //add by frank
                        //request by Hendra, 30 Oktober 2019
                        //untuk paycode 


                        if (transactionAPI.cash != 0 && transactionAPI.Edc1 == 0)
                        {//1. Full cash
                            recordLine.TEPY = "CSH";
                        }
                        else if (transactionAPI.cash == 0 && transactionAPI.Edc1 != 0)
                        {
                            //bank 1
                            recordLine.TEPY = transactionAPI.Bank1;
                        }
                        else
                        {

                            recordLine.TEPY = "CSH";
                        }


                        if (transactionAPI.transactionLines[i].discount > 0 || transactionAPI.transactionLines[i].discount < 0)
                        {
                            try
                            {
                                if (transactionAPI.transactionLines[i].discountType == Config.RetailEnum.discountNormal)
                                {
                                    int? discountPercent = _context.DiscountRetail.Where(c => c.DiscountCode == transactionAPI.transactionLines[i].discountCode).First().DiscountPercent;
                                    //  decimal total = transaction.transactionLines[i].price * transaction.transactionLines[i].quantity;
                                    if (discountPercent == 20)
                                    {
                                        recordLine.DIA1 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 25)
                                    {
                                        recordLine.DIA2 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 30)
                                    {
                                        recordLine.DIA3 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 40)
                                    {
                                        recordLine.DIA4 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else if (discountPercent == 50)
                                    {
                                        recordLine.DIA5 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                    else
                                    {
                                        recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                    }
                                }
                                else
                                {
                                    recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                                }




                            }
                            catch
                            {
                                recordLine.DIA6 = Convert.ToInt32(Math.Abs(transactionAPI.transactionLines[i].discount)).ToString();
                            }
                        }

                        transactionInStoreLine.record = recordLine;
                        transactions.Add(transactionInStoreLine);
                    }
                    TransactionInStoreLine transactionInStoreAutoPost = new TransactionInStoreLine();
                    transactionInStoreAutoPost.transaction = "Confirm";

                    //will be deleted and change for regular time posted
                    RecordTransactionInStoreLine recordLineAutoPost = new RecordTransactionInStoreLine();
                    recordLineAutoPost.CONO = "770";
                    recordLineAutoPost.ORNO = orno;
                    recordLineAutoPost.CTYP = "Y";
                    transactionInStoreAutoPost.record = recordLineAutoPost;
                    transactions.Add(transactionInStoreAutoPost);
                    trans.transactions = transactions;

                    HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, trans);
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(InforTransactionInStoreResult));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforTransactionInStoreResult resultData = serializer.ReadObject(stream) as InforTransactionInStoreResult;
                        //IntegrationLog log = new IntegrationLog();
                        //log.Description = "AddBatchLine";
                        //if (resultData.nrOfFailedTransactions > 0)
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = (transactionAPI.transactionLines.Count + 1);
                        //log.RefNumber = orno;
                        //log.TransactionType = "OIS100MI";
                        //log.Json = JsonConvert.SerializeObject(trans);
                        //_context.IntegrationLog.Add(log);

                    }
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }
            return status;
        }
    }
}