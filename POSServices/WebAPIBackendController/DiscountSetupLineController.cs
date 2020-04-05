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
    [Route("api/DiscountSetupLine")]
    [ApiController]
    public class DiscountSetupLineController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountSetupLineController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDiscountSetupLine(int discountSetupId)
        {
            try
            {
                object discountSetupLineObj = new object();

                var discountSetupLine = (from dsl in _context.DiscountSetupLines.Where(x => x.DiscountSetupId == discountSetupId)
                                         select new
                                         {                                             
                                             GroupCode = dsl.GroupCode,
                                             Code = dsl.Code,
                                             DiscountPercent = dsl.DiscountPrecentage != null ? dsl.DiscountPrecentage : 0,
                                             DiscountCash = dsl.DiscountCash != null ? dsl.DiscountCash : 0,
                                             QtyMin = dsl.QtyMin != null ? dsl.QtyMin : 0,
                                             QtyMax = dsl.QtyMax != null ? dsl.QtyMax : 0,
                                             AmountMin = dsl.AmountMin != null ? dsl.AmountMin : 0,
                                             AmountMax = dsl.AmountMax != null ? dsl.AmountMax : 0,                                             
                                             Multi = dsl.Multi != null ? dsl.Multi : 0
                                         }).ToList();

                if (discountSetupLine.Count > 0)
                    discountSetupLineObj = discountSetupLine;
                else
                    discountSetupLineObj = "Data not found";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = discountSetupLineObj
                });
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
        public async Task<IActionResult> create(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    var discSetup = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountSetup.DiscountCode).First();

                    DiscountSetupLines discSetupLines = new DiscountSetupLines();                    
                    discSetupLines.GroupCode = list[i].GroupCode;
                    discSetupLines.Code = list[i].Code;
                    discSetupLines.DiscountPrecentage = list[i].DiscountPrecentage;
                    discSetupLines.DiscountCash = list[i].DiscountCash;
                    discSetupLines.QtyMin = list[i].QtyMin;
                    discSetupLines.QtyMax = list[i].QtyMax;
                    discSetupLines.AmountMin = list[i].AmountMin;
                    discSetupLines.AmountMax = list[i].AmountMax;                 
                    discSetupLines.Multi = list[i].Multi;
                    discSetupLines.StartDate = discSetup.StartDate;
                    discSetupLines.EndDate = discSetup.EndDate;
                    discSetupLines.DiscountSetupId = discSetup.Id;
                    _context.Add(discSetupLines);
                    _context.SaveChanges();
                }

                return StatusCode(200, new
                {
                    status = "200",
                    create = true,
                    message = "created successfully!"
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
        public async Task<IActionResult> update(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    //var discSetup = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountSetup.DiscountCode).First();

                    bool discExist = false;
                    discExist = _context.DiscountSetupLines.Any(c => c.Id == list[i].Id);
                    if (discExist == true)
                    {
                        var discSetupLnObj = _context.DiscountSetupLines.Where(x => x.Id == list[i].Id).First();                        
                        discSetupLnObj.GroupCode = list[i].GroupCode;
                        discSetupLnObj.Code = list[i].Code;
                        discSetupLnObj.DiscountCash = list[i].DiscountCash;
                        discSetupLnObj.DiscountPrecentage = list[i].DiscountPrecentage;                        
                        discSetupLnObj.AmountMin = list[i].AmountMin;
                        discSetupLnObj.AmountMax = list[i].AmountMax;
                        discSetupLnObj.QtyMin = list[i].QtyMin;
                        discSetupLnObj.QtyMax = list[i].QtyMax;                        
                        discSetupLnObj.Multi = list[i].Multi;
                        discSetupLnObj.StartDate = list[i].StartDate;
                        discSetupLnObj.EndDate = list[i].EndDate;

                        _context.DiscountSetupLines.Update(discSetupLnObj);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Record not found."
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
        public async Task<IActionResult> delete(discountSetupLineList discSetupLnList)
        {
            try
            {
                List<DiscountSetupLines> list = discSetupLnList.discountLines;

                for (int i = 0; i < list.Count; i++)
                {
                    var discSetup = _context.DiscountSetup.Where(x => x.DiscountCode == list[i].DiscountSetup.DiscountCode).First();

                    var discSetupLnObj = _context.DiscountSetupLines.Where(x => x.DiscountSetupId == discSetup.Id && x.Code == list[i].Code).First();
                    _context.DiscountSetupLines.Remove(discSetupLnObj);
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