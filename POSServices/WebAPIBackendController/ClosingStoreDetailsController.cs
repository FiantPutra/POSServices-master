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


namespace POSServices.WebAPIBackendController
{
    [Route("api/ClosingStoreDetails")]
    [ApiController]
    public class ClosingStoreDetailsController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ClosingStoreDetailsController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getClosingStoreDetails(DateTime fromDate, DateTime toDate, String storeCode)
        {
            APIResponse response = new APIResponse();

            try
            {
                object closingStoreDetailObj = new object();

                var closingStoreDetail = (from cs in _context.ClosingStore.Where(cs => cs.OpeningTimeStamp >= fromDate && cs.ClosingTimeStamp <= toDate && cs.StoreCode == storeCode)
                                          select new
                                          {
                                              closingStoreId = cs.ClosingStoreId,
                                              storeCode = cs.StoreCode,
                                              openingTransBal = cs.OpeningTransBal,
                                              closingTransBal = cs.ClosignTranBal,
                                              realTransBal = cs.RealTransBal,
                                              disputeTransBal = cs.DisputeTransBal,
                                              openingPettyCash = cs.OpeningPettyCash,
                                              closingPettyCash = cs.ClosingPettyCash,
                                              realPettyCash = cs.RealPettyCash,
                                              disputePettyCash = cs.DisputePettyCash,
                                              openingDeposit = cs.OpeningDeposit,
                                              closingDeposit = cs.ClosignTranBal,
                                              realDeposit = cs.RealDeposit,
                                              disputeDeposit = cs.DisputeDeposit,
                                              transaction = (from tr in _context.Transaction.Where(tr => tr.ClosingStoreId == cs.ClosingStoreId)
                                                             select new
                                                             {
                                                                 transactionId = tr.TransactionId,
                                                                 transactionDate = tr.TransactionDate,
                                                                 totalDiscount = tr.TotalDiscount,
                                                                 totalAmount = tr.TotalAmounTransaction
                                                             }).ToList()
                                          }).ToList();

                if (closingStoreDetail.Count > 0)
                    closingStoreDetailObj = closingStoreDetail;
                else
                    closingStoreDetailObj = "Data not found";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = closingStoreDetailObj
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    message = ex.ToString()
                });
            }
        }
    }
}