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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using POSServices.Config;

namespace POSServices.WebAPIBackendController
{
    [Route("api/ReturnOrderView")]
    [ApiController]
    public class ReturnOrderViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ReturnOrderViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getReturnOrder()
        {
            try
            {                
                var storcode =
                    (from deliver in _context.InventoryTransaction.Where(c => c.TransactionTypeId == RetailEnum.returnTransaction).OrderByDescending(c => c.Id)
                     select new
                     {
                         Id = deliver.Id,
                         StoreId = deliver.StoreId,
                         TotalQty = deliver.TotalQty,
                         TransactionTypeName = deliver.TransactionTypeName,
                         StoreCode = deliver.StoreCode,
                         StoreName = deliver.StoreName,
                         Remarks = deliver.Remarks,
                         StatusId = deliver.StatusId,
                         EmployeeCode = deliver.EmployeeCode,
                         TotalAmount = deliver.TotalAmount,
                         TransactionId = deliver.TransactionId,
                         WarehouseDestination = deliver.WarehouseDestination,
                         WarehouseOriginal = deliver.WarehouseOriginal,
                         Status = deliver.Status,
                         EmployeeName = deliver.EmployeeName,
                         TransactionDate = deliver.TransactionDate

                     }).ToList();

                return Json(new[] { storcode });
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