using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.Models;

namespace POSServices.WebAPIBackendController
{
    [Route("api/DiscountSetupStore")]
    [ApiController]
    public class DiscountSetupStoreController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountSetupStoreController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDiscountSetupStore(int discountSetupId)
        {
            try
            {
                var discountSetupStore = (from dss in _context.DiscountSetupStore.Where(x => x.DiscountId == discountSetupId)
                                          select new
                                          {
                                              StoreId = dss.StoreId,
                                              Store = dss.Store,
                                              DiscountId = dss.DiscountId
                                          }).ToList();

                return Json(new[] { discountSetupStore });
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
        public async Task<IActionResult> create(discountSetupStoreList discSetupStoreList)
        {
            try
            {
                List<DiscountSetupStore> list = discSetupStoreList.discSetupStore;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    bool storeExist = false;
                    discExist = _context.DiscountSetup.Any(c => c.Id == list[i].DiscountId);
                    var store = _context.Store.Where(x => x.Code == list[i].Store.Code).First();
                    storeExist = _context.DiscountSetupStore.Any(c => c.StoreId == store.Id);

                    if (discExist && !storeExist)
                    {
                        DiscountSetupStore setupStore = new DiscountSetupStore();
                        setupStore.DiscountId = list[i].DiscountId;                        
                        setupStore.StoreId = store.Id;
                        _context.Add(setupStore);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            create = false,
                            message = "Cannot create a record, store id already exist or discount id not exist."
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
        public async Task<IActionResult> update(discountSetupStoreList discSetupStoreList)
        {
            try
            {
                List<DiscountSetupStore> list = discSetupStoreList.discSetupStore;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    var store = _context.Store.Where(x => x.Code == list[i].Store.Code).First();
                    discExist = _context.DiscountSetupStore.Any(c => c.StoreId == store.Id);
                    if (discExist == true)
                    {
                        var discSetupStoreObj = _context.DiscountSetupStore.Where(x => x.StoreId == store.Id).First();
                        discSetupStoreObj.DiscountId = list[i].DiscountId;                        

                        _context.DiscountSetupStore.Update(discSetupStoreObj);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Discount code not found."
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
                    create = false,
                    message = ex.ToString()
                });
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> delete(discountSetupStoreList discSetupStoreList)
        {
            try
            {
                List<DiscountSetupStore> list = discSetupStoreList.discSetupStore;

                for (int i = 0; i < list.Count; i++)
                {
                    var store = _context.Store.Where(x => x.Code == list[i].Store.Code).First();
                    var discSetupStoreObj = _context.DiscountSetupStore.Where(x => x.StoreId == store.Id).First();
                    _context.DiscountSetupStore.Remove(discSetupStoreObj);
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