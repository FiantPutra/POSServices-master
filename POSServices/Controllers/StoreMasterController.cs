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
    [Route("api/StoreMaster")]
    [ApiController]
    public class StoreMasterController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        APIResponse response = new APIResponse();

        public StoreMasterController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        /*
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
        */

        public IActionResult getStore()
        {
            try
            {
                var listModel = from param in _context.Store.ToList()
                                select new StoreAPIModel
                                {
                                    Id = param.Id,
                                    Code = param.Code,
                                    Address = param.Address,
                                    Address2 = param.Address2,
                                    Address3 = param.Address3,
                                    Address4 = param.Address4,
                                    City = param.City,
                                    DateOpen = param.DateOpen,
                                    Name = param.Name,
                                    Location = param.Location,
                                    Regional = param.Regional,
                                    StoreTypeId = param.StoreTypeId,
                                    TargetQty = param.TargetQty,
                                    WarehouseId = param.WarehouseId,
                                    TargetValue = param.TargetValue
                                };

                return Ok(listModel);
            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult getStoreById(int Id)
        {
            var param = _context.Store.Where(x => x.Id == Id).FirstOrDefault();
            var model = new StoreAPIModel
            {
                Id = param.Id,
                Code = param.Code,
                Address = param.Address,
                Address2 = param.Address2,
                Address3 = param.Address3,
                Address4 = param.Address4,
                City = param.City,
                DateOpen = param.DateOpen,
                Name = param.Name,
                Location = param.Location,
                Regional = param.Regional,
                StoreTypeId = param.StoreTypeId,
                TargetQty = param.TargetQty,
                WarehouseId = param.WarehouseId,
                TargetValue = param.TargetValue
            };

            return Ok(model);
        }


        [HttpPost]
        public IActionResult postStore([FromBody] StoreAPIModel param)
        {
            bool exist = _context.Store.Any(x => x.Id == param.Id);
            if(!exist)
            {
                try
                {
                    var data = new Store()
                    {
                        Code = param.Code,
                        Address = param.Address,
                        Address2 = param.Address2,
                        Address3 = param.Address3,
                        Address4 = param.Address4,
                        City = param.City,
                        DateOpen = param.DateOpen,
                        Name = param.Name,
                        Location = param.Location,
                        Regional = param.Regional,
                        StoreTypeId = param.StoreTypeId,
                        TargetQty = param.TargetQty,
                        WarehouseId = param.WarehouseId,
                        TargetValue = param.TargetValue
                    };

                    _context.Store.Add(data);
                    _context.SaveChanges();

                    response.code = "1";
                    response.message = "Data has been successfully saved";

                } catch(Exception ex)
                {
                    response.code = "0";
                    response.message = ex.ToString();
                }
            }
            else 
            {
                response.code = "5";
                response.message = "Data already exists";
            }


            return Ok(response);
        }

        [HttpPut]
        public IActionResult putStore(int id, [FromBody] StoreAPIModel param)
        {
            var dataObject = _context.Store.Where(x => x.Id == param.Id).First();
            if (dataObject.Id != 0)
            {
                try
                {
                    if (param.Address != null)
                        dataObject.Address = param.Address;

                    if (param.Address2 != null)
                        dataObject.Address2 = param.Address2;

                    if (param.Address3 != null)
                        dataObject.Address3 = param.Address3;

                    if (param.Address4 != null)
                        dataObject.Address4 = param.Address4;

                    if (param.Address != null)
                        dataObject.Address = param.Address;

                    if (param.City != null)
                        dataObject.City = param.City;

                    if (param.DateOpen != null)
                        dataObject.DateOpen = param.DateOpen;

                    if (param.Name != null)
                        dataObject.Name = param.Name;

                    if (param.Location != null)
                        dataObject.Location = param.Location;

                    if (param.Regional != null)
                        dataObject.Regional = param.Regional;

                    if (param.StoreTypeId != null)
                        dataObject.StoreTypeId = param.StoreTypeId;

                    if (param.TargetQty != null)
                        dataObject.TargetQty = param.TargetQty;

                    if (param.WarehouseId != null)
                        dataObject.WarehouseId = param.WarehouseId;

                    if (param.TargetValue != null)
                        dataObject.TargetValue = param.TargetValue;

                    _context.Store.Update(dataObject);
                    _context.SaveChanges();

                    response.code = "1";
                    response.message = "Data has been successfully updated";
                }
                catch (Exception ex)
                {
                    response.code = "0";
                    response.message = ex.ToString();
                }
            }
            else
            {
                response.code = "5";
                response.message = "Data does not exists";
            }


            return Ok(response);
        }


    }
}