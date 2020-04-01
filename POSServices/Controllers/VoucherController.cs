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
    [Route("api/Voucher")]
    [ApiController]
    public class VoucherController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public VoucherController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        [HttpGet]
        public Voucher getVoucher(String VoucherCode)
        {
            Voucher v = new Voucher();
            try
            {
                v = _context.Voucher.Where(c => c.VoucherCode == VoucherCode).First();
            }
            catch (Exception ex)
            {
                v.VoucherCode = "No Voucher Available";
                v.Description = "No Voucher Available";
                v.Qty = 0;
                v.Value = 0;
            }
            return v;
        }
    }
}