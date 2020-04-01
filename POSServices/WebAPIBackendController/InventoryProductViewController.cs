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
    [Route("api/InventoryProductView")]
    [ApiController]
    public class InventoryProductViewController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public InventoryProductViewController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getInventoryProduct()
        {
            try
            {
                var inventoryProduct = (from invProd in _context.InventoryTransactionLines
                                        select new
                                        {
                                            ArticleId = invProd.ArticleId,
                                            ArticleName = invProd.ArticleName,
                                            Qty = invProd.Qty,
                                            ReceiveQty = invProd.RecieveQty,
                                            LineNum = invProd.Urridl,
                                            PackingNum = invProd.PackingNumber,
                                            DeliveryOrder = invProd.Urdlix,
                                            DistributionOrder = invProd.Urridn,
                                            SalesPrice = invProd.ValueSalesPrice
                                        }).ToList();

                return Json(new[] { inventoryProduct });
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