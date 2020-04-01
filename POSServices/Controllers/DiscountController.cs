using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/Discount")]
    [ApiController]
    public class DiscountController : Controller
    {
        int sumQtyDiscountMixAndMatchItem = 0;
        decimal sumAmountDiscountMixAndMatchItem = 0;

        private readonly DB_BIENSI_POSContext _context;
        public DiscountController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        List<DiscountApiItem> listDiscountApiItems = new List<DiscountApiItem>();


        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] WebAPIModel.Transaction transactionApi)
        //{
        //    SqlConnection con;
        //    SqlCommand cmd;
        //    SqlDataAdapter da;
        //    DataSet ds;
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WebAPIModel.Transaction transactionApi)
        {
            LogRecord log = new LogRecord();
            log.TimeStamp = DateTime.Now;
            log.Tag = "Discount Pre";
            log.Message = JsonConvert.SerializeObject(transactionApi);
            _context.LogRecord.Add(log);
            _context.SaveChanges();

            List<DiscountItemAPI> discItemApiEmployee = new List<DiscountItemAPI>();
            List<DiscountApi> promotions = new List<DiscountApi>();
            List<DiscountApi> promotionsDistinc = new List<DiscountApi>();
            DiscountMaster discountMaster = new DiscountMaster();

            Store store = _context.Store.Where(c => c.Code == transactionApi.storeCode).First();
            //for discount employee and customer voucher
            if (transactionApi.customerId != "")
            {
                try
                {
                    //for discount employeex
                    //IMPORTANT : harus cek employee ID untuk discount employee
                    //      bool employeeExistInCustomer = _context.Customer.Any(c => c.Address == transactionApi.customerId);
                    bool employeeInMaster = _context.Employee.Any(c => c.EmployeeCode == transactionApi.customerId);
                    if (employeeInMaster)
                    {
                        //Customer customer = _context.Customer.Where(c => c.CustId == transactionApi.customerId).First();
                        int storeIdEmployee = _context.Employee.Where(c => c.EmployeeCode == transactionApi.customerId).First().StoreId;
                        Store storeEmployee = _context.Store.Find(storeIdEmployee);
                        if (storeEmployee.Code == "000")
                        {
                            //employee HO
                            List<DiscountRetail> getDiscountEmployee =
                       _context.DiscountRetail.Where(c => c.CustomerGroupId == _context.CustomerGroup.Where(d => d.Description == "KARYAWAN HO").First().Id
                         && c.DiscountCategory == Config.RetailEnum.discountEmployee
                         && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).ToList();
                            if (getDiscountEmployee.Count > 0)
                            {
                                DiscountRetail dl = getDiscountEmployee.First();
                                //remark by frank
                                //28 oktober 2019, kenapa harus cek customer ?
                                //     bool isExistForCustomer = _context.DiscountStore.Any(c => c.DiscountId == dl.Id && c.StoreId == store.Id);
                                bool isExistForCustomer = true;
                                if (isExistForCustomer)
                                {
                                    int numberOfUsedDiscount = 0;
                                    try
                                    {
                                        //text2 buat employee code pada saat employee transaksi
                                        List<Models.Transaction> list = _context.Transaction.Where(c => c.Text2 == transactionApi.customerId
                                        && c.TransactionDate.Value.Month == DateTime.Now.Month).ToList();
                                        for (int i = 0; i < list.Count; i++)
                                        {
                                            Models.Transaction trans = list[i];
                                            int numbLineDisc = _context.TransactionLines.Where(c => c.TransactionId == trans.Id && c.Discount > 0).Sum(c => c.Qty);
                                            numberOfUsedDiscount = numberOfUsedDiscount + numbLineDisc;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                                    {
                                        DiscountItemAPI discount = new DiscountItemAPI();
                                        discount.amountDiscount = (transactionApi.transactionLines[i].quantity * transactionApi.transactionLines[i].price * dl.DiscountPercent.Value) / 100;
                                        discount.articleId = transactionApi.transactionLines[i].article.articleId;
                                        discount.articleIdFk = transactionApi.transactionLines[i].articleIdFk;
                                        discount.discountCode = dl.DiscountCode;
                                        discount.discountDesc = dl.DiscountPercent + " %";
                                        discount.discountType = Config.RetailEnum.discountEmployee + "";
                                        numberOfUsedDiscount = numberOfUsedDiscount + transactionApi.transactionLines[i].quantity;
                                        if (numberOfUsedDiscount <= 3)
                                        {
                                            //jika qty kurang dari 3
                                            discItemApiEmployee.Add(discount);
                                        }
                                    }

                                    discountMaster.discountItems = trimDiscountEmployee(discItemApiEmployee, transactionApi.customerId);// add here for auto apply
                                    discountMaster.discounts = new List<DiscountApi>();

                                    return Ok(discountMaster);
                                }
                            }
                            else
                            {

                                return Ok(discountMaster);
                            }


                        }
                        else
                        {
                            List<DiscountRetail> getDiscountEmployee =
                            _context.DiscountRetail.Where(c => c.CustomerGroupId == _context.CustomerGroup.Where(d => d.Description == "KARYAWAN TOKO").First().Id
                              && c.DiscountCategory == Config.RetailEnum.discountEmployee
                              && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).ToList();
                            if (getDiscountEmployee.Count > 0)
                            {
                                DiscountRetail dl = getDiscountEmployee.First();
                                //remark by frank
                                //28 oktober 2019, kenapa harus cek customer ?
                                //     bool isExistForCustomer = _context.DiscountStore.Any(c => c.DiscountId == dl.Id && c.StoreId == store.Id);
                                bool isExistForCustomer = true;
                                if (isExistForCustomer)
                                {
                                    int numberOfUsedDiscount = 0;
                                    try
                                    {
                                        //text2 buat employee code pada saat employee transaksi
                                        List<Models.Transaction> list = _context.Transaction.Where(c => c.Text2 == transactionApi.customerId
                                        && c.TransactionDate.Value.Month == DateTime.Now.Month).ToList();
                                        for (int i = 0; i < list.Count; i++)
                                        {
                                            Models.Transaction trans = list[i];
                                            int numbLineDisc = _context.TransactionLines.Where(c => c.TransactionId == trans.Id && c.Discount > 0).Sum(c => c.Qty);
                                            numberOfUsedDiscount = numberOfUsedDiscount + numbLineDisc;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                                    {
                                        DiscountItemAPI discount = new DiscountItemAPI();
                                        discount.amountDiscount = (transactionApi.transactionLines[i].quantity * transactionApi.transactionLines[i].price * dl.DiscountPercent.Value) / 100;
                                        discount.articleId = transactionApi.transactionLines[i].article.articleId;
                                        discount.articleIdFk = transactionApi.transactionLines[i].articleIdFk;
                                        discount.discountCode = dl.DiscountCode;
                                        discount.discountDesc = dl.DiscountPercent + " %";
                                        discount.discountType = Config.RetailEnum.discountEmployee + "";
                                        numberOfUsedDiscount = numberOfUsedDiscount + transactionApi.transactionLines[i].quantity;
                                        if (numberOfUsedDiscount <= 3)
                                        {
                                            //jika qty kurang dari 3
                                            discItemApiEmployee.Add(discount);
                                        }
                                    }

                                    discountMaster.discountItems = trimDiscountEmployee(discItemApiEmployee, transactionApi.customerId);// add here for auto apply
                                    discountMaster.discounts = new List<DiscountApi>();

                                    return Ok(discountMaster);
                                }
                            }
                            else
                            {

                                return Ok(discountMaster);
                            }

                        }


                    }
                    else
                    {
                        //return Ok(discountMaster);
                    }
                }
                catch (Exception ex)
                {


                }
            }
            //end of employee discount


            try
            {
                //normal discount
                List<TransactionLine> transactionLines = transactionApi.transactionLines.Where(c => c.discountType != 2 && c.discountType != 3).ToList();

                transactionApi.transactionLines = null;
                transactionApi.transactionLines = transactionLines;
                for (int i = 0; i < transactionApi.transactionLines.Count; i++)
                {
                    if (transactionApi.transactionLines[i].price > (transactionApi.transactionLines[i].discount / transactionApi.transactionLines[i].quantity))
                    {
                        Article article = transactionApi.transactionLines[i].article;
                        if (transactionApi.customerId == "")
                        {
                            //check brand
                            try
                            {
                                ItemDimensionBrand brand = _context.ItemDimensionBrand.Where(c => c.Description == article.brand).First();
                                List<DiscountRetailLines> discountLinesByBrand = _context.DiscountRetailLines.Where(c => c.BrandCode == brand.Id).ToList();
                                for (int j = 0; j < discountLinesByBrand.Count; j++)
                                {
                                    DiscountRetailLines dl = discountLinesByBrand[j];
                                    int discountId = dl.DiscountRetailId;
                                    //check if store is applied to the discount or not;
                                    bool isExistForCustomer = _context.DiscountStore.Any(c => c.DiscountId == discountId && c.StoreId == store.Id);
                                    //for check if no customer for discount and discount will be applied to all store
                                    int numOFCustomer = _context.DiscountStore.Where(c => c.DiscountId == discountId).ToList().Count;
                                    // if (isExistForCustomer || numOFCustomer == 0)
                                    if (isExistForCustomer || numOFCustomer == 0)
                                    {
                                        try
                                        {
                                            DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId
                                            && c.Status == true
                                            && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();

                                            //add for customer group;
                                            if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                            {
                                                if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                                {
                                                    if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                    {

                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                        discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;
                                                        //for disc percent
                                                        if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                        {
                                                            discount.discountDesc = discount.discountPercent + " %";
                                                        }
                                                        else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                        {
                                                            discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "K";
                                                        }

                                                        discount.status = 1;
                                                        discount.discountType = Config.RetailEnum.discountNormal;
                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        if (dl.DiscountPrecentage.HasValue)
                                                        {
                                                            discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                        }
                                                        else if (dl.CashDiscount.HasValue)
                                                        {
                                                            discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                        }

                                                        //if discount has been applied then dont apply for normal discount
                                                        if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                        {
                                                            promotions.Add(discount);
                                                        }

                                                    }
                                                    if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                                    {
                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.discountItem = dl.ArticleIdDiscount;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = 0;
                                                        discount.discountAmount = 0;


                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                        if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                        {
                                                            discount.discountDesc = discount.discountPercent + " %";
                                                        }
                                                        else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                        {
                                                            discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "K";
                                                        }
                                                        promotions.Add(discount);

                                                    }
                                                    if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                                    {
                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.discountItem = dl.ArticleIdDiscount;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = 0;
                                                        discount.discountAmount = 0;

                                                        int qty = transactionApi.transactionLines.Where(c => c.article.brand == brand.Description).Sum(c => c.quantity);
                                                        decimal money = transactionApi.transactionLines.Where(c => c.article.brand == brand.Description).Sum(c => c.subtotal);

                                                        if (dl.Qty > 0)
                                                        {
                                                            if (transactionApi.transactionLines.Where(c => c.article.brand == brand.Description).Sum(c => c.quantity) >= dl.Qty)
                                                            {
                                                                discount.status = 1;
                                                            }
                                                            else
                                                            {
                                                                discount.status = 0;
                                                            }
                                                        }

                                                        if (dl.AmountTransaction > 0)
                                                        {
                                                            if (transactionApi.transactionLines.Where(c => c.article.brand == brand.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                            {
                                                                discount.status = 1;
                                                            }
                                                            else
                                                            {
                                                                discount.status = 0;
                                                            }
                                                        }

                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                        discount.discountDesc = discount.discountCode;


                                                        promotions.Add(discount);
                                                    }
                                                }
                                                //add discount item API
                                                if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet || dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                                {
                                                    DiscountApiItem discountApiItem = new DiscountApiItem();

                                                    discountApiItem.price = article.price;
                                                    discountApiItem.DiscountRetailId = dc.Id;
                                                    discountApiItem.DiscountRetailLinesId = dl.Id;


                                                    if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                                    {
                                                        discountApiItem.discountDesc = dc.DiscountCode;
                                                        discountApiItem.discountType = Config.RetailEnum.discountBuyAndGet;
                                                    }
                                                    else
                                                    {
                                                        decimal discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                        decimal discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                        if (discountPercent > 0 && discountAmount == 0)
                                                        {
                                                            discountApiItem.discountDesc = discountPercent + " %";
                                                        }
                                                        else if (discountPercent == 0 && discountAmount > 0)
                                                        {
                                                            discountApiItem.discountDesc = (Math.Floor(discountAmount) / 1000) + "K";
                                                        }
                                                        discountApiItem.discountType = Config.RetailEnum.discountMixAndMatch;
                                                    }
                                                    //  discountApiItem.discountDesc = dc.Description;
                                                    discountApiItem.discountCode = dc.DiscountCode;
                                                    discountApiItem.articleId = article.articleId;
                                                    discountApiItem.articleIdFk = article.id;
                                                    discountApiItem.qty = transactionApi.transactionLines[i].quantity;
                                                    if (dc.DiscountPercent.HasValue)
                                                    {
                                                        discountApiItem.amountDiscount = (dc.DiscountPercent.Value * article.price * discountApiItem.qty) / 100;
                                                    }
                                                    else
                                                    {
                                                        discountApiItem.amountDiscount = 0;

                                                    }
                                                    listDiscountApiItems.Add(discountApiItem);
                                                }
                                                //end of add discount api item
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            String error = ex.ToString();
                                        }
                                        //mix and match

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                String error = ex.ToString();
                            }

                            //by department
                            try
                            {
                                ItemDimensionDepartment department = _context.ItemDimensionDepartment.Where(c => c.Description == article.department).First();
                                List<DiscountRetailLines> discountLinesByDepartment = _context.DiscountRetailLines.Where(c => c.Department == department.Id).ToList();
                                foreach (DiscountRetailLines dl in discountLinesByDepartment)
                                {
                                    DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                    if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                    {
                                        if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                        {
                                            if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                discount.status = 1;
                                                discount.discountType = Config.RetailEnum.discountNormal;
                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                if (dl.DiscountPrecentage.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                }
                                                else if (dl.CashDiscount.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                }
                                                //if discount has been applied then dont apply for normal discount
                                                if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                {
                                                    promotions.Add(discount);
                                                }
                                            }
                                            if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                            {

                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.status = 0;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }


                                                if (transactionApi.transactionLines[i].quantity >= dl.Qty && dl.Qty > 0)
                                                {
                                                    discount.status = 1;
                                                }


                                                if (dl.AmountTransaction > 0 && transactionApi.transactionLines[i].subtotal >= dl.AmountTransaction)
                                                {
                                                    discount.status = 1;
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                promotions.Add(discount);


                                            }
                                            //buy and get
                                            if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountItem = dl.ArticleIdDiscount;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = 0;
                                                discount.discountAmount = 0;

                                                int qty = transactionApi.transactionLines.Where(c => c.article.department == department.Description).Sum(c => c.quantity);
                                                decimal money = transactionApi.transactionLines.Where(c => c.article.department == department.Description).Sum(c => c.subtotal);

                                                if (dl.Qty > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.department == department.Description).Sum(c => c.quantity) >= dl.Qty)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                if (dl.AmountTransaction > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.department == department.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                discount.discountDesc = dc.Description;
                                                promotions.Add(discount);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //by department type
                            try
                            {

                                ItemDimensionDepartmentType departmentType = _context.ItemDimensionDepartmentType.Where(c => c.Description == article.departmentType).First();
                                List<DiscountRetailLines> discountLinesByDepartmentType = _context.DiscountRetailLines.Where(c => c.DepartmentType == departmentType.Id).ToList();
                                foreach (DiscountRetailLines dl in discountLinesByDepartmentType)
                                {
                                    DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                    if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                    {
                                        if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                        {
                                            if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                discount.status = 1;
                                                discount.discountType = Config.RetailEnum.discountNormal;
                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                if (dl.DiscountPrecentage.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                }
                                                else if (dl.CashDiscount.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                }
                                                //if discount has been applied then dont apply for normal discount
                                                if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                {
                                                    promotions.Add(discount);
                                                }
                                            }
                                            if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                            {

                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.status = 0;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }


                                                if (transactionApi.transactionLines[i].quantity >= dl.Qty && dl.Qty > 0)
                                                {
                                                    discount.status = 1;
                                                }


                                                if (dl.AmountTransaction > 0 && transactionApi.transactionLines[i].subtotal >= dl.AmountTransaction)
                                                {
                                                    discount.status = 1;
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                promotions.Add(discount);


                                            }
                                            //buy and get
                                            if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountItem = dl.ArticleIdDiscount;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = 0;
                                                discount.discountAmount = 0;

                                                int qty = transactionApi.transactionLines.Where(c => c.article.departmentType == departmentType.Description).Sum(c => c.quantity);
                                                decimal money = transactionApi.transactionLines.Where(c => c.article.departmentType == departmentType.Description).Sum(c => c.subtotal);

                                                if (dl.Qty > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.departmentType == departmentType.Description).Sum(c => c.quantity) >= dl.Qty)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                if (dl.AmountTransaction > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.departmentType == departmentType.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                discount.discountDesc = dc.Description;
                                                promotions.Add(discount);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {


                            }

                            //by department gendre
                            try
                            {
                                ItemDimensionGender gender = _context.ItemDimensionGender.Where(c => c.Description == article.gender).First();
                                List<DiscountRetailLines> discountLinesByGender = _context.DiscountRetailLines.Where(c => c.Gender == gender.Id).ToList();
                                foreach (DiscountRetailLines dl in discountLinesByGender)
                                {
                                    DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                    if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                    {
                                        if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                        {
                                            if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                discount.status = 1;
                                                discount.discountType = Config.RetailEnum.discountNormal;
                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                if (dl.DiscountPrecentage.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                }
                                                else if (dl.CashDiscount.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                }

                                                //if discount has been applied then dont apply for normal discount
                                                if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                {
                                                    promotions.Add(discount);
                                                }
                                            }

                                            if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                            {

                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.status = 0;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;
                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                if (transactionApi.transactionLines[i].quantity >= dl.Qty && dl.Qty > 0)
                                                {
                                                    discount.status = 1;
                                                }


                                                if (dl.AmountTransaction > 0 && transactionApi.transactionLines[i].subtotal >= dl.AmountTransaction)
                                                {
                                                    discount.status = 1;
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                promotions.Add(discount);


                                            }
                                            //buy and get

                                            if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountItem = dl.ArticleIdDiscount;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = 0;
                                                discount.discountAmount = 0;

                                                int qty = transactionApi.transactionLines.Where(c => c.article.gender == gender.Description).Sum(c => c.quantity);
                                                decimal money = transactionApi.transactionLines.Where(c => c.article.gender == gender.Description).Sum(c => c.subtotal);

                                                if (dl.Qty > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.gender == gender.Description).Sum(c => c.quantity) >= dl.Qty)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                if (dl.AmountTransaction > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.gender == gender.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                discount.discountDesc = dc.Description;
                                                promotions.Add(discount);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //by size
                            try
                            {
                                ItemDimensionSize size = _context.ItemDimensionSize.Where(c => c.Description == article.size).First();
                                List<DiscountRetailLines> discountLinesByGender = _context.DiscountRetailLines.Where(c => c.Size == size.Id).ToList();
                                foreach (DiscountRetailLines dl in discountLinesByGender)
                                {
                                    DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                    if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                    {
                                        if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                        {
                                            if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                discount.status = 1;
                                                discount.discountType = Config.RetailEnum.discountNormal;
                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                if (dl.DiscountPrecentage.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                }
                                                else if (dl.CashDiscount.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                }

                                                //if discount has been applied then dont apply for normal discount
                                                if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                {
                                                    promotions.Add(discount);
                                                }
                                            }

                                            if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                            {

                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.status = 0;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;
                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                if (transactionApi.transactionLines[i].quantity >= dl.Qty && dl.Qty > 0)
                                                {
                                                    discount.status = 1;
                                                }


                                                if (dl.AmountTransaction > 0 && transactionApi.transactionLines[i].subtotal >= dl.AmountTransaction)
                                                {
                                                    discount.status = 1;
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                promotions.Add(discount);


                                            }
                                            //buy and get
                                            if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountItem = dl.ArticleIdDiscount;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = 0;
                                                discount.discountAmount = 0;

                                                int qty = transactionApi.transactionLines.Where(c => c.article.size == size.Description).Sum(c => c.quantity);
                                                decimal money = transactionApi.transactionLines.Where(c => c.article.size == size.Description).Sum(c => c.subtotal);

                                                if (dl.Qty > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.size == size.Description).Sum(c => c.quantity) >= dl.Qty)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                if (dl.AmountTransaction > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.size == size.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                discount.discountDesc = dc.Description;
                                                promotions.Add(discount);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //by color
                            try
                            {
                                ItemDimensionColor color = _context.ItemDimensionColor.Where(c => c.Description == article.color).First();
                                List<DiscountRetailLines> discountLinesByGender = _context.DiscountRetailLines.Where(c => c.Color == color.Id).ToList();
                                foreach (DiscountRetailLines dl in discountLinesByGender)
                                {
                                    DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                    if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                    {
                                        if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false)
                                        {
                                            if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                discount.status = 1;
                                                discount.discountType = Config.RetailEnum.discountNormal;
                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                if (dl.DiscountPrecentage.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                }
                                                else if (dl.CashDiscount.HasValue)
                                                {
                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                }

                                                //if discount has been applied then dont apply for normal discount
                                                if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                {
                                                    promotions.Add(discount);
                                                }
                                            }

                                            if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                            {

                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.status = 0;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;
                                                //for disc percent

                                                if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                {
                                                    discount.discountDesc = discount.discountPercent + " %";
                                                }
                                                else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                {
                                                    discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                }

                                                if (transactionApi.transactionLines[i].quantity >= dl.Qty && dl.Qty > 0)
                                                {
                                                    discount.status = 1;
                                                }


                                                if (dl.AmountTransaction > 0 && transactionApi.transactionLines[i].subtotal >= dl.AmountTransaction)
                                                {
                                                    discount.status = 1;
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                promotions.Add(discount);


                                            }
                                            //buy and get
                                            if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                            {
                                                DiscountApi discount = new DiscountApi();
                                                discount.id = dc.Id;
                                                discount.discountItem = dl.ArticleIdDiscount;
                                                discount.discountCode = dc.DiscountCode;
                                                discount.discountPercent = 0;
                                                discount.discountAmount = 0;

                                                int qty = transactionApi.transactionLines.Where(c => c.article.color == color.Description).Sum(c => c.quantity);
                                                decimal money = transactionApi.transactionLines.Where(c => c.article.color == color.Description).Sum(c => c.subtotal);

                                                if (dl.Qty > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.color == color.Description).Sum(c => c.quantity) >= dl.Qty)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                if (dl.AmountTransaction > 0)
                                                {
                                                    if (transactionApi.transactionLines.Where(c => c.article.color == color.Description).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                    {
                                                        discount.status = 1;
                                                    }
                                                    else
                                                    {
                                                        discount.status = 0;
                                                    }
                                                }

                                                discount.articleId = article.articleId;
                                                discount.articleIdFk = article.id;
                                                discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                discount.discountDesc = dc.Description;
                                                promotions.Add(discount);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //by item
                            try
                            {

                                Item item = _context.Item.Where(c => c.ItemId == transactionApi.transactionLines[i].article.articleId).First();
                                List<DiscountRetailLines> discountLinesByGender = _context.DiscountRetailLines.Where(c => c.ArticleId == item.Id).ToList();
                                foreach (DiscountRetailLines dline in discountLinesByGender)
                                {
                                    try
                                    {
                                        DiscountRetailLines dl = dline;
                                        DiscountRetail dc = _context.DiscountRetail.Where(c => c.Id == dl.DiscountRetailId && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).First();
                                        bool isExistForCustomer = _context.DiscountStore.Any(c => c.DiscountId == dc.Id && c.StoreId == store.Id);
                                        int numstore = _context.DiscountStore.Where(c => c.DiscountId == dc.Id).ToList().Count;
                                        //for check if no customer for discount and discount will be applied to all store
                                        //    int numOFCustomer = _context.DiscountStore.Where(c => c.DiscountId == discountId).ToList().Count;
                                        // if (isExistForCustomer || numOFCustomer == 0)
                                        if (isExistForCustomer || numstore == 0)
                                        {
                                            if (_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                            {
                                                if (promotions.Any(c => c.discountCode == dc.DiscountCode) == false || dc.DiscountType == Config.RetailEnum.discountMixAndMatch)
                                                {
                                                    //mix and match for product
                                                    if (dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                                    {
                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.status = 0;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = dc.DiscountPercent.HasValue ? dc.DiscountPercent.Value : 0;
                                                        discount.discountAmount = dc.Amount.HasValue ? dc.DiscountCash.Value : 0;

                                                        //for disc percent

                                                        if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                        {
                                                            discount.discountDesc = discount.discountPercent + " %";
                                                        }
                                                        else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                        {
                                                            discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "K";
                                                        }


                                                        if (dc.Qty > 0 && dc.Amount == 0)
                                                        {
                                                            try
                                                            {
                                                                //by qty
                                                                bool any = _context.DiscountRetailLines.Any(c => c.DiscountRetailId == dc.Id && c.ArticleId == transactionApi.transactionLines[i].articleIdFk);
                                                                sumQtyDiscountMixAndMatchItem = sumQtyDiscountMixAndMatchItem + transactionApi.transactionLines[i].quantity;
                                                                if (sumQtyDiscountMixAndMatchItem >= dc.Qty)
                                                                {
                                                                    if (promotions.Any(c => c.discountCode == dc.DiscountCode) == true)
                                                                    {
                                                                        DiscountApi dpremove = promotions.Where(c => c.discountCode == dc.DiscountCode).First();
                                                                        promotions.Remove(dpremove);

                                                                    }
                                                                    discount.status = 1;
                                                                }
                                                            }
                                                            catch
                                                            {

                                                            }
                                                        }
                                                        else if (dc.Qty == 0 && dc.Amount > 0)
                                                        {
                                                            //by amount
                                                            try
                                                            {
                                                                bool any = _context.DiscountRetailLines.Any(c => c.DiscountRetailId == dc.Id && c.ArticleId == transactionApi.transactionLines[i].articleIdFk);
                                                                sumAmountDiscountMixAndMatchItem = sumAmountDiscountMixAndMatchItem + transactionApi.transactionLines[i].subtotal;
                                                                if (sumAmountDiscountMixAndMatchItem >= dc.Amount)
                                                                {
                                                                    if (promotions.Any(c => c.discountCode == dc.DiscountCode) == true)
                                                                    {
                                                                        DiscountApi dpremove = promotions.Where(c => c.discountCode == dc.DiscountCode).First();
                                                                        promotions.Remove(dpremove);

                                                                    }
                                                                    discount.status = 1;

                                                                }
                                                            }
                                                            catch
                                                            {

                                                            }

                                                        }



                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        discount.discountType = Config.RetailEnum.discountMixAndMatch;
                                                        promotions.Add(discount);
                                                    }
                                                    else if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                    {
                                                        //for normal discount
                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                        discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                        //for disc percent

                                                        if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                        {
                                                            discount.discountDesc = discount.discountPercent + " %";
                                                        }
                                                        else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                        {
                                                            discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                        }

                                                        discount.status = 1;
                                                        discount.discountType = Config.RetailEnum.discountNormal;
                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        //add at 19 desember 2019
                                                        if (dl.DiscountPrecentage.HasValue)
                                                        {
                                                            if (dl.DiscountPrecentage.Value > 0)
                                                            {
                                                                discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                            }
                                                        }
                                                        if (dl.CashDiscount.HasValue)
                                                        {
                                                            if (dl.CashDiscount.Value > 0)
                                                            {
                                                                discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                            }
                                                        }
                                                        //if discount has been applied then dont apply for normal discount
                                                        if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                        {
                                                            promotions.Add(discount);
                                                        }

                                                    }    //buy and get
                                                         //buy and get
                                                    if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                                    {
                                                        DiscountApi discount = new DiscountApi();
                                                        discount.id = dc.Id;
                                                        discount.discountItem = dl.ArticleIdDiscount;
                                                        discount.discountCode = dc.DiscountCode;
                                                        discount.discountPercent = 0;
                                                        discount.discountAmount = 0;

                                                        int qty = transactionApi.transactionLines.Where(c => c.article.articleId == item.ItemId).Sum(c => c.quantity);
                                                        decimal money = transactionApi.transactionLines.Where(c => c.article.articleId == item.ItemId).Sum(c => c.subtotal);

                                                        if (dl.Qty > 0)
                                                        {
                                                            if (transactionApi.transactionLines.Where(c => c.article.articleId == article.articleId).Sum(c => c.quantity) >= dl.Qty)
                                                            {
                                                                discount.status = 1;
                                                            }
                                                            else
                                                            {
                                                                discount.status = 0;
                                                            }
                                                        }

                                                        if (dl.AmountTransaction > 0)
                                                        {
                                                            if (transactionApi.transactionLines.Where(c => c.article.articleId == article.articleId).Sum(c => c.subtotal) >= dl.AmountTransaction)
                                                            {
                                                                discount.status = 1;
                                                            }
                                                            else
                                                            {
                                                                discount.status = 0;
                                                            }
                                                        }

                                                        discount.articleId = article.articleId;
                                                        discount.articleIdFk = article.id;
                                                        discount.discountType = Config.RetailEnum.discountBuyAndGet;
                                                        discount.discountDesc = dc.Description;
                                                        promotions.Add(discount);
                                                    }
                                                }

                                                //add discount item API by 22012019

                                                if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet || dc.DiscountCategory == Config.RetailEnum.discountMixAndMatch)
                                                {
                                                    DiscountApiItem discountApiItem = new DiscountApiItem();

                                                    discountApiItem.price = article.price;
                                                    discountApiItem.DiscountRetailId = dc.Id;
                                                    discountApiItem.DiscountRetailLinesId = dl.Id;


                                                    if (dc.DiscountCategory == Config.RetailEnum.discountBuyAndGet)
                                                    {
                                                        discountApiItem.discountDesc = dc.Description;
                                                        discountApiItem.discountType = Config.RetailEnum.discountBuyAndGet;
                                                    }
                                                    else
                                                    {
                                                        decimal discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                        decimal discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                        if (discountPercent > 0 && discountAmount == 0)
                                                        {
                                                            discountApiItem.discountDesc = discountPercent + " %";
                                                        }
                                                        else if (discountPercent == 0 && discountAmount > 0)
                                                        {
                                                            discountApiItem.discountDesc = (Math.Floor(discountAmount) / 1000) + "K";
                                                        }
                                                        //if discount mix and match > 1 item

                                                        discountApiItem.discountType = Config.RetailEnum.discountMixAndMatch;
                                                    }
                                                    //  discountApiItem.discountDesc = dc.Description;
                                                    discountApiItem.discountCode = dc.DiscountCode;
                                                    discountApiItem.articleId = article.articleId;
                                                    discountApiItem.articleIdFk = article.id;
                                                    discountApiItem.qty = transactionApi.transactionLines[i].quantity;
                                                    if (dc.DiscountPercent.HasValue && dc.DiscountPercent.Value > 0)
                                                    {

                                                        discountApiItem.amountDiscount = (dc.DiscountPercent.Value * article.price * discountApiItem.qty) / 100;

                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                        {
                                                            if (dc.DiscountCash.HasValue)
                                                            {
                                                                discountApiItem.amountDiscount = dc.DiscountCash.Value;
                                                            }
                                                        }


                                                    }
                                                    listDiscountApiItems.Add(discountApiItem);
                                                }
                                                //end of add discount api item
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        else
                        {
                            // by customer group
                            try
                            {
                                //karena by customer group jadi cek dlu does coestomer exist
                                if (transactionApi.customerId != "")
                                {

                                    bool custExist = _context.Customer.Any(c => c.CustId == transactionApi.customerId);
                                    if (custExist)
                                    {
                                        Customer customer = _context.Customer.Where(c => c.CustId == transactionApi.customerId).First();
                                        List<DiscountRetail> discountRetailsCustGroup = _context.DiscountRetail.Where(c => c.CustomerGroupId == customer.CustGroupId
                                        && c.Status == true && c.StartDate <= DateTime.UtcNow.ToLocalTime() && c.EndDate >= DateTime.Now.ToLocalTime()).ToList();
                                        for (int l = 0; l < discountRetailsCustGroup.Count; l++)
                                        {
                                            DiscountRetail dc = discountRetailsCustGroup[l];
                                            if (!_context.CustomerGroup.Where(c => c.Id == dc.CustomerGroupId).First().Description.Equals("Default"))
                                            {
                                                //by item
                                                try
                                                {
                                                    //by item
                                                    Item item = _context.Item.Where(c => c.ItemId == transactionApi.transactionLines[i].article.articleId).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.ArticleId == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }
                                                // by brand
                                                try
                                                {
                                                    ItemDimensionBrand item = _context.ItemDimensionBrand.Where(c => c.Description == transactionApi.transactionLines[i].article.brand).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.BrandCode == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }

                                                // by department
                                                try
                                                {

                                                    ItemDimensionDepartment item = _context.ItemDimensionDepartment.Where(c => c.Description == transactionApi.transactionLines[i].article.department).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.Department == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }


                                                // by department Type
                                                try
                                                {
                                                    //by item
                                                    ItemDimensionDepartmentType item = _context.ItemDimensionDepartmentType.Where(c => c.Description == transactionApi.transactionLines[i].article.departmentType).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.DepartmentType == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {

                                                }

                                                // by Gender
                                                try
                                                {
                                                    //by item
                                                    ItemDimensionGender item = _context.ItemDimensionGender.Where(c => c.Description == transactionApi.transactionLines[i].article.gender).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.Gender == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }


                                                try
                                                {
                                                    //by size
                                                    ItemDimensionSize item = _context.ItemDimensionSize.Where(c => c.Description == transactionApi.transactionLines[i].article.size).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.Size == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            promotions.Add(discount);

                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }

                                                // by color
                                                try
                                                {

                                                    ItemDimensionColor item = _context.ItemDimensionColor.Where(c => c.Description == transactionApi.transactionLines[i].article.color).First();
                                                    List<DiscountRetailLines> discountLineCustGroup = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dc.Id
                                                    && c.Color == item.Id).ToList();
                                                    for (int j = 0; j < discountLineCustGroup.Count; j++)
                                                    {
                                                        DiscountRetailLines dl = discountLineCustGroup[j];
                                                        if (dc.DiscountCategory == Config.RetailEnum.discountNormal)
                                                        {
                                                            //for normal discount
                                                            DiscountApi discount = new DiscountApi();
                                                            discount.id = dc.Id;
                                                            discount.discountCode = dc.DiscountCode;
                                                            discount.discountPercent = dl.DiscountPrecentage.HasValue ? dl.DiscountPrecentage.Value : 0;
                                                            discount.discountAmount = dl.CashDiscount.HasValue ? dl.CashDiscount.Value : 0;

                                                            //for disc percent
                                                            if (discount.discountPercent > 0 && discount.discountAmount == 0)
                                                            {
                                                                discount.discountDesc = discount.discountPercent + " %";
                                                            }
                                                            else if (discount.discountPercent == 0 && discount.discountAmount > 0)
                                                            {
                                                                discount.discountDesc = (Math.Floor(discount.discountAmount) / 1000) + "k";
                                                            }

                                                            discount.status = 1;
                                                            discount.discountType = Config.RetailEnum.discountNormal;
                                                            discount.articleId = article.articleId;
                                                            discount.articleIdFk = article.id;
                                                            if (dl.DiscountPrecentage.HasValue)
                                                            {
                                                                if (dl.DiscountPrecentage > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * (article.price * (dl.DiscountPrecentage.Value / 100));
                                                                }
                                                            }
                                                            if (dl.CashDiscount.HasValue)
                                                            {
                                                                if (dl.CashDiscount > 0)
                                                                {
                                                                    discount.totalDiscount = transactionApi.transactionLines[i].quantity * dl.CashDiscount.Value;
                                                                }
                                                            }
                                                            //if discount has been applied then dont apply for normal discount
                                                            //  if (transactionApi.transactionLines[i].discountCode == "" || transactionApi.transactionLines[i].discountCode == null)
                                                            //  {
                                                            promotions.Add(discount);
                                                            //  }
                                                        }
                                                    }
                                                }
                                                catch
                                                {


                                                }


                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) { }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }

            //log record
            LogRecord log2 = new LogRecord();
            log2.TimeStamp = DateTime.Now;
            log2.Tag = "Discount";
            log2.Message = JsonConvert.SerializeObject(transactionApi);
            _context.LogRecord.Add(log2);
            _context.SaveChanges();


            discountMaster.discounts = eliminateDiscount(promotions, transactionApi);
            discountMaster.discountItems = getListDiscountItem(promotions);
            //get normal discount;
            return Ok(discountMaster);
        }


        //change by frank on 15 Novemebr 2019
        //check discount buy and get
        private List<DiscountApi> eliminateDiscount(List<DiscountApi> discountApi, WebAPIModel.Transaction transactionAPI)
        {

            List<DiscountRetail> discountRetails = _context.DiscountRetail.ToList();
            List<DiscountApi> list = new List<DiscountApi>();
            for (int i = 0; i < discountApi.Count; i++)
            {
                {
                    DiscountRetail discountRetail = discountRetails.Where(c => c.DiscountCode == discountApi[i].discountCode).First();
                    try
                    {
                        if (discountApi[i].discountType == Config.RetailEnum.discountMixAndMatch)
                        {
                            if (sumQtyDiscountMixAndMatchItem > 0 || sumAmountDiscountMixAndMatchItem == 0)
                            {
                                int totalQtyDiscount = discountRetail.Qty.Value;
                                int totalQtyPurchase = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).Sum(c => c.qty);
                                //rubah in jka mau all condition meet to lebih dari
                                if (totalQtyPurchase == totalQtyDiscount)
                                {
                                    discountApi[i].status = 1;
                                }
                                else
                                {
                                    discountApi[i].status = 0;
                                }
                            }
                            else if (sumQtyDiscountMixAndMatchItem == 0 || sumAmountDiscountMixAndMatchItem > 0)
                            {
                                decimal totalQtyDiscount = discountRetail.Amount.Value;
                                decimal totalQtyPurchase = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).Sum(c => c.amountDiscount);
                                //rubah in jka mau all condition meet to lebih dari
                                if (totalQtyPurchase == totalQtyDiscount)
                                {
                                    discountApi[i].status = 1;
                                }
                                else
                                {
                                    discountApi[i].status = 0;
                                }
                            }

                            discountApi[i].discountApiItems = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).ToList();
                            list.Add(discountApi[i]);
                        }
                        else if (discountApi[i].discountType == Config.RetailEnum.discountBuyAndGet)
                        {
                            int totalQtyDiscount = discountRetail.Qty.Value;
                            int totalQtyPurchase = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).Sum(c => c.qty);
                            if (totalQtyPurchase >= totalQtyDiscount)
                            {
                                discountApi[i].status = 1;
                            }
                            else
                            {
                                discountApi[i].status = 0;
                            }
                            //add for discountapiitems
                            if (discountRetail.Qty == 0)
                            {
                                try
                                {
                                    discountApi[i].discountApiItems = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).OrderBy(c => c.amountDiscount).Take(1).ToList();

                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    discountApi[i].discountApiItems = listDiscountApiItems.Where(c => c.discountCode == discountRetail.DiscountCode).OrderBy(c => c.amountDiscount).Take(discountRetail.Qty.Value).ToList();

                                }
                                catch { }

                            }
                            list.Add(discountApi[i]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            return list;
        }

        private List<DiscountItemAPI> getListDiscountItem(List<DiscountApi> discountApiPram)
        {
            List<DiscountItemAPI> discountItemApi = new List<DiscountItemAPI>();
            for (int i = 0; i < discountApiPram.Count; i++)
            {

                DiscountItemAPI discountItemAPI = new DiscountItemAPI();
                discountItemAPI.articleId = discountApiPram[i].articleId;
                discountItemAPI.amountDiscount = discountApiPram[i].totalDiscount;
                discountItemAPI.articleIdFk = discountApiPram[i].articleIdFk;

                if (discountItemAPI.amountDiscount > 0)
                {

                    discountItemAPI.discountDesc = discountApiPram[i].discountPercent.ToString() != "0" ? discountApiPram[i].discountPercent.ToString() + " %" : (Math.Floor(discountApiPram[i].discountAmount) / 1000) + "K";
                }
                else
                {
                    discountItemAPI.discountDesc = "";
                }

                discountItemAPI.discountCode = discountApiPram[i].discountCode;
                discountItemAPI.discountType = discountApiPram[i].discountType.ToString();
                // discountItemApi.Add(discountItemAPI);

                if (discountApiPram[i].discountType.ToString() == Config.RetailEnum.discountNormal.ToString())
                {
                    discountItemApi.Add(discountItemAPI);
                }
            }
            // DiscountItemAPI listItemApi = elimintateBogo(transactionAPI);
            //discountItemApi.Add(listItemApi);

            List<DiscountItemAPI> discountItemApiGroup = discountItemApi.Where(c => c.articleId != null | c.amountDiscount > 0).GroupBy(item => item.articleId)
             .Select(grp => grp.Aggregate((max, cur) => (max == null
             || cur.amountDiscount > max.amountDiscount) ? cur : max)).ToList();

            return discountItemApiGroup;
        }

        private List<DiscountItemAPI> trimDiscountEmployee(List<DiscountItemAPI> discItemApiEmployee, String employeeCode)
        {
            int numberOfUsedDiscount = 0;
            try
            {
                List<Models.Transaction> list = _context.Transaction.Where(c => c.Text1 == employeeCode && c.TransactionDate.Value.Month == DateTime.Now.Month).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    Models.Transaction trans = list[i];
                    int numbLineDisc = _context.TransactionLines.Where(c => c.TransactionId == trans.Id && c.Discount > 0).Sum(c => c.Qty);
                    numberOfUsedDiscount = numberOfUsedDiscount + numbLineDisc;
                }

            }
            catch
            {


            }
            List<DiscountItemAPI> listDiscountItemAPi = new List<DiscountItemAPI>();
            if (numberOfUsedDiscount < 3)
            {
                if ((discItemApiEmployee.Count + numberOfUsedDiscount) >= 3)
                {
                    listDiscountItemAPi = discItemApiEmployee.OrderByDescending(c => c.amountDiscount).ToList();
                    for (int i = 0; i < listDiscountItemAPi.Count; i++)
                    {
                        if (i > (2 - numberOfUsedDiscount))
                        {
                            listDiscountItemAPi[i].amountDiscount = 0;
                            listDiscountItemAPi[i].discountDesc = "";
                            listDiscountItemAPi[i].discountCode = "";
                            // listDiscountItemAPi.Remove(listDiscountItemAPi[i]);
                        }
                    }
                }
                else
                {
                    listDiscountItemAPi = discItemApiEmployee;
                }
            }
            return listDiscountItemAPi;
        }
    }
}