using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.Models;

namespace POSServices.WebAPIBackendController
{
    [Route("api/UserSetup")]
    [ApiController]
    public class UserSetupController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        private readonly IAuthRepository _repo;

        public UserSetupController(DB_BIENSI_POSContext context, IAuthRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> create(userLoginList userLoginList)
        {
            try
            {
                List<UserLogin> list = userLoginList.UserList;

                for (int i = 0; i < list.Count; i++)
                {
                    bool dataexist = _context.UserLogin.Any(c => c.UserId == list[i].UserId);
                    if (!dataexist)
                    {
                        UserLogin login = new UserLogin();                        
                        string password = list[i].OldPassword;
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(password, out passwordHash, out passwordSalt);                        
                        login.PasswordHash = passwordHash;
                        login.PasswordSalt = passwordSalt;
                        //login.ConfirmPassword = list[i].ConfirmPassword;
                        //login.NewPassword = password;
                        login.OldPassword = password;
                        login.Role = list[i].Role;                        
                        _context.Add(login);
                        _context.SaveChanges();

                        CreateStoreifexist(login.UserId, login.Id);                         
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            create = false,
                            message = "Cannot create a record, user id already exist."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    create = true,
                    message = "Created successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    create = false,
                    message = ex.ToString()
                });
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> update(userLoginList userLoginList)
        {
            try
            {
                List<UserLogin> list = userLoginList.UserList;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    discExist = _context.UserLogin.Any(c => c.UserId == list[i].UserId);
                    if (discExist == true)
                    {
                        var login = _context.UserLogin.Where(x => x.UserId == list[i].UserId).First();                        
                        string password = list[i].OldPassword;
                        if (password != "")
                        {
                            byte[] passwordHash, passwordSalt;
                            CreatePasswordHash(password, out passwordHash, out passwordSalt);
                            login.PasswordHash = passwordHash;
                            login.PasswordSalt = passwordSalt;
                        }
                        else
                        {
                            login.PasswordHash = login.PasswordHash;
                            login.PasswordSalt = login.PasswordSalt;
                        }
                        login.Role = list[i].Role;
                        login.Status = login.Status;
                        login.LastLogin = login.LastLogin;
                        login.OldPassword = password;
                        _context.UserLogin.Update(login);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "User id not found."
                        });
                    }
                }

                return StatusCode(200, new
                {
                    status = "200",
                    update = true,
                    message = "updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    update = false,
                    message = ex.ToString()
                });
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> delete(userLoginList userLoginList)
        {
            try
            {
                List<UserLogin> list = userLoginList.UserList;

                for (int i = 0; i < list.Count; i++)
                {
                    var login = _context.UserLogin.Where(x => x.UserId == list[i].UserId).First();
                    _context.UserLogin.Remove(login);
                    _context.SaveChanges();
                }

                return StatusCode(200, new
                {
                    status = "200",
                    delete = true,
                    message = "Deleted successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    delete = false,
                    message = ex.ToString()
                });
            }
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void CreateStoreifexist(int userId, int loginid)
        {
            try
            {
                var storeid = _context.Employee.Where(x => x.Id == userId).FirstOrDefault().StoreId;
                if (storeid != 0)
                {
                    LoginStore loginStore = new LoginStore();
                    loginStore.StoreId = storeid;
                    loginStore.LoginId = loginid;
                    loginStore.StoreCode = _context.Store.Where(c => c.Id == storeid).First().Code;
                    _context.Add(loginStore);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                StatusCode(500, new
                {
                    status = "500",
                    message = ex.ToString()
                });
            }            
        }
    }
}