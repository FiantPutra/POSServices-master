using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/StoreData")]
    [ApiController]
    public class StoreDataController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        private readonly IAuthRepository _repo;

        public StoreDataController(DB_BIENSI_POSContext context, IAuthRepository repo)
        {
            _repo = repo;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> getStore(String storeCode, String deviceId)
        {
            if (deviceId != null)
            {
                bool deviceaidi = _context.DeviceTable.Any(x => x.DeviceId == deviceId);
                if (!deviceaidi)
                {
                    DeviceTable dtnew = new DeviceTable();
                    var check = _context.DeviceTable.LastOrDefault();
                    if (check == null)
                    {
                        dtnew.InitialId = "AAA";
                    }
                    else
                    {
                        string ddv = check.InitialId;
                        ddv = getGet(ddv);
                        dtnew.InitialId = ddv;
                    }
                    dtnew.FirstLogin = DateTime.Now.ToString();
                    dtnew.TypeDevice = "POS";
                    dtnew.DeviceId = deviceId;
                    _context.DeviceTable.Add(dtnew);
                    _context.SaveChanges();

                }
                else
                {
                    DeviceTable tab = _context.DeviceTable.Where(x => x.DeviceId == deviceId).First();
                    tab.LastLogin = DateTime.Now.ToString();
                    _context.Update(tab);
                    _context.SaveChanges();
                }
            }

            StoreData storeData = new StoreData();

            if (_context.Store.Any(c => c.Code == storeCode))
            {

                //     if (employee == null)
                //     {
                //         return NotFound();
                //    }
                try
                {
                    Store store = _context.Store.Where(c => c.Code == storeCode).First();

                    try
                    {
                        storeData.deviceCode = _context.DeviceTable.Where(x => x.DeviceId == deviceId).First().InitialId;
                    }
                    catch
                    {
                        storeData.deviceCode = "";
                    }

                    try
                    {
                        storeData.store = masterStore(store.Id);
                    }
                    catch
                    {

                    }
                    try
                    {
                        storeData.warehouse = masterWarehose(_context.Store.Where(c => c.Id == store.Id).First().Code);
                    }
                    catch
                    {

                    }
                    try
                    {
                        storeData.banks = masterBanks(store.Id);
                    }
                    catch
                    {

                    }

                    try
                    {
                        storeData.employees = masterEmployee(store.Id);
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        storeData.currency = masterCurrency("IDR");
                    }
                    catch (Exception)
                    {

                    }


                    try
                    {
                        storeData.budgetStore = this.budgetStore(storeCode);
                    }
                    catch
                    {


                    }


                }
                catch (Exception ex)
                {
                    return Ok(ex.ToString());
                }
            }
            return Ok(storeData);
        }



        [HttpPost]
        public async Task<IActionResult> CheckLogin([FromBody] Authentification authentification)
        {
            var employeecode = await _context.Employee.Where(c => c.EmployeeCode == authentification.userId).Select(c => c.Id).FirstOrDefaultAsync();
            // var login = await _context.UserLogin.FirstOrDefaultAsync(x => x.UserId == Int32.Parse(username));
            var login = await _context.UserLogin.FirstOrDefaultAsync(x => x.UserId == employeecode);
            if (login == null || login.Status == false)
            {
                return StatusCode(404, new
                {
                    status = "404",
                    error = true,
                    message = "Username Not found"
                });
            }

            if (!VerifyPasswordHash(authentification.password, login.PasswordHash, login.PasswordSalt))
            {
                return StatusCode(404, new
                {
                    status = "404",
                    error = true,
                    message = "Incorrect password"
                });
            }
            //var userexist = _repo.Login(authentification.userId, authentification.password);
            //if(userexist == null)
            //{
            //    return StatusCode(404, new
            //    {
            //        status = "404",
            //        error = true,
            //        message = "Not found"
            //    });
            //}
            bool deviceaidi = _context.DeviceTable.Any(x => x.DeviceId == authentification.deviceId);
            if (!deviceaidi)
            {
                DeviceTable dtnew = new DeviceTable();
                var check = _context.DeviceTable.LastOrDefault();
                if (check == null)
                {
                    dtnew.InitialId = "AAA";
                }
                else
                {
                    string ddv = check.InitialId;
                    ddv = getGet(ddv);
                    dtnew.InitialId = ddv;
                }
                dtnew.FirstLogin = DateTime.Now.ToString();
                dtnew.TypeDevice = "MPOS";
                dtnew.DeviceId = authentification.deviceId;
                _context.DeviceTable.Add(dtnew);
                _context.SaveChanges();

            }
            else
            {
                DeviceTable tab = _context.DeviceTable.Where(x => x.DeviceId == authentification.deviceId).First();
                tab.LastLogin = DateTime.Now.ToString();
                _context.Update(tab);
                _context.SaveChanges();
            }

            String employeeId = authentification.storeId;
            StoreData storeData = new StoreData();

            //     if (employee == null)
            //     {
            //         return NotFound();
            //    }
            try
            {
                Employee employee = _context.Employee.Where(c => c.EmployeeCode == authentification.userId).First();

                storeData.deviceCode = _context.DeviceTable.Where(x => x.DeviceId == authentification.deviceId).First().InitialId;
                try
                {
                    storeData.store = masterStore(employee.StoreId);
                }
                catch
                {

                }
                try
                {
                    storeData.warehouse = masterWarehose(_context.Store.Where(c => c.Id == employee.StoreId).First().Code);
                }
                catch
                {

                }
                try
                {
                    storeData.banks = masterBanks(employee.StoreId);
                }
                catch
                {

                }

                try
                {
                    storeData.employees = masterEmployeepost(employee.StoreId);
                }
                catch (Exception)
                {


                }
                try
                {
                    storeData.currency = masterCurrency("IDR");
                }
                catch (Exception)
                {


                }

                try
                {
                    storeData.budgetStore = this.budgetStore(_context.Store.Where(c => c.Id == employee.StoreId).First().Code);
                }
                catch
                {


                }
                try
                {
                    UserLogin userheckk = _context.UserLogin.Where(x => x.UserId == employeecode).First();
                    userheckk.LastLogin = DateTime.Now;
                    _context.Update(userheckk);
                    _context.SaveChanges();
                }
                catch
                {

                }


            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
            return Ok(storeData);
            //return StatusCode(200, new
            //{
            //    status = "200",
            //    error = false,
            //    message = "Login Success",
            //    body = storeData
            //});
        }
        
        private string getGet(string ddv)
        {
            //public string GetNextPrefix(string prefixToIncrement)
            //{
            string previousPrefix = "AAA";
            char[] digits = ddv.ToCharArray();

            for (int i = previousPrefix.Length - 1; i >= 0; --i)
            {
                if (digits[i] == 'Z')
                {
                    digits[i] = 'A';
                }
                else
                {
                    digits[i] = (char)(digits[i] + 1);  //error here
                    break;
                }
            }
            //return new String(digits);
            //}

            return new String(digits);
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        private CurrencyAPI masterCurrency(String currencyCode)
        {
            Currency storeDb = _context.Currency.Where(c => c.Code == currencyCode).First();
            CurrencyAPI store = new CurrencyAPI();
            store.sign = storeDb.Description;
            store.name = storeDb.Code;

            List<Denomination> denomination = new List<Denomination>();
            List<CurrencyDenomination> currencyDenominations = _context.CurrencyDenomination.Where(c => c.CurrencyId == storeDb.Id).ToList();
            foreach (CurrencyDenomination cd in currencyDenominations)
            {
                Denomination d = new Denomination();
                d.currencyIdFk = cd.CurrencyId;
                d.id = cd.Id;
                d.nominal = cd.Nominal;
                denomination.Add(d);
            }
            store.denominations = denomination;
            return store;
        }

        private StoreMaster masterStore(int idStore)
        {
            Store storeDb = _context.Store.Where(c => c.Id == idStore).First();
            StoreMaster store = new StoreMaster();
            store.Id = idStore;
            store.Code = storeDb.Code;
            store.Name = storeDb.Name;
            store.Address = storeDb.Name;
            store.Address2 = storeDb.Address2;
            store.Address3 = storeDb.Address3;
            store.Address4 = storeDb.Address4;
            store.City = storeDb.City;
            store.Location = storeDb.Location;
            store.Regional = storeDb.Regional;
            store.StoreTypeId = storeDb.StoreTypeId;
            store.WarehouseId = storeDb.WarehouseId;
            try
            {
                store.CustomerIdStore = _context.Customer.Where(c => c.StoreId == idStore).First().CustId;

            }
            catch
            {
                //  store.CustomerIdStore = _context.Customer.Where(c => c.StoreId == 962).First().CustId;

            }

            return store;
        }

        //private List<BankMaster> masterBanks(int storeID)
        //{
        //    List<BankMaster> masterBankList = new List<BankMaster>();
        //    try
        //    {
        //        List<StorePaymentMethod> bankDb = _context.StorePaymentMethod.Where(c => c.StoreId == storeID).ToList();
        //        foreach (StorePaymentMethod b in bankDb)
        //        {
        //            Bank bankMaster = _context.Bank.Where(c => c.BankId == b.BankCode).First();
        //            BankMaster bank = new BankMaster();
        //            bank.bankId = bankMaster.BankId;
        //            bank.bankName = bankMaster.Name;
        //            masterBankList.Add(bank);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return masterBankList;
        //}

        private List<EmployeeMaster> masterEmployeepost(int storeID)
        {
            List<EmployeeMaster> masterEmployeeList = new List<EmployeeMaster>();
            try
            {
                List<Employee> dbEmployee = _context.Employee.Where(c => c.StoreId == storeID).ToList();
                foreach (Employee b in dbEmployee)
                {
                    EmployeePossition employeePossition = _context.EmployeePossition.Where(c => c.Id == b.PossitionId).First();
                    Store store = _context.Store.Where(c => c.Id == b.StoreId).First();
                    EmployeeMaster employee = new EmployeeMaster
                    {

                        employeeId = b.EmployeeCode,
                        id = b.Id,
                        name = b.EmployeeName,
                        storeId = store.Id,
                        storeCode = store.Code,
                        storeName = store.Name,

                        possition = new Possition
                        {
                            id = employeePossition.Id,
                            possitionName = employeePossition.Name,
                            possitionId = employeePossition.PossitionId
                        }
                    };

                    masterEmployeeList.Add(employee);
                }
            }
            catch
            {

            }
            return masterEmployeeList;
        }
        private WarehouseMaster masterWarehose(String codeWarehouse)
        {
            Warehouse storeDb = _context.Warehouse.Where(c => c.Code == codeWarehouse).First();
            WarehouseMaster objectData = new WarehouseMaster();
            objectData.Code = storeDb.Code;
            objectData.Name = storeDb.Name;
            objectData.Address = storeDb.Name;
            objectData.Address2 = storeDb.Address2;
            objectData.Address3 = storeDb.Address3;
            objectData.Address4 = storeDb.Address4;
            objectData.City = storeDb.City;
            objectData.Regional = storeDb.Regional;
            return objectData;
        }
        private BudgetStore budgetStore(String storeCOde)
        {

            BudgetStore b = new BudgetStore();
            try
            {
                ExpenseStore expense = _context.ExpenseStore.Where(c => c.StoreCode == storeCOde).First();
                b.budget = expense.TotalBudget.Value;
                b.remaining = expense.RemaingBudget.Value;
            }
            catch
            {
                b.budget = 0;
                b.remaining = 0;
            }


            return b;


        }
        private List<BankMaster> masterBanks(int storeID)
        {
            List<BankMaster> masterBankList = new List<BankMaster>();
            try
            {
                List<StorePaymentMethod> bankDb = _context.StorePaymentMethod.Where(c => c.StoreId == storeID).ToList();
                foreach (StorePaymentMethod b in bankDb)
                {
                    Bank bankMaster = _context.Bank.Where(c => c.BankId == b.BankCode).First();
                    BankMaster bank = new BankMaster();
                    bank.bankId = bankMaster.BankId;
                    bank.bankName = bankMaster.Name;
                    bank.account = bankMaster.Account;
                    masterBankList.Add(bank);
                }
            }
            catch
            {
            }
            return masterBankList;
        }

        //private List<EmployeeMaster> masterEmployee(int storeID)
        //{
        //    List<EmployeeMaster> masterEmployeeList = new List<EmployeeMaster>();
        //    try
        //    {
        //        List<Employee> dbEmployee = _context.Employee.Where(c => c.StoreId == storeID).ToList();
        //        foreach (Employee b in dbEmployee)
        //        {
        //            EmployeePossition employeePossition = _context.EmployeePossition.Where(c => c.Id == b.PossitionId).First();
        //            EmployeeMaster employee = new EmployeeMaster
        //            {
        //                employeeId = b.EmployeeCode,
        //                id = b.Id,
        //                name = b.EmployeeName,
        //                possition = new Possition
        //                {
        //                    id = employeePossition.Id,
        //                    possitionName = employeePossition.Name,
        //                    possitionId = employeePossition.PossitionId
        //                }
        //            };
        //            try
        //            {
        //                employee.passwordHash = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().PasswordHash;
        //                employee.passwordSalt = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().PasswordSalt;
        //                employee.passwordaja = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().OldPassword;
        //            }
        //            catch
        //            {
        //                employee.passwordHash = null;
        //                employee.passwordSalt = null;
        //                employee.passwordaja = "";
        //            }
        //            masterEmployeeList.Add(employee);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return masterEmployeeList;
        //}

        private List<EmployeeMaster> masterEmployee(int storeID)
        {
            List<EmployeeMaster> masterEmployeeList = new List<EmployeeMaster>();
            try
            {
                string city = _context.Store.Where(x => x.Id == storeID).First().City;
                List<Store> dbStores = _context.Store.ToList();
                //    List<Store> dbStores = _context.Store.Where(x => x.City == city).ToList();
                for (int i = 0; i < dbStores.Count; i++)
                {
                    List<Employee> dbEmployee = _context.Employee.Where(c => c.StoreId == dbStores[i].Id && c.Status == true).ToList();
                    foreach (Employee b in dbEmployee)
                    {
                        EmployeePossition employeePossition = _context.EmployeePossition.Where(c => c.Id == b.PossitionId).First();
                        EmployeeMaster employee = new EmployeeMaster
                        {
                            employeeId = b.EmployeeCode,
                            storeId = dbStores[i].Id,
                            storeCode = dbStores[i].Code,
                            storeName = dbStores[i].Name,
                            id = b.Id,
                            name = b.EmployeeName,
                            possition = new Possition
                            {
                                id = employeePossition.Id,
                                possitionName = employeePossition.Name,
                                possitionId = employeePossition.PossitionId
                            }
                        };
                        if (b.StoreId == storeID)
                        {
                            try
                            {
                                employee.passwordHash = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().PasswordHash;
                                employee.passwordSalt = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().PasswordSalt;
                                employee.passwordaja = _context.UserLogin.Where(x => x.UserId == b.Id).FirstOrDefault().OldPassword;
                            }
                            catch
                            {
                                employee.passwordHash = null;
                                employee.passwordSalt = null;
                                employee.passwordaja = "";
                            }
                        }
                        else
                        {

                            employee.passwordHash = null;
                            employee.passwordSalt = null;
                            employee.passwordaja = "GABOLEHLOGINASDASDASDA";
                        }

                        masterEmployeeList.Add(employee);
                    }

                }

            }
            catch
            {

            }
            return masterEmployeeList;
        }
    }
}