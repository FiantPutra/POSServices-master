using System;
using System.Collections.Generic;
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
    [Route("api/ClosingStore")]
    [ApiController]
    public class ClosingStoreController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ClosingStoreController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        //POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClosingStoreAPI transactionApi)
        {
            //log record
            LogRecord log = new LogRecord();
            log.TimeStamp = DateTime.Now;
            log.Tag = "Closing Store";
            log.Message = JsonConvert.SerializeObject(transactionApi);
            _context.LogRecord.Add(log);

            ClosingStore cashierShift = new ClosingStore();
            cashierShift.ClosignTranBal = transactionApi.closingDeposit;
            cashierShift.ClosingDeposit = transactionApi.closingDeposit;
            cashierShift.ClosingPettyCash = transactionApi.closingDeposit;
            cashierShift.ClosingStoreId = transactionApi.closingStoreId;
            cashierShift.ClosingTimeStamp = DateTime.Now;
            cashierShift.DeviceName = transactionApi.deviceName;
            cashierShift.DisputePettyCash = transactionApi.disputePettyCash;
            cashierShift.DisputeTransBal = transactionApi.disputeTransBal;
            cashierShift.EmployeeId = transactionApi.employeeId;
            cashierShift.EmployeeName = transactionApi.employeeName;
            cashierShift.OpeningDeposit = transactionApi.openingDeposit;
            cashierShift.OpeningPettyCash = transactionApi.openingPettyCash;
            cashierShift.OpeningTimeStamp = DateTime.Now;
            cashierShift.OpeningTransBal = transactionApi.openingTransBal;
            cashierShift.RealDeposit = transactionApi.realDeposit;
            cashierShift.RealPettyCash = transactionApi.realPettyCash;
            cashierShift.RealTransBal = transactionApi.realTransBal;
            cashierShift.StatusClose = transactionApi.statusClose;
            cashierShift.StoreCode = transactionApi.storeCode;
            _context.Add(cashierShift);
            _context.SaveChanges();
            this.sequenceNumber(transactionApi);

            return Ok(cashierShift);
        }

        private void sequenceNumber(ClosingStoreAPI transactionApi)
        {


            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.closingStoreId;
            log.Date = DateTime.Now;
            log.TransactionType = "Closing Store";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
    }
}