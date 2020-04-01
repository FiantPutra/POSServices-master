using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/Promotion")]
    [ApiController]
    public class PromotionController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public PromotionController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // List
        private int indexItem(int id, List<Item> itemList)
        {
            int value = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexitemBrand(int id, List<ItemDimensionBrand> itemList)
        {
            int value = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexItemColor(int id, List<ItemDimensionColor> itemList)
        {
            int value = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexCustomergroup(int id, List<CustomerGroup> custList)
        {
            int value = 0;
            for (int i = 0; i < custList.Count; i++)
            {
                if (custList[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexItemDepartment(int id, List<ItemDimensionDepartment> indxlist)
        {
            int value = 0;
            for (int i = 0; i < indxlist.Count; i++)
            {
                if (indxlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexItemType(int id, List<ItemDimensionDepartmentType> indxlist)
        {
            int value = 0;
            for (int i = 0; i < indxlist.Count; i++)
            {
                if (indxlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexItemGender(int id, List<ItemDimensionGender> indxlist)
        {
            int value = 0;
            for (int i = 0; i < indxlist.Count; i++)
            {
                if (indxlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private int indexItemSize(int id, List<ItemDimensionSize> indxlist)
        {
            int value = 0;
            for (int i = 0; i < indxlist.Count; i++)
            {
                if (indxlist[i].Id == id)
                {
                    value = i;
                    break;
                }
            }
            return value;
        }

        private String discountCategory(int code)
        {
            String data = "";
            if (code == RetailEnum.discountEmployee)
            {
                data = "Employee Discount";
            }
            else if (code == RetailEnum.discountMixAndMatch)
            {
                data = "Mix and Match Discount";
            }
            else if (code == RetailEnum.discountBuyAndGet)
            {
                data = "Buy And Get Discount";
            }
            else if (code == RetailEnum.discountNormal)
            {
                data = "Normal Discount";
            }
            else
            {
                data = "Not Spesific";
            }
            return data;
        }


        private List<DiscountSelectedItemAPI> getSelectedItem(int discountRetailIdList, String storeCode)
        {
            List<DiscountSelectedItemAPI> selectedItems = new List<DiscountSelectedItemAPI>();
            List<DiscountSelectedItemAlias> selectedItemAlias = new List<DiscountSelectedItemAlias>();
            try
            {
                var listItemSelectedMaster = _context.DiscountItemSelected.Where(c => c.DiscountId == discountRetailIdList).ToList();
                var itemMaster = _context.Item;
                var inventoryMaster = _context.InventoryLines.Where(c => c.WarehouseId == storeCode);
                var itemSelected =
                    from listItemSelected in listItemSelectedMaster
                    join item in itemMaster on listItemSelected.ItemId equals item.Id
                    join inventory in inventoryMaster on item.Id equals inventory.ItemId
                    select new DiscountSelectedItemAlias
                    {
                        id = item.Id,
                        promotionIdFk = discountRetailIdList,
                        articleId = item.ItemId,
                        articleIdAlias = item.ItemIdAlias,
                        articleName = item.Name,                        
                        price = 0,                        
                        unit = "PCS"

                    };

                selectedItemAlias = itemSelected.ToList();                
            }
            catch
            {

            }
            for (int i = 0; i < selectedItemAlias.Count; i++)
            {
                DiscountSelectedItemAPI item = new DiscountSelectedItemAPI();
                DiscountSelectedItemAlias alias = selectedItemAlias[i];
                item.id = alias.id;
                item.promotionIdFk = discountRetailIdList;
                item.articleId = alias.articleId;
                item.articleName = alias.articleName;
                item.brand = alias.brand;
                item.color = alias.color;
                item.department = alias.department;
                item.departmentType = alias.departmentType;
                item.gender = alias.gender;
                item.price = 0;
                item.size = alias.size;
                item.unit = "PCS";
                selectedItems.Add(item);
            }

            return selectedItems;

        }

        [HttpGet]
        public async Task<IActionResult> getPromotion(String StoreCode)
        {
            //List<Promotion> promotions = new List<Promotion>();
            //List<Item> listitem = _context.Item.ToList();
            //List<ItemDimensionBrand> listbrand = _context.ItemDimensionBrand.ToList();
            //List<ItemDimensionColor> listcolor = _context.ItemDimensionColor.ToList();
            //List<CustomerGroup> listcust = _context.CustomerGroup.ToList();
            //List<ItemDimensionDepartment> listdepartment = _context.ItemDimensionDepartment.ToList();
            //List<ItemDimensionDepartmentType> listdepartmenttype = _context.ItemDimensionDepartmentType.ToList();
            //List<ItemDimensionGender> listgender = _context.ItemDimensionGender.ToList();
            //List<ItemDimensionSize> listsize = _context.ItemDimensionSize.ToList();
            ////search discocunt employee by customer group;
            //List<DiscountRetail> masterDiscount = _context.DiscountRetail.ToList();
            try
            {
                //    Store store = _context.Store.Where(c => c.Code == StoreCode).First(); //remark

                //    for (int k = 0; k < masterDiscount.Count; k++)
                //    //foreach (DiscountRetail dl in masterDiscount)
                //    {
                //        DiscountRetail dl = masterDiscount[k];

                //        bool exist = _context.DiscountStore.Any(c => c.StoreId == store.Id && c.DiscountId == dl.Id);
                //        if (exist == true)
                //        {
                //            List<PromotionLines> promotionLines = new List<PromotionLines>();
                //            switch (dl.DiscountCategory)
                //            {
                //                case 1: // discount employee
                //                    Promotion discount = new Promotion();
                //                    discount.id = dl.Id;
                //                    discount.description = dl.DiscountName;
                //                    discount.discountCode = dl.DiscountCode;
                //                    discount.discountName = dl.DiscountName;
                //                    discount.endDate = dl.EndDate.Value.ToShortDateString();
                //                    discount.startDate = dl.StartDate.Value.ToShortDateString();
                //                    discount.discountCategory = this.discountCategory(dl.DiscountCategory);
                //                    //promotion lines
                //                    PromotionLines lines = new PromotionLines
                //                    {
                //                        id = dl.Id,
                //                        amount = 0,
                //                        promotionIdFk = dl.Id,
                //                        bank = null,
                //                        articleId = "00",
                //                        articleName = "None",
                //                        brand = "None",
                //                        color = "None",
                //                        customerGroup = "None",
                //                        department = "None",
                //                        departmentType = "None",
                //                        gender = "None",
                //                        size = "None",
                //                        discountCode = dl.DiscountCode,
                //                        discountPercent = dl.DiscountPercent.Value,
                //                        discountPrice = 0,
                //                        qta = 0,
                //                        specialPrice = 0
                //                    };
                //                    promotionLines.Add(lines);
                //                    discount.promotionLines = promotionLines;
                //                    promotions.Add(discount);
                //                    break;
                //                case 0: //normal discount
                //                    try
                //                    {
                //                        List<PromotionLines> promotionLinesNormal = new List<PromotionLines>();
                //                        Promotion discountNormal = new Promotion();
                //                        discountNormal.id = dl.Id;
                //                        discountNormal.description = dl.DiscountName;
                //                        discountNormal.discountCode = dl.DiscountCode;
                //                        discountNormal.discountName = dl.DiscountName;
                //                        discountNormal.endDate = dl.EndDate.Value.ToShortDateString();
                //                        discountNormal.startDate = dl.StartDate.Value.ToShortDateString();
                //                        discountNormal.discountCategory = this.discountCategory(dl.DiscountCategory);
                //                        //promotion lines
                //                        List<DiscountRetailLines> discountRetailLines = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dl.Id).Take(50).ToList();
                //                        for (int i = 0; i < discountRetailLines.Count; i++)
                //                        {
                //                            // tambah ini 
                //                            DiscountRetailLines retailLinesObj = discountRetailLines[i];
                //                            int idx1 = indexItem(retailLinesObj.ArticleId, listitem);
                //                            int idx2 = indexitemBrand(retailLinesObj.BrandCode, listbrand);
                //                            int idx3 = indexItemColor(retailLinesObj.Color, listcolor);
                //                            int idx4 = indexCustomergroup(dl.CustomerGroupId, listcust);
                //                            int idx5 = indexItemDepartment(retailLinesObj.Department, listdepartment);
                //                            int idx6 = indexItemType(retailLinesObj.DepartmentType, listdepartmenttype);
                //                            int idx7 = indexItemGender(retailLinesObj.Gender, listgender);
                //                            int idx8 = indexItemSize(retailLinesObj.Size, listsize);

                //                            Item item = listitem[idx1];
                //                            ItemDimensionBrand itembrand = listbrand[idx2];
                //                            ItemDimensionColor itemcolor = listcolor[idx3];
                //                            CustomerGroup custgrup = listcust[idx4];
                //                            ItemDimensionDepartment itemdepartment = listdepartment[idx5];
                //                            ItemDimensionDepartmentType itemtype = listdepartmenttype[idx6];
                //                            ItemDimensionGender itemgender = listgender[idx7];
                //                            ItemDimensionSize itemsize = listsize[idx8];
                //                            //
                //                            PromotionLines linesNormal = new PromotionLines
                //                            {
                //                                id = retailLinesObj.Id,
                //                                amount = 0,
                //                                promotionIdFk = dl.Id,
                //                                //  articleId = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().ItemId,
                //                                articleId = retailLinesObj.ArticleId.ToString() == "" ? null : item.ItemId,
                //                                // articleName = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().Name,
                //                                articleName = retailLinesObj.ArticleId.ToString() == "" ? null : item.Name,
                //                                bank = null,
                //                                //  brand = retailLinesObj.BrandCode.ToString() == "" ? null : _context.ItemDimensionBrand.Where(c => c.Id == retailLinesObj.BrandCode).First().Description,
                //                                brand = retailLinesObj.BrandCode.ToString() == "" ? null : itembrand.Description,
                //                                //   color = retailLinesObj.Color.ToString() == "" ? null : _context.ItemDimensionColor.Where(c => c.Id == retailLinesObj.Color).First().Description,
                //                                color = retailLinesObj.Color.ToString() == "" ? null : itemcolor.Description,
                //                                //customerGroup = _context.CustomerGroup.Where(c => c.Id == dl.CustomerGroupId).First().Description,
                //                                customerGroup = custgrup.Description,
                //                                //  department = retailLinesObj.Department.ToString() == "" ? null : _context.ItemDimensionDepartment.Where(c => c.Id == retailLinesObj.Department).First().Description,
                //                                department = retailLinesObj.Department.ToString() == "" ? null : itemdepartment.Description,
                //                                // departmentType = retailLinesObj.DepartmentType.ToString() == "" ? null : _context.ItemDimensionDepartmentType.Where(c => c.Id == retailLinesObj.DepartmentType).First().Description,
                //                                departmentType = retailLinesObj.DepartmentType.ToString() == "" ? null : itemtype.Description,
                //                                // gender = retailLinesObj.Gender.ToString() == "" ? null : _context.ItemDimensionGender.Where(c => c.Id == retailLinesObj.Gender).First().Description,
                //                                gender = retailLinesObj.Gender.ToString() == "" ? null : itemgender.Description,
                //                                //size = retailLinesObj.Size.ToString() == "" ? null : _context.ItemDimensionSize.Where(c => c.Id == retailLinesObj.Size).First().Description,
                //                                size = retailLinesObj.Size.ToString() == "" ? null : itemsize.Description,
                //                                discountCode = dl.DiscountCode,
                //                                discountPercent = retailLinesObj.DiscountPrecentage.HasValue ? retailLinesObj.DiscountPrecentage.Value : 0,
                //                                discountPrice = retailLinesObj.CashDiscount.HasValue ? retailLinesObj.CashDiscount.Value : 0,
                //                                qta = retailLinesObj.Qty.HasValue ? retailLinesObj.Qty.Value : 0,
                //                                specialPrice = 0
                //                            };
                //                            promotionLinesNormal.Add(linesNormal);
                //                        }
                //                        discountNormal.promotionLines = promotionLinesNormal;
                //                        promotions.Add(discountNormal);
                //                    }
                //                    catch (Exception ex)
                //                    {


                //                    }
                //                    break;
                //                case 2: //mix and match discount
                //                    List<PromotionLines> promotionLinesMixMatch = new List<PromotionLines>();
                //                    Promotion discountMixMatch = new Promotion();
                //                    discountMixMatch.id = dl.Id;
                //                    discountMixMatch.description = dl.DiscountName;
                //                    discountMixMatch.discountCode = dl.DiscountCode;
                //                    discountMixMatch.discountName = dl.DiscountName;
                //                    discountMixMatch.endDate = dl.EndDate.Value.ToShortDateString();
                //                    discountMixMatch.startDate = dl.StartDate.Value.ToShortDateString();
                //                    discountMixMatch.discountCategory = this.discountCategory(dl.DiscountCategory);

                //                    //promotion lines
                //                    List<DiscountRetailLines> discountRetailLinesMixAndMatch = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dl.Id).Take(10).ToList();
                //                    for (int i = 0; i < discountRetailLinesMixAndMatch.Count; i++)
                //                    {
                //                        DiscountRetailLines retailLinesObj = discountRetailLinesMixAndMatch[i];
                //                        PromotionLines linesNormal = new PromotionLines
                //                        {
                //                            id = retailLinesObj.Id,
                //                            amount = retailLinesObj.AmountTransaction.HasValue ? retailLinesObj.AmountTransaction.Value : 0,
                //                            promotionIdFk = dl.Id,
                //                            articleId = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().ItemId,
                //                            articleName = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().Name,
                //                            bank = null,
                //                            brand = retailLinesObj.BrandCode.ToString() == "" ? null : _context.ItemDimensionBrand.Where(c => c.Id == retailLinesObj.BrandCode).First().Description,
                //                            color = retailLinesObj.Color.ToString() == "" ? null : _context.ItemDimensionColor.Where(c => c.Id == retailLinesObj.Color).First().Description,
                //                            customerGroup = _context.CustomerGroup.Where(c => c.Id == dl.CustomerGroupId).First().Description,
                //                            department = retailLinesObj.Department.ToString() == "" ? null : _context.ItemDimensionDepartment.Where(c => c.Id == retailLinesObj.Department).First().Description,
                //                            departmentType = retailLinesObj.DepartmentType.ToString() == "" ? null : _context.ItemDimensionDepartmentType.Where(c => c.Id == retailLinesObj.DepartmentType).First().Description,
                //                            gender = retailLinesObj.Gender.ToString() == "" ? null : _context.ItemDimensionGender.Where(c => c.Id == retailLinesObj.Gender).First().Description,
                //                            size = retailLinesObj.Size.ToString() == "" ? null : _context.ItemDimensionSize.Where(c => c.Id == retailLinesObj.Size).First().Description,
                //                            discountCode = dl.DiscountCode,
                //                            discountPercent = retailLinesObj.DiscountPrecentage.HasValue ? retailLinesObj.DiscountPrecentage.Value : 0,
                //                            discountPrice = retailLinesObj.DiscountPrice.HasValue ? retailLinesObj.DiscountPrice.Value : 0,
                //                            qta = retailLinesObj.Qty.HasValue ? retailLinesObj.Qty.Value : 0,
                //                            specialPrice = 0
                //                        };
                //                        promotionLinesMixMatch.Add(linesNormal);
                //                    }
                //                    discountMixMatch.promotionLines = promotionLinesMixMatch;
                //                    promotions.Add(discountMixMatch);
                //                    break;

                //                case 3: //buy and get
                //                    List<PromotionLines> promotionLinesBuyGet = new List<PromotionLines>();
                //                    Promotion discountBuyGet = new Promotion();
                //                    discountBuyGet.id = dl.Id;
                //                    discountBuyGet.description = dl.DiscountName;
                //                    discountBuyGet.discountCode = dl.DiscountCode;
                //                    discountBuyGet.discountName = dl.DiscountName;
                //                    discountBuyGet.endDate = dl.EndDate.Value.ToShortDateString();
                //                    discountBuyGet.startDate = dl.StartDate.Value.ToShortDateString();
                //                    discountBuyGet.discountCategory = this.discountCategory(dl.DiscountCategory);

                //                    //promotion lines
                //                    List<DiscountRetailLines> discountRetailLinesBuyGet = _context.DiscountRetailLines.Where(c => c.DiscountRetailId == dl.Id).Take(10).ToList();
                //                    for (int i = 0; i < discountRetailLinesBuyGet.Count; i++)
                //                    {
                //                        DiscountRetailLines retailLinesObj = discountRetailLinesBuyGet[i];
                //                        PromotionLines linesNormal = new PromotionLines
                //                        {
                //                            id = retailLinesObj.Id,
                //                            amount = retailLinesObj.AmountTransaction.HasValue ? retailLinesObj.AmountTransaction.Value : 0,
                //                            promotionIdFk = dl.Id,
                //                            articleId = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().ItemId,
                //                            articleName = retailLinesObj.ArticleId.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleId).First().Name,
                //                            bank = null,
                //                            brand = retailLinesObj.BrandCode.ToString() == "" ? null : _context.ItemDimensionBrand.Where(c => c.Id == retailLinesObj.BrandCode).First().Description,
                //                            color = retailLinesObj.Color.ToString() == "" ? null : _context.ItemDimensionColor.Where(c => c.Id == retailLinesObj.Color).First().Description,
                //                            customerGroup = _context.CustomerGroup.Where(c => c.Id == dl.CustomerGroupId).First().Description,
                //                            department = retailLinesObj.Department.ToString() == "" ? null : _context.ItemDimensionDepartment.Where(c => c.Id == retailLinesObj.Department).First().Description,
                //                            departmentType = retailLinesObj.DepartmentType.ToString() == "" ? null : _context.ItemDimensionDepartmentType.Where(c => c.Id == retailLinesObj.DepartmentType).First().Description,
                //                            gender = retailLinesObj.Gender.ToString() == "" ? null : _context.ItemDimensionGender.Where(c => c.Id == retailLinesObj.Gender).First().Description,
                //                            size = retailLinesObj.Size.ToString() == "" ? null : _context.ItemDimensionSize.Where(c => c.Id == retailLinesObj.Size).First().Description,
                //                            discountCode = dl.DiscountCode,
                //                            discountPercent = retailLinesObj.DiscountPrecentage.HasValue ? retailLinesObj.DiscountPrecentage.Value : 0,
                //                            discountPrice = retailLinesObj.DiscountPrice.HasValue ? retailLinesObj.DiscountPrice.Value : 0,
                //                            qta = retailLinesObj.Qty.HasValue ? retailLinesObj.Qty.Value : 0,
                //                            specialPrice = 0,
                //                            articleIdDiscount = retailLinesObj.ArticleIdDiscount.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleIdDiscount).First().ItemId,
                //                            articleNameDiscount = retailLinesObj.ArticleIdDiscount.ToString() == "" ? null : _context.Item.Where(c => c.Id == retailLinesObj.ArticleIdDiscount).First().Name,

                //                        };
                //                        promotionLinesBuyGet.Add(linesNormal);
                //                    }
                //                    discountBuyGet.promotionLines = promotionLinesBuyGet;
                //                    discountBuyGet.discountItems = getSelectedItem(dl.Id, StoreCode);
                //                    promotions.Add(discountBuyGet);

                //                    break;
                //            }
                //        }                    
                //    }

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
            //return Ok(promotions.OrderBy(c => c.status));
        }
    }
}