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
    [Route("api/ExpenseStore")]
    [ApiController]
    public class ExpenseStoreController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ExpenseStoreController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getExpenseStore()
        {
            try
            {
                return Json(new[] { _context.Expense.OrderByDescending(c => c.Id).ToList() });
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