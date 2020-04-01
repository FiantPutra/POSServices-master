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
    [Route("api/PriceList")]
    [ApiController]
    public class PriceListController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public PriceListController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getPriceList()
        {
            try
            {
                var priceList = (from pl in _context.PriceList
                                 select new
                                 {
                                     ItemId = pl.ItemId,
                                     SalesPrice = pl.SalesPrice,
                                     Currency = pl.Currency
                                 }).ToList();

                return Json(new[] { priceList });
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

        [HttpPost("Create")]
        public async Task<IActionResult> create(ListPrice listPrice)
        {
            try
            {
                List<PriceList> list = listPrice.prices;

                for (int i = 0; i < list.Count; i++)
                {
                    PriceList price = new PriceList();
                    price.ItemId = list[i].ItemId;
                    price.SalesPrice = list[i].SalesPrice;
                    price.Currency = list[i].Currency;
                    _context.Add(price);
                    bool discExist = false;
                    discExist = _context.PriceList.Any(c => c.ItemId == price.ItemId);
                    if (discExist == false)
                    {
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            create = false,
                            message = "Cannot create a record, item number already exist."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    create = true,
                    message = "Created successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    create = false,
                    message = ex.ToString()
                });
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> update(ListPrice listPrice)
        {
            try
            {
                List<PriceList> list = listPrice.prices;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    discExist = _context.PriceList.Any(c => c.ItemId == list[i].ItemId);
                    if (discExist == true)
                    {
                        var price = _context.PriceList.Where(x => x.ItemId == list[i].ItemId).First();

                        price.SalesPrice = list[i].SalesPrice;
                        price.Currency = list[i].Currency;
                        _context.PriceList.Update(price);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Item number not found."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    update = true,
                    message = "updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    update = false,
                    message = ex.ToString()
                });
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> delete(ListPrice listPrice)
        {
            try
            {
                List<PriceList> list = listPrice.prices;

                for (int i = 0; i < list.Count; i++)
                {
                    var price = _context.PriceList.Where(x => x.ItemId == list[i].ItemId).First();
                    _context.PriceList.Remove(price);
                    _context.SaveChanges();
                }

                return StatusCode(200, new
                {
                    status = "200",
                    delete = true,
                    message = "Deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    delete = false,
                    message = ex.ToString()
                });
            }
        }
    }
}