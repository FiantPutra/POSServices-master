using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Config;
using POSServices.Models;
using POSServices.WebAPIModel;
using POSServices.WebApiSunfishModel;

namespace POSServices.WebApiSunfishController
{    
    public class WebApiSunfishController
    {
        private readonly DB_BIENSI_POSContext _context;
        public WebApiSunfishController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        public void getEmployeeMaster()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-SFAPI-UserName", sunfishConfig.username);
                client.DefaultRequestHeaders.Add("X-SFAPI-AppName", sunfishConfig.appname);
                client.DefaultRequestHeaders.Add("X-SFAPI-RSAKey", sunfishConfig.RSAkey);
                client.DefaultRequestHeaders.Add("X-SFAPI-Account", sunfishConfig.account);
                // Make your request...

                try
                {
                    HttpResponseMessage message = client.GetAsync(sunfishConfig.Uri + "index.cfm?endpoint=/biensi_SFFULL_EO_GetAllEmpBiensi").Result;
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(SunfishEmployee));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        SunfishEmployee resultData = serializer.ReadObject(stream) as SunfishEmployee;
                        try
                        {
                            LogRecord logRecord = new LogRecord();
                            logRecord.Tag = "Batch - Employee - Data";
                            logRecord.TimeStamp = DateTime.Now;
                            logRecord.Message = JsonConvert.SerializeObject(resultData);
                            _context.LogRecord.Add(logRecord);
                            _context.SaveChanges();

                        }
                        catch { }

                        for (int i = 0; i < resultData.RESULT.Count; i++)
                        {
                            try
                            {
                                //check if employee not exist then add new employee
                                bool isEmployeeExist = _context.Employee.Any(c => c.EmployeeCode == resultData.RESULT[i].NIK);
                                if (isEmployeeExist == false)
                                {
                                    try
                                    {
                                        Employee employee = new Employee();
                                        employee.Status = resultData.RESULT[i].STATUS_ACTIVE.Equals(1) ? true : false;
                                        employee.EmployeeName = resultData.RESULT[i].NAME;
                                        employee.EmployeeCode = resultData.RESULT[i].NIK;
                                        employee.LastUpdateDate = DateTime.Now;
                                        try
                                        {
                                            employee.PossitionId = _context.EmployeePossition.Where(c => c.PossitionId == resultData.RESULT[i].POS_CODE).First().Id;

                                        }
                                        catch
                                        {

                                            employee.PossitionId = _context.EmployeePossition.Where(c => c.PossitionId == "OFC123").First().Id;

                                        }
                                        // employee.PossitionId = _context.EmployeePossition.Where(c => c.PossitionId == resultData.RESULT[i].POS_CODE).First().Id;
                                        try
                                        {
                                            employee.StoreId = _context.Store.Where(c => c.Code == resultData.RESULT[i].STORE_CODE).First().Id;
                                        }
                                        catch
                                        {
                                            employee.StoreId = _context.Store.Where(c => c.Code == "000").First().Id;
                                        }

                                        _context.Employee.Add(employee);
                                    }
                                    catch
                                    {

                                    }                                  
                                }
                            }
                            catch
                            {

                            }
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        String data = message.Headers.ToString();

                    }
                }
                catch (Exception ex)
                {

                }
            }

            userlogincreation();
        }

        public void getEmployeeStore()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-SFAPI-UserName", sunfishConfig.username);
                client.DefaultRequestHeaders.Add("X-SFAPI-AppName", sunfishConfig.appname);
                client.DefaultRequestHeaders.Add("X-SFAPI-RSAKey", sunfishConfig.RSAkey);
                client.DefaultRequestHeaders.Add("X-SFAPI-Account", sunfishConfig.account);
                // Make your request...

                try
                {
                    HttpResponseMessage message = client.GetAsync(sunfishConfig.Uri + "index.cfm?endpoint=/biensi_SFFULL_EO_getallempstorefirst").Result;
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(SunfishEmployee));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        SunfishEmployee resultData = serializer.ReadObject(stream) as SunfishEmployee;

                        try
                        {
                            LogRecord logRecord = new LogRecord();
                            logRecord.Tag = "Batch - Employee - Data";
                            logRecord.TimeStamp = DateTime.Now;
                            logRecord.Message = JsonConvert.SerializeObject(resultData);
                            _context.LogRecord.Add(logRecord);
                            _context.SaveChanges();

                        }
                        catch { }
                        for (int i = 0; i < resultData.RESULT.Count; i++)
                        {
                            try
                            {
                                if (_context.EmployeePossition.Any(c => c.PossitionId == resultData.RESULT[i].POS_CODE) == true)
                                {
                                    //check if employee not exist then add new employee
                                    bool isEmployeeExist = _context.Employee.Any(c => c.EmployeeCode == resultData.RESULT[i].NIK);
                                    if (isEmployeeExist == false)
                                    {
                                        try
                                        {
                                            Employee employee = new Employee();
                                            employee.Status = resultData.RESULT[i].STATUS_ACTIVE.Equals(1) ? true : false;
                                            employee.EmployeeName = resultData.RESULT[i].NAME;
                                            employee.EmployeeCode = resultData.RESULT[i].NIK;
                                            employee.LastUpdateDate = DateTime.Now;
                                            employee.PossitionId = _context.EmployeePossition.Where(c => c.PossitionId == resultData.RESULT[i].POS_CODE).First().Id;
                                            employee.StoreId = _context.Store.Where(c => c.Code == resultData.RESULT[i].STORE_CODE).First().Id;
                                            _context.Employee.Add(employee);
                                        }
                                        catch
                                        {

                                        }                                        
                                    }                                                                        
                                }
                            }
                            catch
                            {

                            }
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        String data = message.Headers.ToString();

                    }
                }
                catch (Exception ex)
                {

                }
            }

            userlogincreation();
        }

        public void getEmployee()
        {
            getEmployeeStore();
            getEmployeeMaster();

        }
        private void userlogincreation()
        {
            string ConnectionString = Config.General.sqlRoute;
            string insertSql1 = "INSERT INTO USERLOGIN(UserId,PasswordHash,PasswordSalt,Status,Role,NewPassword,OldPassword,ConfirmPassword) VALUES (@UserId,@PasswordHash,@PasswordSalt,@Status,@Role,@NewPassword,@OldPassword,@ConfirmPassword) SELECT SCOPE_IDENTITY();";
            string insertSql2 = "INSERT INTO LOGINSTORE(StoreId,LoginId,StoreCode) VALUES (@StoreId,@LoginId,@StoreCode)";            
            string password = "123456";
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var passhash = passwordHash;
            var passsalt = passwordSalt;


            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                myConnection.Open();                

                List<Employee> emplist = _context.Employee.Where(x => x.Status == true).ToList();
                for (int i = 0; i < emplist.Count; i++)
                {
                    bool dataexist = _context.UserLogin.Any(x => x.UserId == emplist[i].Id);
                    if (dataexist == false)
                    {
                        SqlCommand command1 = new SqlCommand(insertSql1, myConnection);

                        command1.Parameters.AddWithValue("@UserId", emplist[i].Id);
                        command1.Parameters.AddWithValue("@PasswordHash", passhash);
                        command1.Parameters.AddWithValue("@PasswordSalt", passsalt);
                        command1.Parameters.AddWithValue("@Status", 1);
                        command1.Parameters.AddWithValue("@Role", "non");
                        command1.Parameters.AddWithValue("@NewPassword", "");
                        command1.Parameters.AddWithValue("@ConfirmPassword", "");
                        command1.Parameters.AddWithValue("@OldPassword", "123456");
                        int primaryKey = Convert.ToInt32(command1.ExecuteScalar());

                        SqlCommand command2 = new SqlCommand(insertSql2, myConnection);
                        command2.Parameters.AddWithValue("@StoreId", emplist[i].StoreId);
                        command2.Parameters.AddWithValue("@LoginId", primaryKey);
                        command2.Parameters.AddWithValue("@StoreCode", _context.Store.Where(c => c.Id == emplist[i].StoreId).First().Code);

                        command2.ExecuteNonQuery();
                        command1.Parameters.Clear();
                        command2.Parameters.Clear();

                    }                    
                }
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task getPosition()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-SFAPI-UserName", sunfishConfig.username);
                client.DefaultRequestHeaders.Add("X-SFAPI-AppName", sunfishConfig.appname);
                client.DefaultRequestHeaders.Add("X-SFAPI-RSAKey", sunfishConfig.RSAkey);
                client.DefaultRequestHeaders.Add("X-SFAPI-Account", sunfishConfig.account);
                // Make your request...

                try
                {
                    HttpResponseMessage message = client.GetAsync(sunfishConfig.Uri + "index.cfm?endpoint=/biensi_SFFULL_EO_getmasterstoreposition").Result;
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(SunfishPosition));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        SunfishPosition resultData = serializer.ReadObject(stream) as SunfishPosition;
                        for (int i = 0; i < resultData.RESULT.Count; i++)
                        {
                            try
                            {
                                bool isPossitionex = _context.EmployeePossition.Any(c => c.PossitionId == resultData.RESULT[i].POSITION_CODE);
                                if (isPossitionex == false)
                                {
                                    EmployeePossition employee = new EmployeePossition();
                                    employee.PossitionId = resultData.RESULT[i].POSITION_CODE;
                                    employee.Name = resultData.RESULT[i].POSITION_NAME;
                                    _context.EmployeePossition.Add(employee);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    EmployeePossition employee = _context.EmployeePossition.Where(x => x.PossitionId == resultData.RESULT[i].POSITION_CODE).First();
                                    employee.PossitionId = resultData.RESULT[i].POSITION_CODE;
                                    employee.Name = resultData.RESULT[i].POSITION_NAME;
                                    _context.EmployeePossition.Update(employee);
                                    _context.SaveChanges();
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        String data = message.Headers.ToString();

                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}