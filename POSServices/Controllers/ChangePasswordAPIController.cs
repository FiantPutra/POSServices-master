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
    [Route("api/ChangePasswordAPI")]
    [ApiController]
    public class ChangePasswordAPIController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ChangePasswordAPIController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<IActionResult> changepass([FromBody]ChangePasswordModel change)   
        {
            var employeecode = await _context.Employee.Where(c => c.EmployeeCode == change.username).Select(c => c.Id).FirstOrDefaultAsync();
            var login = await _context.UserLogin.FirstOrDefaultAsync(x => x.UserId == employeecode);

            if (login == null)
            {
                return StatusCode(404, new
                {
                    status = "404",
                    error = true,
                    message = "Username Not found"
                });
            }

            if (!VerifyPasswordHash(change.currentpassword, login.PasswordHash, login.PasswordSalt))
            {
                return StatusCode(404, new
                {
                    status = "404",
                    error = true,
                    message = "Incorrect Current Password"
                });
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(change.newpassword, out passwordHash, out passwordSalt);

            login.PasswordHash = passwordHash;
            login.PasswordSalt = passwordSalt;
            login.OldPassword = change.newpassword;

            _context.UserLogin.Update(login);
            _context.SaveChanges();

            return StatusCode(200, new
            {
                status = "200",
                error = false,
                message = "Change password success!"
            });
        }



        private bool VerifyPasswordHash(string currentpassword, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(currentpassword));
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
    }
}