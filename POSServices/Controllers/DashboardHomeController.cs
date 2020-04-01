using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardHomeController : ControllerBase
    {
        private readonly DB_BIENSI_POSContext _context;
        public DashboardHomeController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDashboardHome()
        {
            DashboardAPIModel model = new DashboardAPIModel();

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var DaysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var lastDay = new DateTime(now.Year, now.Month, DaysInMonth);
            var getBulan = DateTime.Now.Month;
            var getTahun = DateTime.Now.Year;
            DateTime minggu1A = DateTime.ParseExact(getBulan + "/" + "1" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu1B = DateTime.ParseExact(getBulan + "/" + "8" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);

            DateTime minggu2A = DateTime.ParseExact(getBulan + "/" + "8" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu2B = DateTime.ParseExact(getBulan + "/" + "16" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu3A = DateTime.ParseExact(getBulan + "/" + "16" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu3B = DateTime.ParseExact(getBulan + "/" + "24" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu4A = DateTime.ParseExact(getBulan + "/" + "24" + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime minggu4B = DateTime.ParseExact(getBulan + "/" + DaysInMonth + "/" + getTahun, "M/d/yyyy", CultureInfo.InvariantCulture);

            var TOTALVALUE1 = _context.Transaction.Where(x => x.TransactionDate >= minggu1A && x.TransactionDate <= minggu1B).ToList().Select(x => x.TotalAmounTransaction).Sum();
            model.total1 = TOTALVALUE1;
            var TOTALVALUE2 = _context.Transaction.Where(x => x.TransactionDate >= minggu2A && x.TransactionDate <= minggu2B).ToList().Select(x => x.TotalAmounTransaction).Sum();
            model.total2 = TOTALVALUE2;
            var TOTALVALUE3 = _context.Transaction.Where(x => x.TransactionDate >= minggu3A && x.TransactionDate <= minggu3B).ToList().Select(x => x.TotalAmounTransaction).Sum();
            model.total3 = TOTALVALUE3;
            var TOTALVALUE4 = _context.Transaction.Where(x => x.TransactionDate >= minggu4A && x.TransactionDate <= minggu4B).ToList().Select(x => x.TotalAmounTransaction).Sum();
            model.total4 = TOTALVALUE4;

            model.doAll = _context.InventoryTransaction.Where(x => x.TransactionTypeName == "DO").Count();
            model.doOpen = _context.InventoryTransaction.Where(x => x.TransactionTypeName == "DO" && x.Status == "Pending").Count();
            model.total = _context.Transaction.Count();
            model.harian = _context.Transaction.Where(x => x.TransactionDate == DateTime.Now).Count();
            model.employee = _context.Employee.Count();
            model.employeeActive = _context.Employee.Where(x => x.Status == true).Count();

            return Ok(model);
        }
    }
}