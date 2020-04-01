using System;
using System.Collections.Generic;
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
    public class InforAPIController : Controller
    {
        String userName = "wcsdomain\\infor6";
        String password = "P@$$W0rd";

        private readonly DB_BIENSI_POSContext _context;
        public InforAPIController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }        
        
        public async Task<String> postRequestOrder([FromBody] RequestOrder transaction, long id)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //add for DO Header
                    InforDeliveryOrderPOST inforObj = new InforDeliveryOrderPOST();
                    inforObj.program = "MHS850MI";
                    List<TransactionDeliveryPOST> listTransaction = new List<TransactionDeliveryPOST>();
                    for (int i = 0; i < transaction.requestOrderLines.Count; i++)
                    {
                        TransactionDeliveryPOST t = new TransactionDeliveryPOST();
                        t.transaction = "AddDO";
                        RecordDeliveryPOST record = new RecordDeliveryPOST();
                        record.PRMD = "*EXE";
                        record.CONO = "770";
                        record.WHLO = "200";
                        record.E0PA = "FSH";
                        record.E0PB = "FSH";
                        record.E065 = "FSH";
                        record.CUNO = _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.storeCode).First().Id).First().CustId;
                        record.ITNO = transaction.requestOrderLines[i].article.articleIdAlias;
                        record.WHSL = "FG0101";
                        record.TWSL = "FG0101";
                        record.ALQT = transaction.requestOrderLines[i].quantity + "";
                        record.DLQT = transaction.requestOrderLines[i].quantity + "";
                        record.TRTP = Config.RetailEnum.RequestOrderEnum;
                        record.USD4 = "PCS";
                        record.RESP = "INFOR";
                        record.RIDL = (i + 1) + "";
                        record.RIDN = id + "";
                        t.record = record;
                        listTransaction.Add(t);
                        //     break;
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
                        //log.Description = transaction.requestOrderId;
                        //try
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //catch
                        //{
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //if (log.NrOfSuccessfullTransactions > 0)
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //else
                        //{

                        //    log.RefNumber = id + "";
                        //}
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.TransactionType = "MHS850MI";
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }
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
        public async Task<String> postReturn([FromBody] ReturnOrder transaction, long id)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //add for DO Header
                    InforDeliveryOrderPOST inforObj = new InforDeliveryOrderPOST();
                    inforObj.program = "MHS850MI";
                    List<TransactionDeliveryPOST> listTransaction = new List<TransactionDeliveryPOST>();
                    for (int i = 0; i < transaction.returnOrderLines.Count; i++)
                    {
                        TransactionDeliveryPOST t = new TransactionDeliveryPOST();
                        t.transaction = "AddDO";
                        RecordDeliveryPOST record = new RecordDeliveryPOST();
                        record.PRMD = "*EXE";
                        record.CONO = "770";
                        record.WHLO = transaction.storeCode;
                        record.E0PA = "FSH";
                        record.E0PB = "FSH";
                        record.E065 = "FSH";
                        record.GEDT = transaction.date.Replace("-", "");
                        record.CUNO = "210";// _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.storeCode).First().Id).First().CustId; 
                        record.ITNO = transaction.returnOrderLines[i].article.articleIdAlias;
                        record.WHSL = "FG0101";
                        record.TWSL = "TOKO";//transaction.storeCode;
                        record.ALQT = transaction.returnOrderLines[i].quantity + "";
                        record.DLQT = transaction.returnOrderLines[i].quantity + "";
                        record.TRTP = Config.RetailEnum.ReturnOrderEnum;
                        record.USD4 = "PCS";
                        record.RTDT = transaction.date.Replace("-", "");
                        record.RPDT = transaction.date.Replace("-", "");
                        record.RESP = "INFOR";
                        record.RIDL = (i + 1) + "";
                        if (record.RIDL == "1")
                        {
                            if (transaction.remark.Length <= 10)
                            {

                                record.RORN = transaction.remark;
                            }
                            else
                            {
                                record.RORN = transaction.remark.ToString().Substring(0, 9);
                            }
                        }
                        else
                        {
                            record.RORN = "";
                        }

                        record.RIDN = id + "";
                        t.record = record;
                        listTransaction.Add(t);
                        //    break;
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
                        //log.Description = transaction.returnOrderId;
                        //try
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //catch
                        //{
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //if (log.NrOfSuccessfullTransactions > 0)
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //else
                        //{

                        //    log.RefNumber = id + "";
                        //}
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.TransactionType = "MHS850MI";
                        //log.Json = JsonConvert.SerializeObject(inforObj);
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }

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
        public async Task<String> postMutasi([FromBody] DeliveryOrderApproval transaction, long id)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //add for DO Header
                    InforDeliveryOrderPOST inforObj = new InforDeliveryOrderPOST();
                    inforObj.program = "MHS850MI";
                    List<TransactionDeliveryPOST> listTransaction = new List<TransactionDeliveryPOST>();
                    for (int i = 0; i < transaction.deliveryOrderLines.Count; i++)
                    {
                        TransactionDeliveryPOST t = new TransactionDeliveryPOST();
                        t.transaction = "AddDO";
                        RecordDeliveryPOST record = new RecordDeliveryPOST();
                        record.PRMD = "*EXE";
                        record.CONO = "770";
                        if (transaction.storeCode == transaction.warehouseFrom)
                        {
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            //record.CUNO = _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.mutasiToWarehouse).First().Id).First().CustId;
                            record.CUNO = transaction.warehouseTo;
                        }
                        else if (transaction.storeCode == transaction.warehouseTo)
                        {
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            //record.CUNO = _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.mutasiToWarehouse).First().Id).First().CustId;
                            record.CUNO = transaction.warehouseTo;
                        }
                        else
                        {
                            //adding for HO Transaction
                            //add on 31 Oktober 2019
                            //by frank
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            record.CUNO = transaction.warehouseTo;

                        }
                        record.E0PA = "FSH";
                        record.E0PB = "FSH";
                        record.E065 = "FSH";
                        record.ITNO = transaction.deliveryOrderLines[i].article.articleIdAlias;
                        record.WHSL = "FG0101";
                        record.TWSL = "FG0101";
                        record.ALQT = transaction.deliveryOrderLines[i].qtyReceive + "";
                        record.DLQT = transaction.deliveryOrderLines[i].qtyReceive + "";
                        record.TRTP = Config.RetailEnum.MutasiOrderEnum;
                        record.USD4 = "PCS";
                        record.GEDT = transaction.deliveryDate;
                        record.RPDT = transaction.deliveryDate;  //DateTime.Now.ToString("ddMMyy");
                        record.RESP = "INFOR";
                        record.RIDL = (i + 1) + "";
                        record.RIDN = id + "";
                        // record.RORN = transaction.remarks;
                        t.record = record;
                        listTransaction.Add(t);
                        //    break;
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
                        //log.Description = transaction.deliveryOrderId;
                        //try
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //catch
                        //{
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //if (log.NrOfSuccessfullTransactions > 0)
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //else
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.TransactionType = "MHS850MI";
                        //log.Json = JsonConvert.SerializeObject(inforObj);
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }

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
        public async Task<String> postHOTransction([FromBody] DeliveryOrderApproval transaction, long id)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //add for DO Header
                    InforDeliveryOrderPOST inforObj = new InforDeliveryOrderPOST();
                    inforObj.program = "MHS850MI";
                    List<TransactionDeliveryPOST> listTransaction = new List<TransactionDeliveryPOST>();
                    for (int i = 0; i < transaction.deliveryOrderLines.Count; i++)
                    {
                        TransactionDeliveryPOST t = new TransactionDeliveryPOST();
                        t.transaction = "AddDO";
                        RecordDeliveryPOST record = new RecordDeliveryPOST();
                        record.PRMD = "*EXE";
                        record.CONO = "770";
                        if (transaction.storeCode == transaction.warehouseFrom)
                        {
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            //record.CUNO = _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.mutasiToWarehouse).First().Id).First().CustId;
                            record.CUNO = transaction.warehouseTo;
                        }
                        else if (transaction.storeCode == transaction.warehouseTo)
                        {
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            //record.CUNO = _context.Customer.Where(c => c.StoreId == _context.Store.Where(d => d.Code == transaction.mutasiToWarehouse).First().Id).First().CustId;
                            record.CUNO = transaction.warehouseTo;
                        }
                        else
                        {
                            //adding for HO Transaction
                            //add on 31 Oktober 2019
                            //by frank
                            record.WHLO = transaction.warehouseFrom; // AAAB : to warehose
                            record.CUNO = transaction.warehouseTo;

                        }
                        record.E0PA = "FSH";
                        record.E0PB = "FSH";
                        record.E065 = "FSH";
                        record.ITNO = transaction.deliveryOrderLines[i].article.articleIdAlias;
                        record.WHSL = "FG0101";
                        record.TWSL = "FG0101";
                        record.ALQT = transaction.deliveryOrderLines[i].qtyReceive + "";
                        record.DLQT = transaction.deliveryOrderLines[i].qtyReceive + "";
                        record.TRTP = transaction.transactionHOType;
                        record.USD4 = "PCS";
                        record.GEDT = transaction.deliveryDate;
                        record.RPDT = transaction.deliveryDate;  //DateTime.Now.ToString("ddMMyy");
                        record.RESP = "INFOR";
                        record.RIDL = (i + 1) + "";
                        record.RIDN = id + "";
                        // record.RORN = transaction.remarks;
                        t.record = record;
                        listTransaction.Add(t);
                        //    break;
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
                        //log.Description = transaction.deliveryOrderId;
                        //try
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //catch
                        //{
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //if (log.NrOfSuccessfullTransactions > 0)
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //else
                        //{
                        //    log.RefNumber = id + "";
                        //}
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.TransactionType = "MHS850MI";
                        //log.Json = JsonConvert.SerializeObject(inforObj);
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }

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

        //public async Task<String> RecieveRequestOrder([FromBody] DeliveryOrder transaction, int id)
        //{
        //    //this.getDeliveryOrderIdLines(transaction.deliveryOrderId, id);
        //    var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
        //    var handler = new HttpClientHandler { Credentials = credentials };
        //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        //    String status = "";
        //    List<Item> items = _context.Item.ToList();
        //    using (var client = new HttpClient(handler))
        //    {
        //        // Make your request...
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        try
        //        {
        //            //add for DO Header
        //            InforDeliveryReciepts inforObj = new InforDeliveryReciepts();
        //            inforObj.program = "MHS850MI";
        //            List<TransactionDeliveryReciepts> listTransaction = new List<TransactionDeliveryReciepts>();
        //            for (int i = 0; i < transaction.deliveryOrderLines.Count; i++)
        //            {
        //                var artikelaidifk = items.Where(x => x.Id == transaction.deliveryOrderLines[i].articleIdFk).First().ItemIdAlias;

        //                //InventoryTransactionLines transactionLines = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == id
        //                //&& c.ArticleId == artikelaidifk && c.PackingNumber == transaction.deliveryOrderLines[i].packingNumber).First();
        //                Models.InventoryTransactionLines transactionLines = _context.InventoryTransactionLines.Find(transaction.deliveryOrderLines[i].id);
        //                TransactionDeliveryReciepts t = new TransactionDeliveryReciepts();
        //                t.transaction = "AddDOReceipt";
        //                RecordDeliveryReciepts record = new RecordDeliveryReciepts();
        //                record.PRFL = "*EXE";
        //                record.WHLO = transaction.warehouseTo;
        //                record.TWHL = "200";
        //                record.E0PA = "FSH";
        //                record.E065 = "FSH";
        //                record.ITNO = artikelaidifk;//transaction.deliveryOrderLines[i].article.articleIdAlias;
        //                record.WHSL = "FG0101";
        //                record.QTY = transaction.deliveryOrderLines[i].qtyReceive + "";
        //                record.RIDN = transactionLines.Urridn + "";
        //                record.RIDL = transactionLines.Urridl + "";
        //                record.RIDI = transactionLines.Urdlix + "";
        //                record.PACN = transactionLines.PackingNumber;
        //                t.record = record;
        //                int qtyDelivered = transaction.deliveryOrderLines[i].qtyDeliver.Value;
        //                int qtyRecieved = transaction.deliveryOrderLines[i].qtyReceive.Value;

        //                if (qtyRecieved == qtyDelivered && !record.PACN.Equals("0"))
        //                {
        //                    listTransaction.Add(t);
        //                }
        //            }

        //            inforObj.transactions = listTransaction;
        //            HttpResponseMessage message = await client.PostAsJsonAsync(Config.General.urlInfor, inforObj);
        //            if (message.IsSuccessStatusCode)
        //            {
        //                status = message.ToString();
        //                var serializer = new DataContractJsonSerializer(typeof(InforObjPostReturn));
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                byte[] byteArray = Encoding.UTF8.GetBytes(result);
        //                MemoryStream stream = new MemoryStream(byteArray);
        //                InforObjPostReturn resultData = serializer.ReadObject(stream) as InforObjPostReturn;
        //                //  status = "Return : " + resultData.results[0].errorMessage + "Sukses "+ resultData.nrOfSuccessfullTransactions;
        //                //IntegrationLog log = new IntegrationLog();
        //                //log.Description = transaction.deliveryOrderId;
        //                //try
        //                //{
        //                //    log.ErrorMessage = resultData.results[0].errorMessage;
        //                //}
        //                //catch
        //                //{
        //                //}
        //                //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
        //                //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
        //                //if (log.NrOfSuccessfullTransactions > 0)
        //                //{
        //                //    log.RefNumber = transaction.deliveryOrderId + "";
        //                //}
        //                //else
        //                //{

        //                //    log.RefNumber = transaction.deliveryOrderId + "";
        //                //}
        //                //log.NumOfLineSubmited = listTransaction.Count;
        //                //log.TransactionType = "MHS850MI - AddDOReceipt";
        //                //log.Json = JsonConvert.SerializeObject(inforObj);
        //                //_context.IntegrationLog.Add(log);
        //                //_context.SaveChanges();
        //            }

        //            else
        //            {
        //                status = "Failed : " + message.ToString();
        //            }
        //            //add for DO Lines
        //        }
        //        catch (Exception ex)
        //        {
        //            status = ex.ToString();
        //        }
        //    }
        //    return status;
        //}

        public async Task<String> RecieveRequestOrderManual([FromBody] DeliveryOrderApproval transaction, int id)
        {
            //this.getDeliveryOrderIdLines(transaction.deliveryOrderId, id);
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //add for DO Header
                    InforDeliveryReciepts inforObj = new InforDeliveryReciepts();
                    inforObj.program = "MHS850MI";
                    List<TransactionDeliveryReciepts> listTransaction = new List<TransactionDeliveryReciepts>();
                    for (int i = 0; i < transaction.deliveryOrderLines.Count; i++)
                    {
                        // var artikelaidifk = _context.Item.Where(x => x.Id == transaction.deliveryOrderLines[i].articleIdFk).First().ItemIdAlias;

                        //InventoryTransactionLines transactionLines = _context.InventoryTransactionLines.Where(c => c.InventoryTransactionId == id
                        //&& c.ArticleId == transaction.deliveryOrderLines[i].article.articleIdAlias && c.PackingNumber == transaction.deliveryOrderLines[i].packingNumber).First();
                        Models.InventoryTransactionLines transactionLines = _context.InventoryTransactionLines.Find(transaction.deliveryOrderLines[i].id);
                        TransactionDeliveryReciepts t = new TransactionDeliveryReciepts();
                        t.transaction = "AddDOReceipt";
                        RecordDeliveryReciepts record = new RecordDeliveryReciepts();
                        record.PRFL = "*EXE";
                        record.WHLO = transaction.warehouseTo;
                        record.TWHL = "200";
                        record.E0PA = "FSH";
                        record.E065 = "FSH";
                        record.ITNO = transaction.deliveryOrderLines[i].article.articleIdAlias;//transaction.deliveryOrderLines[i].article.articleIdAlias;
                        record.WHSL = "FG0101";
                        record.QTY = transaction.deliveryOrderLines[i].qtyReceive + "";
                        record.RIDN = transaction.transactionId + "";

                        record.RIDL = (transactionLines.Urridl.HasValue ? transactionLines.Urridl.Value : 0) + "";
                        record.RIDI = (transactionLines.Urdlix.HasValue ? transactionLines.Urdlix.Value : 0) + "";
                        record.PACN = transactionLines.PackingNumber;
                        t.record = record;
                        listTransaction.Add(t);
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
                        //log.Description = transaction.deliveryOrderId;
                        //try
                        //{
                        //    log.ErrorMessage = resultData.results[0].errorMessage;
                        //}
                        //catch
                        //{
                        //}
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //if (log.NrOfSuccessfullTransactions > 0)
                        //{
                        //    log.RefNumber = transaction.transactionId + "";
                        //}
                        //else
                        //{

                        //    log.RefNumber = transaction.transactionId + "";
                        //}
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.TransactionType = "MHS850MI - AddDOReceipt";
                        //log.Json = JsonConvert.SerializeObject(inforObj);
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }

                    else
                    {
                        status = "Failed : " + message.ToString();
                    }
                    //add for DO Lines
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