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
    [Route("api/StoreMasterAll")]
    [ApiController]
    public class StoreMasterAllController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public StoreMasterAllController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        [HttpGet]
        public List<StoreMaster> getStore(String storeCode)
        {
            //1 city, 2 regional
            List<StoreMaster> listRetails = new List<StoreMaster>();
            List<Store> stores = new List<Store>();


            String customerId = "";
            stores = _context.Store.Where(c => c.Code != "ACF" || c.Code != "CAM"
            || c.Code != "KAT" || c.Code != "KAU" || c.Code != "LAC" || c.Code != "NBR"
            || c.Code != "NBS" || c.Code != "NBT" || c.Code != "300"
            || c.Code != "310").ToList();
            foreach (Store p in stores)
            {
                try
                {
                    customerId = _context.Customer.Where(c => c.StoreId == p.Id).First().CustId;
                }
                catch (Exception ex)
                {
                    customerId = "Non";

                }

                StoreMaster article = new StoreMaster
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    City = p.City,
                    Regional = p.Regional,
                    Address = p.Address,
                    CustomerIdStore = customerId

                };

                if (article.CustomerIdStore.Equals("Non"))
                {

                }
                else
                {
                    listRetails.Add(article);
                }

            }

            return listRetails;
        }
    }
}