using POSServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIModel
{
    public class DiscountRetailAPIoff
    {
        public int Id { get; set; }
        public int DiscountCategory { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public int CustomerGroupId { get; set; }
        public string DiscountPartner { get; set; }
        public string Description { get; set; }
        public int? DiscountType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? discountCash { get; set; }
        public int? qty { get; set; }
        public decimal? amount { get; set; }
        public int? priority { get; set; }
        public bool? triggerSt { get; set; }
    }
    public class DiscountStoreAPI
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int DiscountId { get; set; }
    }
    public class DiscountRetailLinesAPIoff
    {
        public int Id { get; set; }
        public int BrandCode { get; set; }
        public int Department { get; set; }
        public int DepartmentType { get; set; }
        public int Gender { get; set; }
        public int ArticleId { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public int DiscountRetailId { get; set; }
        public decimal? DiscountPrecentage { get; set; }
        public decimal? CashDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? Qty { get; set; }
        public decimal? AmountTransaction { get; set; }
        public int ArticleIdDiscount { get; set; }
    }

    public class RequestOrderLineOff
    {
        // public Article article { get; set; }
        //public int articleIdFk { get; set; }
        public string articleAlias { get; set; }
        public int id { get; set; }
        public int quantity { get; set; }
        public string requestOrderId { get; set; }
        public int requestOrderIdFk { get; set; }
        public string unit { get; set; }
    }
    public class RequestOrderOff
    {
        public string storeCode { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public string sequenceNumber { get; set; }
        public string requestDeliveryDate { get; set; }
        public string requestOrderId { get; set; }
        public IList<RequestOrderLineOff> requestOrderLines { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String warehouseId { get; set; }
        public String customerIdStore { set; get; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public String oldSJ { get; set; }
    }


    public class ReturnOrderLineOff
    {
        // public Article article { get; set; }
        // public int articleIdFk { get; set; }
        public string articleAlias { get; set; }
        public int id { get; set; }
        public int quantity { get; set; }
        public string returnOrderId { get; set; }
        public int returnOrderIdFk { get; set; }
        public string unit { get; set; }
    }
    public class ReturnOrderOff
    {
        public string storeCode { get; set; }
        public string sequenceNumber { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public string remark { get; set; }
        public string returnOrderId { get; set; }
        public IList<ReturnOrderLineOff> returnOrderLines { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String warehouseId { get; set; }
        public String oldSJ { get; set; }
    }

    public class MutasiOrderLineOff
    {
        //public Article article { get; set; }
        //public int articleIdFk { get; set; }
        public string articleAlias { get; set; }
        public string articleName { get; set; }
        public int id { get; set; }
        public string mutasiOrderId { get; set; }
        public int mutasiOrderIdFk { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
    }


    public class MutasiOrderOff
    {
        public string storeCode { get; set; }
        public string sequenceNumber { get; set; }
        public string transactionId { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public string mutasiFromWarehouse { get; set; }
        public string mutasiToWarehouse { get; set; }
        public string mutasiOrderId { get; set; }
        public IList<MutasiOrderLineOff> mutasiOrderLines { get; set; }
        public string requestDeliveryDate { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public String oldSJ { get; set; }
    }

    public class DeliveryOrderLineOff
    {
        //public Article article { get; set; }
        public string articleAlias { get; set; }
        public int articleIdFk { get; set; }
        public string deliveryOrderId { get; set; }
        public int deliveryOrderIdFk { get; set; }
        public int id { get; set; }
        public int URRIDL { get; set; }
        public int? qtyDeliver { get; set; }
        public int? qtyReceive { get; set; }
        public decimal? amount { get; set; }
        public String packingNumber { get; set; }

    }
    public class DeliveryOrderOff
    {
        public string date { get; set; }
        public string deliveryDate { get; set; }
        public string deliveryOrderId { get; set; }
        public IList<DeliveryOrderLineOff> deliveryOrderLines { get; set; }
        public string deliveryTime { get; set; }
        public int id { get; set; }
        public int? status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int? totalQty { get; set; }
        public string storeCode { get; set; }
        public string warehouseFrom { get; set; }
        public string warehouseTo { get; set; }
        public string CustomerIdStore { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public decimal? totalAmount { set; get; }
    }

    public class GetInvenOFFline
    {
        public int id { get; set; }
        public string warehouseCode { get; set; }
        public int itemId { get; set; }
        public int qty { get; set; }
        public IList<GetInvenOFFlineLines> doLines { get; set; }
    }

    public class GetInvenOFFlineLines
    {
        public int transactionType { get; set; }
        public string transactionTypeName { get; set; }
        public string transRefId { get; set; }
        public int qty { get; set; }
        public int transactionLinesId { get; set; }
        public DateTime transDate { get; set; }
    }


    public class TransactionLineAPI
    {
        public string articleId { get; set; }
        public int qty { get; set; }
        public decimal unitPrice { get; set; }
        public decimal amount { get; set; }
        public decimal? discount { get; set; }
        public long transactionId { get; set; }
        public string articleName { get; set; }
        public int? discountType { get; set; }
        public string discountCode { get; set; }
        public string spgid { get; set; }
        public string articleIdAlias { get; set; }
        //public Article article { get; set; }
        //public int articleIdFk { get; set; }
        //public int id { get; set; }
        //public decimal price { get; set; }
        //public int quantity { get; set; }
        //public decimal subtotal { get; set; }
        //public string transactionId { get; set; }
        //public int transactionIdFk { get; set; }
        //public decimal discount { get; set; }
        //public int discountType { get; set; }
        //public String discountCode { get; set; }
        //public String spgId { set; get; }
    }

    public class DeliveryOrderApprovalLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public string deliveryOrderId { get; set; }
        public int deliveryOrderIdFk { get; set; }
        public int id { get; set; }
        public int? qtyDeliver { get; set; }
        public int? qtyReceive { get; set; }
        public decimal? amount { get; set; }
        public String packingNumber { get; set; }


    }
    public class DeliveryOrderApproval
    {
        public string date { get; set; }
        public string deliveryDate { get; set; }
        public string deliveryOrderId { get; set; }
        public IList<DeliveryOrderApprovalLine> deliveryOrderLines { get; set; }
        public string deliveryTime { get; set; }
        public int id { get; set; }
        public int? status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int? totalQty { get; set; }
        public string storeCode { get; set; }
        public string transactionId { get; set; }
        public string warehouseFrom { get; set; }
        public string warehouseTo { get; set; }
        public string CustomerIdStore { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public decimal? totalAmount { set; get; }

        //add by frank for transaction type
        public string transactionHOType { set; get; }
    }
    public class TransactionAPI
    {
        //  public string storeCode { get; set; }
        public decimal cash { get; set; }
        public decimal change { get; set; }
        //  public string customerIdStore { get; set; }

        //   public string SequenceNumber { get; set; }
        public string customerId { get; set; }
        public string currency { get; set; }
        public string date { get; set; }
        public decimal discount { get; set; }
        public string employeeId { get; set; }
        //  public int id { get; set; }
        public string paymentType { get; set; }
        public string receiptId { get; set; }
        public string spgId { get; set; }
        // public int status { get; set; }
        //    public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal total { get; set; }
        public decimal Edc1 { get; set; }
        public decimal Edc2 { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public string transactionId { get; set; }
        // public int transactionType { get; set; }
        public IList<TransactionLineAPI> transactionLines { get; set; }
    }
    public class ResultLine
    {

        public int qty { set; get; }
        public decimal amount { set; get; }
        public String category { set; get; }
    }
    public class StockTakeLineAPI
    {
        public int id { set; get; }
        public int articleId { set; get; }
        public Article article { set; get; }
        public int goodQty { set; get; }
        public int rejectQty { set; get; }
        public int whGoodQty { set; get; }
        public int whRejectQty { set; get; }
        public int status { set; get; }
    }

    public class LastSequenceNumber
    {
        public string transactionId { get; set; }
        public string closingStoreId { get; set; }
        public string closingShiftId { get; set; }
        public string pettyCashId { get; set; }
        public string requestOrderId { get; set; }
        public string mutasiOrderId { get; set; }
        public string returnOrderId { get; set; }
    }
    public class StockTakeAPI
    {
        public String employeeId { set; get; }
        public String employeeName { set; get; }
        public String storeCode { set; get; }
        public List<StockTakeLineAPI> stockTakeLines { set; get; }
    }

    public class PettyCashLine
    {
        public string expenseName { get; set; }
        public int id { get; set; }
        public string pettyCashId { get; set; }
        public int pettyCashIdFk { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal total { get; set; }
    }

    public class PettyCash
    {

        public string storeCode { get; set; }
        public string customerIdStore { get; set; }

        public string sequenceNumber { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public String expenseCategoryId { set; get; }
        public string expenseCategory { get; set; }
        public string expenseDate { get; set; }
        public int id { get; set; }
        public string pettyCashId { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal totalExpense { get; set; }
        public IList<PettyCashLine> pettyCashLine { get; set; }
    }

    public class ClosingStoreAPI
    {
        public String closingStoreId { set; get; }
        public String storeCode { set; get; }
        public String openingTimestamp { set; get; }
        public string sequenceNumber { get; set; }
        public String closingTimestamp { set; get; }
        public decimal openingTransBal { set; get; }
        public decimal closingTransBal { set; get; }
        public decimal realTransBal { set; get; }
        public decimal disputeTransBal { set; get; }
        public decimal openingPettyCash { set; get; }
        public decimal closingPettyCash { set; get; }
        public decimal realPettyCash { set; get; }
        public decimal disputePettyCash { set; get; }
        public decimal openingDeposit { set; get; }
        public decimal closingDeposit { set; get; }
        public decimal realDeposit { set; get; }
        public decimal disputeDeposit { set; get; }
        public String deviceName { set; get; }
        public String statusClose { set; get; }
        public String employeeId { set; get; }
        public String employeeName { set; get; }

    }

    public partial class ClosingStoreDetails
    {
        public string ClosingStoreId { get; set; }
        public string StoreCode { get; set; }
        public DateTime? OpeningTimeStamp { get; set; }
        public DateTime? ClosingTimeStamp { get; set; }
        public decimal? OpeningTransBal { get; set; }
        public decimal? ClosignTranBal { get; set; }
        public decimal? RealTransBal { get; set; }
        public decimal? DisputeTransBal { get; set; }
        public decimal? OpeningPettyCash { get; set; }
        public decimal? ClosingPettyCash { get; set; }
        public decimal? RealPettyCash { get; set; }
        public decimal? DisputePettyCash { get; set; }
        public decimal? OpeningDeposit { get; set; }
        public decimal? ClosingDeposit { get; set; }
        public decimal? RealDeposit { get; set; }
        public decimal? DisputeDeposit { get; set; }
        public IList<Transaction> transaction { get; set; }
    }
    public class ClosingShiftAPI
    {
        public String closingShiftId { set; get; }
        public String storeCode { set; get; }
        public String shiftCode { set; get; }
        public String openingTimestamp { set; get; }
        public String closingTimestamp { set; get; }
        public decimal openingTransBal { set; get; }
        public decimal closingTransBal { set; get; }
        public decimal realTransBal { set; get; }
        public decimal disputeTransBal { set; get; }
        public string sequenceNumber { get; set; }
        public decimal openingPettyCash { set; get; }
        public decimal closingPettyCash { set; get; }
        public decimal realPettyCash { set; get; }
        public decimal disputePettyCash { set; get; }
        public decimal openingDeposit { set; get; }
        public decimal closingDeposit { set; get; }
        public decimal realDeposit { set; get; }
        public decimal disputeDeposit { set; get; }
        public String deviceName { set; get; }
        public String statusClose { set; get; }
        public String employeeId { set; get; }
        public String employeeName { set; get; }


    }

    public class CurrencyAPI
    {
        public String sign { get; set; }
        public String name { get; set; }
        public List<Denomination> denominations { get; set; }
    }

    public class Denomination
    {
        public int id { get; set; }
        public int currencyIdFk { get; set; }
        public decimal nominal { get; set; }
    }
    public class Promotion
    {
        public string description { get; set; }
        public string discountCategory { get; set; }
        public string discountCode { get; set; }
        public string discountName { get; set; }
        public string endDate { get; set; }
        public int id { get; set; }
        public string startDate { get; set; }
        public int status { get; set; }
        public List<PromotionLines> promotionLines { set; get; }
        public List<DiscountSelectedItemAPI> discountItems { set; get; }
    }

    public class PromotionLines
    {
        public int id { set; get; }
        public int promotionIdFk { set; get; }
        public String discountCode { set; get; }
        public String articleId { set; get; }
        public String articleName { set; get; }
        public String brand { set; get; }
        public String size { set; get; }
        public String color { set; get; }
        public String gender { set; get; }
        public String department { set; get; }
        public String departmentType { set; get; }
        public String customerGroup { set; get; }
        public int qta { set; get; }
        public decimal amount { set; get; }
        public String bank { set; get; }
        public decimal discountPercent { set; get; }
        public decimal discountPrice { set; get; }
        public decimal specialPrice { set; get; }
        public String articleIdDiscount { set; get; }
        public String articleNameDiscount { set; get; }
    }

    public class DiscountMaster
    {
        public List<DiscountItemAPI> discountItems { set; get; }
        public List<DiscountApi> discounts { set; get; }

    }
    public class DiscountSelectedItemAPI
    {
        public int id { set; get; }
        public int promotionIdFk { set; get; }
        public String articleId { set; get; }
        public String articleName { set; get; }
        public String brand { set; get; }
        public String gender { set; get; }
        public String department { set; get; }
        public String departmentType { set; get; }
        public String size { set; get; }
        public String color { set; get; }
        public String unit { set; get; }
        public Double price { set; get; }
    }

    public class DiscountSelectedItemAlias
    {
        public int id { set; get; }
        public int promotionIdFk { set; get; }
        public String articleId { set; get; }
        public String articleIdAlias { set; get; }
        public String articleName { set; get; }
        public String brand { set; get; }
        public String gender { set; get; }
        public String department { set; get; }
        public String departmentType { set; get; }
        public String size { set; get; }
        public String color { set; get; }
        public String unit { set; get; }
        public Double price { set; get; }
    }

    public class DiscountApi
    {
        public int id { get; set; }
        public String discountCode { set; get; }
        public int discountItem { get; set; }
        public decimal discountPercent { get; set; }
        public decimal discountAmount { get; set; }
        public int status { get; set; }
        public int discountType { set; get; }
        public String articleId { set; get; }
        public long articleIdFk { set; get; }
        public decimal totalDiscount { set; get; }
        public String discountDesc { set; get; }
        public List<DiscountApiItem> discountApiItems { set; get; }

    }

    public class DiscountItemAPI
    {
        public String articleId { set; get; }
        public decimal amountDiscount { set; get; }
        public long articleIdFk { set; get; }
        public String discountDesc { set; get; }
        public String discountCode { set; get; }
        public String discountType { set; get; }

    }
    public class CostCategoryMaster
    {
        public int Id { get; set; }
        public string CostCategoryId { get; set; }
        public string Name { get; set; }
        public string Coa { get; set; }
    }
    public class TransactionHistory
    {
        public long Id { get; set; }
        public int? StoreId { get; set; }
        public string StoreCode { get; set; }
        public int? CustomerId { get; set; }
        public string RecieptCode { get; set; }
        public int? EmployeeId { get; set; }
        public int? Spgid { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalAmounTransaction { get; set; }
        public int? Qty { get; set; }
        public string MethodOfPayment { get; set; }
        public decimal? MarginTransaction { get; set; }
        public string TransactionId { get; set; }
        public string ShiftCode { get; set; }
        public int? TransactionType { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Edc1 { get; set; }
        public decimal? Edc2 { get; set; }
        public decimal? Change { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public int? VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public decimal? Voucher { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TimeSpan? TransactionTime { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Orno { get; set; }
        public DateTime? TransDateStore { get; set; }
        public IList<TransactionLineHistory> transactionLines { get; set; }
    }

    public class TransactionLineHistory
    {
        public long Id { get; set; }
        public string ArticleId { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public long TransactionId { get; set; }
        public string ArticleName { get; set; }
        public int? DiscountType { get; set; }
        public string DiscountCode { get; set; }
        public string Spgid { get; set; }
        public string ArticleIdAlias { get; set; }
    }
    public class CustomerData
    {
        public int id { get; set; }
        public string custId { get; set; }
        public string name { get; set; }
        public int custGroupid { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int storeId { get; set; }
        public string defaultCurr { get; set; }
    }
    public class StoreData
    {
        public string deviceCode { set; get; }
        public BudgetStore budgetStore { set; get; }
        public CurrencyAPI currency { set; get; }
        public CustomerData customerData { get; set; }
        public StoreMaster store { set; get; }
        public WarehouseMaster warehouse { set; get; }
        public List<BankMaster> banks { set; get; }
        public List<EmployeeMaster> employees { set; get; }
    }

    public class BudgetStore
    {
        public decimal budget { set; get; }
        public decimal remaining { set; get; }

    }
    public class StoreMaster
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
        public string CustomerIdStore { get; set; }
    }
    public class WarehouseMaster
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
    }
    public class BankMaster
    {
        public String bankId { set; get; }
        public String bankName { set; get; }
        public string account { get; set; }
    }
    public class DiscountApiItem
    {
        public String articleId { set; get; }
        public decimal amountDiscount { set; get; }
        public long articleIdFk { set; get; }
        public String discountDesc { set; get; }
        public String discountCode { set; get; }
        public decimal price { set; get; }
        public int discountType { set; get; }
        public int qty { set; get; }
        public int DiscountRetailLinesId { set; get; }
        public int DiscountRetailId { set; get; }

    }
    public class Inventory
    {
        public int articleId { get; set; }
        public int? goodQty { get; set; }
        public int id { get; set; }
        public int rejectQty { get; set; }
        public int status { get; set; }
        public int whGoodQty { get; set; }
        public int whRejectQty { get; set; }
    }
    public class ReturnOrderLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public int id { get; set; }
        public int quantity { get; set; }
        public string returnOrderId { get; set; }
        public int returnOrderIdFk { get; set; }
        public string unit { get; set; }
    }
    public class ReturnOrder
    {
        public string storeCode { get; set; }
        public string sequenceNumber { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public string remark { get; set; }
        public string returnOrderId { get; set; }
        public IList<ReturnOrderLine> returnOrderLines { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String warehouseId { get; set; }
        public String oldSJ { get; set; }
    }
    public class RequestOrderLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public int id { get; set; }
        public int quantity { get; set; }
        public string requestOrderId { get; set; }
        public int requestOrderIdFk { get; set; }
        public string unit { get; set; }
    }
    public class RequestOrder
    {
        public string storeCode { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public string sequenceNumber { get; set; }
        public string requestDeliveryDate { get; set; }
        public string requestOrderId { get; set; }
        public IList<RequestOrderLine> requestOrderLines { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String warehouseId { get; set; }
        public String customerIdStore { set; get; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public String oldSJ { get; set; }
    }
    public class HOTransactionLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public int id { get; set; }
        public string mutasiOrderId { get; set; }
        public int mutasiOrderIdFk { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
    }
    public class MutasiOrderLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public long id { get; set; }
        public string mutasiOrderId { get; set; }
        public long mutasiOrderIdFk { get; set; }
        public decimal quantity { get; set; }
        public string unit { get; set; }
    }

    public class ChangePasswordModel
    {
        public string username { get; set; }
        public string currentpassword { get; set; }
        public string newpassword { get; set; }
    }
    public class MutasiOrder
    {
        public string storeCode { get; set; }
        public string sequenceNumber { get; set; }
        public string date { get; set; }
        public string transactionId { get; set; }
        public long id { get; set; }
        public string mutasiFromWarehouse { get; set; }
        public string mutasiToWarehouse { get; set; }
        public string mutasiOrderId { get; set; }
        public IList<MutasiOrderLine> mutasiOrderLines { get; set; }
        public string requestDeliveryDate { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal totalQty { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public string remarks { get; set; }
        public String oldSJ { get; set; }
        public bool isPbmPbk { set; get; }
        public decimal? totalAmount { set; get; }

        public int mutasiType { set; get; }
        public string mutasiTypeName { set; get; }

    }
    public class HOTransaction
    {
        public string storeCode { get; set; }
        public string sequenceNumber { get; set; }
        public string date { get; set; }
        public string transactionId { get; set; }
        public int id { get; set; }
        public string mutasiFromWarehouse { get; set; }
        public string mutasiToWarehouse { get; set; }
        public string mutasiOrderId { get; set; }
        public IList<HOTransactionLine> hoTransactionLines { get; set; }
        public string requestDeliveryDate { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public int totalQty { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public string remarks { get; set; }
        public String oldSJ { get; set; }
        public bool isPbmPbk { set; get; }
        public decimal? totalAmount { set; get; }

        public int hoTransTypeId { set; get; }
        public string hoTransTypeCode { set; get; }

    }

    public class DeliveryOrderLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public string deliveryOrderId { get; set; }
        public long deliveryOrderIdFk { get; set; }
        public int id { get; set; }
        public int? qtyDeliver { get; set; }
        public int? qtyReceive { get; set; }
        public decimal? amount { get; set; }
        public String packingNumber { get; set; }

    }
    public class DeliveryOrder
    {
        public string date { get; set; }
        public string deliveryDate { get; set; }
        public string deliveryOrderId { get; set; }
        public IList<DeliveryOrderLine> deliveryOrderLines { get; set; }
        public string deliveryTime { get; set; }
        public long id { get; set; }
        public int? status { get; set; }
        public string statusName { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal? totalQty { get; set; }
        public string storeCode { get; set; }
        public string warehouseFrom { get; set; }
        public string warehouseTo { get; set; }
        public string CustomerIdStore { get; set; }
        public String employeeId { get; set; }
        public String employeeName { get; set; }
        public decimal? totalAmount { set; get; }

        public bool isPbmPbk { set; get; }
        public String SjFIsik { set; get; }

    }
    public class Article
    {
        public string articleId { get; set; }
        public string articleIdAlias { get; set; }
        public string articleName { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public string department { get; set; }
        public string departmentType { get; set; }
        public string gender { get; set; }
        public int id { get; set; }
        public decimal price { get; set; }
        public string size { get; set; }
        public string unit { get; set; }
        public string itemGroup { get; set; }
        public string itemGroupDesc { get; set; }
        public bool isService { get; set; }

    }
    public class TransactionLine
    {
        public Article article { get; set; }
        public int articleIdFk { get; set; }
        public int id { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal subtotal { get; set; }
        public string transactionId { get; set; }
        public int transactionIdFk { get; set; }
        public decimal discount { get; set; }
        public int discountType { get; set; }
        public String discountCode { get; set; }
        public String spgId { set; get; }
    }
    public class Transaction
    {
        public string storeCode { get; set; }
        public decimal cash { get; set; }
        public decimal change { get; set; }
        public string customerIdStore { get; set; }

        public string SequenceNumber { get; set; }
        public string customerId { get; set; }
        public string currency { get; set; }
        public string date { get; set; }
        public decimal discount { get; set; }
        public string employeeId { get; set; }
        public int id { get; set; }
        public int paymentType { get; set; }
        public string receiptId { get; set; }
        public string spgId { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal total { get; set; }
        public decimal Edc1 { get; set; }
        public decimal Edc2 { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public string transactionId { get; set; }
        public int transactionType { get; set; }

        public string openShiftId { get; set; }
        public string openStoreId { get; set; }
        public IList<TransactionLine> transactionLines { get; set; }
    }
    public class TransactionMposFor
    {
        public string storeCode { get; set; }
        public decimal cash { get; set; }
        public decimal change { get; set; }
        public string customerIdStore { get; set; }
        public string openShiftId { get; set; }
        public string openStoreId { get; set; }
        public string sequenceNumber { get; set; }
        public string customerId { get; set; }
        public string currency { get; set; }
        public decimal voucher { get; set; }
        public string date { get; set; }
        public decimal discount { get; set; }
        public string employeeId { get; set; }
        public int id { get; set; }
        public int paymentType { get; set; }
        public string receiptId { get; set; }
        public string spgId { get; set; }
        public int status { get; set; }
        public string time { get; set; }
        public string timeStamp { get; set; }
        public decimal total { get; set; }
        public decimal edc1 { get; set; }
        public decimal edc2 { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public string transactionId { get; set; }
        public int transactionType { get; set; }
        public IList<TransactionLine> transactionLines { get; set; }
    }

    public class ItemDimension
    {
        public List<ItemDimensionBrand> brands { set; get; }
        public List<ItemDimensionColor> colors { set; get; }
        public List<ItemDimensionDepartment> departments { set; get; }
        public List<ItemDimensionDepartmentType> departmentTypes { set; get; }
        public List<ItemDimensionGender> genders { set; get; }
        public List<ItemDimensionSize> sizes { set; get; }

    }
    public class Authentification
    {
        public String userId { set; get; }
        public String password { set; get; }
        public String token { set; get; }
        public String deviceId { set; get; }
        public String storeId { set; get; }

    }
    public class APIResponse
    {
        public String code { set; get; }
        public String message { set; get; }
    }
    public class APIResponseHO
    {
        public String code { set; get; }
        public String message { set; get; }
        public int id { set; get; }
    }
    public class EmployeeMaster
    {
        public int id { set; get; }
        public string employeeId { set; get; }
        public string name { set; get; }
        public byte[] passwordHash { set; get; }
        public byte[] passwordSalt { set; get; }
        public string passwordaja { set; get; }
        public string storeCode { set; get; }
        public string storeName { set; get; }
        public int storeId { set; get; }

        public Possition possition { set; get; }
    }
    public class Possition
    {
        public int id { set; get; }
        public string possitionId { set; get; }
        public string possitionName { set; get; }
    }

    public class DiscountRetailAPI
    {
        public int Id { get; set; }
        public int DiscountCategory { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public int CustomerGroupId { get; set; }
        public string DiscountPartner { get; set; }
        public string Description { get; set; }
        public int? DiscountType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public int? DiscountPercent { get; set; }

        public int? Qty { get; set; }
        public int? Priority { get; set; }
        public decimal? Amount { get; set; }
    }

    public class DiscountRetailLinesAPI
    {
        public int Id { get; set; }
        public int BrandCode { get; set; }
        public int Department { get; set; }
        public int DepartmentType { get; set; }
        public int Gender { get; set; }
        public int ArticleId { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public int DiscountRetailId { get; set; }
        public decimal? DiscountPrecentage { get; set; }
        public decimal? CashDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? Qty { get; set; }
        public decimal? AmountTransaction { get; set; }
        public int ArticleIdDiscount { get; set; }
    }

    public class DiscountItemSelectedAPI
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int DiscountId { get; set; }
    }

    public class DeliveryOrderList
    {
        public List<DeliveryOrder> deliveries { get; set; }
    }
}
