using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/LastSequenceNumber")]
    [ApiController]
    public class LastSequenceNumberController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;


        public LastSequenceNumberController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult LastSequence(string storeCode)
        {
            LastSequenceNumber lastSeq = new LastSequenceNumber();
            try
            {
                var transaction = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Transaction").FirstOrDefault().LastTransId;
                lastSeq.transactionId = transaction;
            }
            catch
            {
                lastSeq.transactionId = null;
            }
            try
            {
                var closingshift = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Closing Shift").FirstOrDefault().LastTransId;
                lastSeq.closingShiftId = closingshift;
            }
            catch
            {
                lastSeq.closingShiftId = null;
            }
            try
            {
                var closingstore = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Closing Store").FirstOrDefault().LastTransId;
                lastSeq.closingStoreId = closingstore;
            }
            catch
            {
                lastSeq.closingStoreId = null;
            }
            try
            {
                var Mutasiorder = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Mutasi Order").FirstOrDefault().LastTransId;
                lastSeq.mutasiOrderId = Mutasiorder;
            }
            catch
            {
                lastSeq.mutasiOrderId = null;
            }
            try
            {
                var PettyCash = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Petty Cash").FirstOrDefault().LastTransId;
                lastSeq.pettyCashId = PettyCash;
            }
            catch
            {
                lastSeq.pettyCashId = null;
            }
            try
            {
                var RequestOrder = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Request Order").FirstOrDefault().LastTransId;
                lastSeq.requestOrderId = RequestOrder;
            }
            catch
            {
                lastSeq.requestOrderId = null;
            }
            try
            {
                var ReturnOrder = _context.SequenceNumberLog.OrderByDescending(s => s.LastNumberSequence).OrderByDescending(s => s.Date).Where(x => x.StoreCode == storeCode && x.TransactionType == "Return Order").FirstOrDefault().LastTransId;
                lastSeq.returnOrderId = ReturnOrder;
            }
            catch
            {
                lastSeq.returnOrderId = null;
            }


            return Json(lastSeq);
        }
    }
}