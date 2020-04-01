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
    [Route("api/RequestOrderView")]
    [ApiController]
    public class RequestOrderViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public RequestOrderViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getRequestOrder(string transType)
        {
            try
            {
                return Json(new[] { _context.InventoryTransaction.Where(x => x.TransactionTypeName == transType).ToList() });
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