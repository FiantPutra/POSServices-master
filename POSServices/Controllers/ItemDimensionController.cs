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
    [Route("api/ItemDimension")]
    [ApiController]
    public class ItemDimensionController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public ItemDimensionController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getData()
        {
            ItemDimension item = new ItemDimension();
            item.brands = _context.ItemDimensionBrand.ToList();
            item.sizes = _context.ItemDimensionSize.ToList();
            item.colors = _context.ItemDimensionColor.ToList();
            item.departments = _context.ItemDimensionDepartment.ToList();
            item.departmentTypes = _context.ItemDimensionDepartmentType.ToList();
            item.genders = _context.ItemDimensionGender.ToList();

            return Ok(item);

        }
    }
}