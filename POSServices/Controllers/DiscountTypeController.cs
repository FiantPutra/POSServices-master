using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POSServices.WebAPIModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POSServices.Controllers
{
    [Route("api/DiscountType")]
    public class DiscountTypeController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            List<EnumAPIModel> listModel = new List<EnumAPIModel>();
            listModel.Add(new EnumAPIModel { code = 1, name = "Normal" });
            listModel.Add(new EnumAPIModel { code = 2, name = "Employee HO" });
            listModel.Add(new EnumAPIModel { code = 3, name = "Employee Toko" });
            listModel.Add(new EnumAPIModel { code = 4, name = "Mix and Match" });
            listModel.Add(new EnumAPIModel { code = 5, name = "Buy and Get" });
            listModel.Add(new EnumAPIModel { code = 5, name = "Voucher" });
            return Ok(listModel);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            List<EnumAPIModel> listModel = new List<EnumAPIModel>();
            listModel.Add(new EnumAPIModel { code = 1, name = "Normal" });
            listModel.Add(new EnumAPIModel { code = 2, name = "Employee HO" });
            listModel.Add(new EnumAPIModel { code = 3, name = "Employee Toko" });
            listModel.Add(new EnumAPIModel { code = 4, name = "Mix and Match" });
            listModel.Add(new EnumAPIModel { code = 5, name = "Buy and Get" });
            listModel.Add(new EnumAPIModel { code = 5, name = "Voucher" });

            EnumAPIModel model = new EnumAPIModel();
            foreach (var mod in listModel.Where(n => n.code == id))
            {
                model = mod;
                break;
            }

            return Ok(model);
        }

    }
}
