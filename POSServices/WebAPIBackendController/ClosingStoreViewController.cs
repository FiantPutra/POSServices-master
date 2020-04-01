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
    [Route("api/ClosingStoreView")]
    [ApiController]
    public class ClosingStoreViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ClosingStoreViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getClosingStore()
        {
            try
            {
                var closingStore = (from cs in _context.ClosingStore
                                    select new
                                    {
                                        ClosingStoreId = cs.ClosingStoreId,
                                        StoreCode = cs.StoreCode,
                                        OpeningTime = cs.OpeningTimeStamp,
                                        ClosingTime = cs.ClosingTimeStamp,
                                        OpeningTransBal = cs.OpeningTransBal,
                                        ClosingTransBal = cs.ClosignTranBal,
                                        RealTransBal = cs.RealTransBal,
                                        DisputeTransBal = cs.DisputeTransBal,
                                        OpeningPettyCash = cs.OpeningPettyCash,
                                        ClosingPettyCash = cs.ClosingPettyCash,
                                        RealPettyCash = cs.RealPettyCash,
                                        DisputePettyCash = cs.DisputePettyCash,
                                        OpeningDeposit = cs.OpeningDeposit,
                                        ClosingDeposit = cs.ClosingDeposit,
                                        RealDeposit = cs.RealDeposit,
                                        DisputeDeposit = cs.DisputeDeposit,
                                        EmployeeId = cs.EmployeeId,
                                        EmployeeName = cs.EmployeeName,
                                        StatusClose = cs.StatusClose
                                    }).ToList();

                return Json(new[] { closingStore });
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