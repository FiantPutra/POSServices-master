using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POSServices.Models;
using POSServices.WebAPIModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POSServices.Controllers
{
    [Route("api/[controller]")]
    public class DiscountCodeController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public DiscountCodeController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        /*[HttpGet]
        public IActionResult Get()
        {
            List<EnumAPIModel> listModel = new List<EnumAPIModel>();
            listModel.Add(new EnumAPIModel { code = 0, name = "All" });
            listModel.Add(new EnumAPIModel { code = 1, name = "Color" });
            listModel.Add(new EnumAPIModel { code = 2, name = "Size" });
            listModel.Add(new EnumAPIModel { code = 3, name = "Gender" });
            listModel.Add(new EnumAPIModel { code = 4, name = "Department Type" });
            listModel.Add(new EnumAPIModel { code = 5, name = "Department " });
            listModel.Add(new EnumAPIModel { code = 6, name = "Brand" });
            listModel.Add(new EnumAPIModel { code = 7, name = "Item" });
            return Ok(listModel);
        } */

        // GET api/<controller>/5
        [HttpGet]
        public IActionResult Get([FromQuery]int discountCode)
        {
            List<EnumAPIModel> listModel = new List<EnumAPIModel>();

            if (discountCode == 1)
            {
                return Ok(from param in _context.ItemDimensionColor.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 2)
            {
                return Ok(from param in _context.ItemDimensionSize.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 3)
            {
                return Ok(from param in _context.ItemDimensionGender.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 4)
            {
                return Ok(from param in _context.ItemDimensionDepartmentType.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 5)
            {
                return Ok(from param in _context.ItemDimensionDepartment.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 6)
            {
                return Ok(from param in _context.ItemDimensionBrand.ToList()
                          select new
                          {
                              code = param.Code,
                              name = param.Description
                          });
            }
            else
            if (discountCode == 7)
            {
                return Ok(from param in _context.Item.ToList()
                          select new
                          {
                              code = param.ItemId,
                              name = param.Name
                          });
            }

            return Ok("No data exists");
        }



    }
}
