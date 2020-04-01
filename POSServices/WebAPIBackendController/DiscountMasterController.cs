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
    [Route("api/DiscountMaster")]
    [ApiController]
    public class DiscountMasterController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountMasterController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDiscountMaster()
        {
            var discountMaster = (from ds in _context.DiscountSetup
                                  join dsl in _context.DiscountSetupLines
                                  on ds.Id equals dsl.DiscountSetupId
                                  select new {
                                      DiscountCode = ds.DiscountCode,
                                      DiscountCategory = ds.DiscountCategory,
                                      DiscountName = ds.DiscountName,
                                      DiscountType = ds.DiscountType,
                                      CustomerGroupId = ds.CustomerGroupId,
                                      StartDate = ds.StartDate,
                                      EndDate = ds.EndDate,
                                      Status = ds.Status,
                                      DiscountCash = ds.DiscountCash,
                                      DiscountPercent = ds.DiscountPercent,
                                      QtyMin = ds.QtyMin,
                                      QtyMax = ds.QtyMax,
                                      AmountMin = ds.AmountMin,
                                      AmountMax = ds.AmountMax,
                                      ApprovedDate = ds.ApprovedDate,
                                      Multi = ds.Multi,                                      
                                      GroupCode = dsl.GroupCode,
                                      Code = dsl.Code,
                                      DiscountPercentLine = dsl.DiscountPrecentage,
                                      DiscountCashLine = dsl.DiscountCash,
                                      QtyMinLine = dsl.QtyMin,
                                      QtyMaxLine = dsl.QtyMax,
                                      AmountMinLine = dsl.AmountMin,
                                      AmountMaxLine = dsl.AmountMax,                                      
                                      MultiLine = dsl.Multi
                                  }).ToList();

            return Json(new[] { discountMaster });
        }
    }
}