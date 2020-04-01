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
    public class InforAPIPettyCash : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public InforAPIPettyCash(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        public async Task<String> postPettyCash([FromBody] PettyCash transaction, int id, String roundNumberParam)
        {
            var credentials = new NetworkCredential(inforConfig.username, inforConfig.password);
            var handler = new HttpClientHandler { Credentials = credentials };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            String status = "";
            String roundNumber = roundNumberParam;
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    InforPettyCash inforObjTrans = new InforPettyCash();
                    inforObjTrans.program = "OPS270MI";
                    List<TransactionPettyCash> listTransaction = new List<TransactionPettyCash>();
                    for (int i = 0; i < transaction.pettyCashLine.Count; i++)
                    {
                        TransactionPettyCash t = new TransactionPettyCash();
                        t.transaction = "AddSlsTicketLin";
                        RecordPettyCash record = new RecordPettyCash();
                        record.CONO = "770";
                        record.DIVI = "AAA";
                        record.XRCD = "7000";
                        record.ITRN = roundNumber;
                        record.WHLO = transaction.storeCode;
                        record.ORNO = "PC" + id + "";
                        record.DLIX = "1";
                        record.PONR = (i + 1) + "";
                        record.POSX = "00";
                        record.CUCD = "IDR";
                        record.CUNO = transaction.customerIdStore;
                        record.ITNO = transaction.expenseCategoryId;
                        record.TRDT = DateTime.Now.ToString("yyyyMMdd");
                        record.TRTM = DateTime.Now.ToString("HHmmss");//"113000";
                        record.CUAM = transaction.pettyCashLine[i].total + "";
                        record.CSHC = "CSH";
                        record.VTCD = "0";
                        record.PYCD = "CSH";
                        record.ALUN = "PCS";

                        record.IVQA = transaction.pettyCashLine[i].quantity + "";
                        record.REFE = transaction.pettyCashLine[i].expenseName;
                        record.INYR = DateTime.Now.Year + "";
                        record.VTP1 = 0 + "";
                        record.ARAT = 1 + "";
                        record.CRTP = 1 + "";

                        t.record = record;
                        listTransaction.Add(t);
                    }

                    //sales ticket line



                    inforObjTrans.transactions = listTransaction;
                    HttpResponseMessage message = await client.PostAsJsonAsync(General.urlInfor, inforObjTrans);
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
                        //log.Description = transaction.pettyCashId;
                        //log.ErrorMessage = resultData.results[0].errorMessage;
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.RefNumber = transaction.pettyCashId + "";
                        //log.Json = JsonConvert.SerializeObject(inforObjTrans);
                        //log.TransactionType = "OPS270MI-PettyCashTcktLine";
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }
                    else
                    {
                        status = "Failed : " + message.ToString();
                    }
                    //send ticket pay

                    List<TransactionPettyCash> listTransactionPay = new List<TransactionPettyCash>();
                    InforPettyCash inforObjPay = new InforPettyCash();
                    inforObjPay.program = "OPS270MI";
                    TransactionPettyCash tRansactionAddSlsTicketPay = new TransactionPettyCash();
                    tRansactionAddSlsTicketPay.transaction = "AddSlsTicketPay";
                    RecordPettyCash recordAddSlsTicketPayEDC2 = new RecordPettyCash();
                    recordAddSlsTicketPayEDC2.CONO = "770";
                    recordAddSlsTicketPayEDC2.DLIX = "1";
                    recordAddSlsTicketPayEDC2.DIVI = "AAA";
                    recordAddSlsTicketPayEDC2.XRCD = "7090";
                    recordAddSlsTicketPayEDC2.ITRN = roundNumber;
                    recordAddSlsTicketPayEDC2.WHLO = transaction.storeCode;
                    recordAddSlsTicketPayEDC2.ORNO = "PC" + id + "";
                    recordAddSlsTicketPayEDC2.PONR = (transaction.pettyCashLine.Count + 1) + "";
                    recordAddSlsTicketPayEDC2.POSX = "00";
                    recordAddSlsTicketPayEDC2.CUCD = "IDR";
                    recordAddSlsTicketPayEDC2.TRDT = DateTime.Now.ToString("yyyyMMdd");
                    recordAddSlsTicketPayEDC2.TRTM = DateTime.Now.ToString("HHmmss");//"113000";
                    recordAddSlsTicketPayEDC2.CUAM = transaction.totalExpense + "";
                    recordAddSlsTicketPayEDC2.PYCD = "CSH";
                    recordAddSlsTicketPayEDC2.REFE = transaction.expenseCategory;
                    recordAddSlsTicketPayEDC2.ARAT = "1";
                    recordAddSlsTicketPayEDC2.CRTP = "1";
                    recordAddSlsTicketPayEDC2.ALUN = "PCS";
                    recordAddSlsTicketPayEDC2.CSHC = "CSH";

                    tRansactionAddSlsTicketPay.record = recordAddSlsTicketPayEDC2;
                    listTransactionPay.Add(tRansactionAddSlsTicketPay);
                    inforObjPay.transactions = listTransactionPay;
                    HttpResponseMessage messagebatch = await client.PostAsJsonAsync(Config.General.urlInfor, inforObjPay);

                    if (messagebatch.IsSuccessStatusCode)
                    {
                        status = messagebatch.ToString();
                        var serializer = new DataContractJsonSerializer(typeof(InforObjPostReturn));
                        var result = messagebatch.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforObjPostReturn resultData = serializer.ReadObject(stream) as InforObjPostReturn;
                        //  status = "Return : " + resultData.results[0].errorMessage + "Sukses "+ resultData.nrOfSuccessfullTransactions;
                        //IntegrationLog log = new IntegrationLog();
                        //log.Description = transaction.pettyCashId;
                        //log.ErrorMessage = resultData.results[0].errorMessage;
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.RefNumber = transaction.pettyCashId + "";
                        //log.Json = JsonConvert.SerializeObject(inforObjPay);
                        //log.TransactionType = "OPS270MI-PettyCashTkcPay";
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
                    }
                    //end for pay

                    //send batch
                    //for auto batch
                    List<TransactionPettyCash> listTransactionbatch = new List<TransactionPettyCash>();
                    InforPettyCash inforObjBatch = new InforPettyCash();
                    inforObjBatch.program = "OPS270MI";
                    TransactionPettyCash tRansactionAddBatch = new TransactionPettyCash();
                    tRansactionAddBatch.transaction = "BchPrcRound";
                    RecordPettyCash recordRoundNumber = new RecordPettyCash();
                    recordRoundNumber.CONO = "770";
                    recordRoundNumber.ITRN = roundNumber;
                    recordRoundNumber.DIVI = "AAA";
                    recordRoundNumber.TRDT = DateTime.Now.ToString("yyyyMMdd");
                    recordRoundNumber.WHLO = transaction.storeCode;
                    recordRoundNumber.ACDT = DateTime.Now.ToString("yyyyMMdd");
                    tRansactionAddBatch.record = recordRoundNumber;
                    listTransactionbatch.Add(tRansactionAddBatch);
                    inforObjBatch.transactions = listTransactionbatch;
                    HttpResponseMessage messageTicketPay = await client.PostAsJsonAsync(Config.General.urlInfor, inforObjBatch);

                    if (messageTicketPay.IsSuccessStatusCode)
                    {
                        status = messageTicketPay.ToString();
                        var serializer = new DataContractJsonSerializer(typeof(InforObjPostReturn));
                        var result = messageTicketPay.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        InforObjPostReturn resultData = serializer.ReadObject(stream) as InforObjPostReturn;
                        //  status = "Return : " + resultData.results[0].errorMessage + "Sukses "+ resultData.nrOfSuccessfullTransactions;
                        //IntegrationLog log = new IntegrationLog();
                        //log.Description = transaction.pettyCashId;
                        //log.ErrorMessage = resultData.results[0].errorMessage;
                        //log.NrOfFailedTransactions = resultData.nrOfFailedTransactions;
                        //log.NrOfSuccessfullTransactions = resultData.nrOfSuccessfullTransactions;
                        //log.NumOfLineSubmited = listTransaction.Count;
                        //log.RefNumber = transaction.pettyCashId + "";
                        //log.Json = JsonConvert.SerializeObject(inforObjBatch);
                        //log.TransactionType = "OPS270MI-PettyCashBatch";
                        //_context.IntegrationLog.Add(log);
                        //_context.SaveChanges();
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
                            //log.TransactionType = "Round Number Petty Cash";
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
    }
}