using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIModel
{
    public class APIModel
    {
    }

    public class StoreAPIModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Regional { get; set; }
        public int? StoreTypeId { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string WarehouseId { get; set; }
        public int? TargetQty { get; set; }
        public decimal? TargetValue { get; set; }
        public DateTime? DateOpen { get; set; }

    }

    public class StoreTypeAPIModel
    {
        public int Id { get; set; }
        public string TypeId { get; set; }
        public string Name { get; set; }
        public bool? StoreInStore { get; set; }
        public string InforOrderTypeNormal { get; set; }
        public string InforOrderTypeRetur { get; set; }
        public string InforXrcdnormal { get; set; }
        public string InforXrcdretur { get; set; }
    }

    public class CustomerGroupAPIModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CustomerAPIModel
    {
        public int Id { get; set; }
        public string CustId { get; set; }
        public string Name { get; set; }
        public int CustGroupId { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? StoreId { get; set; }
        public string DefaultCurr { get; set; }
    }

    public class ItemDimensionAPIModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    
    public class WarehouseAPIModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string Regional { get; set; }
        public string Division { get; set; }
        public int? StoreId { get; set; }

    }

    public class ItemAPIModel
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string DepartmentType { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroupDesc { get; set; }
        public string ItemIdAlias { get; set; }
        public bool? IsServiceItem { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
    }

    public class ItemGroupAPIModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class DashboardAPIModel
    {
        public decimal? total1 { get; set; }
        public decimal? total2 { get; set; }
        public decimal? total3 { get; set; }
        public decimal? total4 { get; set; }
        public int doAll { get; set; }
        public int doOpen { get; set; }
        public int total { get; set; }
        public int harian { get; set; }
        public int employee { get; set; }
        public int employeeActive { get; set; }
    }

    public class BasketSizeAPIModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public int trns1 { get; set; }
        public int qty1 { get; set; }
        public int bs1 { get; set; }
        public int trns2 { get; set; }
        public int qty2 { get; set; }
        public int bs2 { get; set; }
        public int grandTrns { get; set; }
        public int grandQty { get; set; }
        public int grandBS { get; set; }
    }

    public class ObjectAPIModel
    {
        public string param { get; set; }

        public Object value { get; set; }

        public DbType typeValue { get; set; }
    }

    public class EnumAPIModel
    {
        public int code { get; set; }

        public string name { get; set; }
    }

    public class MutasiAPIModel
    {
        public string noReturn { get; set; }

        public DateTime tanggalRetur { get; set; }

        public string showroom { get; set; }

        public string showroomTujuan { get; set; }

        public string keterangan { get; set; }

        public string status { get; set; }

        public List<MutasiLineAPIModel> lines { get; set; }

    }

    public class MutasiLineAPIModel
    {
        public string articleId { get; set; }

        public string articleName { get; set; }

        public string color { get; set; }

        public string size { get; set; }

        public decimal qty { get; set; }

        public decimal price { get; set; }

        public decimal total { get; set; }
    }    
}
