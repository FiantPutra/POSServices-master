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
    [Route("api/CostCategoryMaster")]
    [ApiController]
    public class CostCategoryController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public CostCategoryController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<CostCategoryMaster> getStore()
        {
            //1 city, 2 regional
            List<CostCategoryMaster> listRetails = new List<CostCategoryMaster>();
            List<CostCategory> rtlObj = _context.CostCategory.ToList();
            foreach (CostCategory p in rtlObj)
            {
                CostCategoryMaster article = new CostCategoryMaster
                {
                    Id = p.Id,
                    Coa = p.Coa,
                    Name = p.Name,
                    CostCategoryId = p.CostCategoryId
                };
                listRetails.Add(article);
            }
            return listRetails;
        }
    }
}