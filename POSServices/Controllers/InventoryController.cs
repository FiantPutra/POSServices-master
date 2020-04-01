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
    [Route("api/Inventory")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public InventoryController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<IActionResult> getInventory(String warehouseId)
        {
            // var items = from item in _context.Item select new { Id = item.Id };

            List<Inventory> listInventory = new List<Inventory>();
            List<InventoryLines> inventLines = _context.InventoryLines.Where(c => c.WarehouseId == warehouseId).ToList();
            foreach (InventoryLines p in inventLines)
            {
                //  Item item = _context.Item.Where(c => c.Id == p.ItemId).First();
                Inventory article = new Inventory
                {
                    id = p.ItemId,
                    articleId = p.ItemId,
                    goodQty = p.Qty,
                    rejectQty = 0,
                    status = 0,
                    whGoodQty = 0,
                    whRejectQty = 0
                };
                listInventory.Add(article);
            }
            return Ok(listInventory);
        }
    }
}