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
    [Route("api/ClosingShift")]
    [ApiController]
    public class ShiftController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ShiftController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        //POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClosingShiftAPI transactionApi)
        {
            APIResponse response = new APIResponse();

            try
            {
                CashierShift cashierShift = new CashierShift();
                cashierShift.CashierShiftId = transactionApi.closingShiftId;
                cashierShift.DeviceName = transactionApi.deviceName;
                cashierShift.EmployeeCode = transactionApi.employeeId;
                cashierShift.EmployeeName = transactionApi.employeeName;
                cashierShift.OpeningTime = Convert.ToDateTime(transactionApi.openingTimestamp);
                cashierShift.OpeningBalance = transactionApi.openingTransBal;
                cashierShift.ShiftCode = transactionApi.shiftCode;
                cashierShift.ShiftName = transactionApi.shiftCode + " - " + transactionApi.employeeName;
                cashierShift.ClosingBalance = transactionApi.closingTransBal;
                cashierShift.StoreCode = transactionApi.storeCode;
                cashierShift.StoreName = _context.Store.Where(c => c.Code == transactionApi.storeCode).First().Name;
                cashierShift.ClosingTime = Convert.ToDateTime(transactionApi.closingTimestamp);
                _context.Add(cashierShift);
                //log record
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "Closing Shift";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                _context.LogRecord.Add(log);
                _context.SaveChanges();
                this.sequenceNumber(transactionApi);

                response.code = "1";
                response.message = "Sucess Add Data";

                return Ok(response);
            }
            catch (Exception e)
            {
                response.code = "0";
                response.message = e.ToString();

                return BadRequest(response);
            }            
        }

        private void sequenceNumber(ClosingShiftAPI transactionApi)
        {


            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.closingShiftId;
            log.Date = DateTime.Now;
            log.TransactionType = "Closing Shift";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
    }
}