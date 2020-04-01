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
    [Route("api/TransactionView")]
    [ApiController]
    public class TransactionViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public TransactionViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getTransaction()
        {
            try
            {
                var transaction = (from tr in _context.Transaction
                                   join trl in _context.TransactionLines
                                   on tr.Id equals trl.TransactionId
                                   select new
                                   {
                                       StoreId = tr.StoreId,
                                       StoreCode = tr.StoreCode,
                                       CustomerId = tr.CustomerId,
                                       ReceiptCode = tr.RecieptCode,
                                       EmployeeId = tr.EmployeeId,
                                       MethodOfPaym = tr.MethodOfPayment,
                                       TransactionId = tr.TransactionId,
                                       TransType = tr.TransactionType,
                                       Cash = tr.Cash,
                                       EDC1 = tr.Edc1,
                                       EDC2 = tr.Edc2,
                                       Change = tr.Change,
                                       Bank1 = tr.Bank1,
                                       Bank2 = tr.Bank2,
                                       TransDateStore = tr.TransDateStore,
                                       ClosingStoreId = tr.ClosingStoreId,
                                       ClosingShiftId = tr.ClosingShiftId,
                                       ArticleId = trl.ArticleId,
                                       Qty = trl.Qty,
                                       UnitPrice = trl.UnitPrice,
                                       Amount = trl.Amount,
                                       Discount = trl.Discount,
                                       ArticleName = trl.ArticleName,
                                       ArticleIdAlias = trl.ArticleIdAlias,
                                       DiscountType = trl.DiscountType,
                                       DiscountCode = trl.DiscountCode,
                                       SPGId = trl.Spgid
                                   }).ToList();

                return Json(new[] { transaction });
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