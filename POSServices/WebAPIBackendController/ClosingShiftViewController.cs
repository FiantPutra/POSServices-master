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
    [Route("api/ClosingShiftView")]
    [ApiController]
    public class ClosingShiftViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ClosingShiftViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getClosingShift()
        {
            try
            {
                var closingShift = (from cs in _context.CashierShift
                                    select new
                                    {
                                        EmployeeCode = cs.EmployeeCode,
                                        EmployeeName = cs.EmployeeName,
                                        OpeningBalance = cs.OpeningBalance,
                                        ClosingBalance = cs.ClosingTime,
                                        OpeningTime = cs.OpeningTime,
                                        ShiftName = cs.ShiftName,
                                        ShiftCode = cs.ShiftCode,
                                        StoreCode = cs.StoreCode,
                                        StoreName = cs.StoreName,
                                        CashierShiftId = cs.CashierShiftId
                                    }).ToList();

                return Json(closingShift);
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