using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DB_BIENSI_POSContext _context;
        public AuthRepository(DB_BIENSI_POSContext context)
        {
            _context = context;
        }
        public async Task<UserLogin> Login(string username, string password)
        {
            var employeecode = await _context.Employee.Where(c => c.EmployeeCode == username).Select(c => c.Id).FirstOrDefaultAsync();
            // var login = await _context.UserLogin.FirstOrDefaultAsync(x => x.UserId == Int32.Parse(username));
            var login = await _context.UserLogin.FirstOrDefaultAsync(x => x.UserId == employeecode);
            if (login == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, login.PasswordHash, login.PasswordSalt))
            {
                return null;
            }

            login.LastLogin = DateTime.UtcNow;
            _context.Update(login);
            await _context.SaveChangesAsync();
            //auth sukses
            return login;
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

        public async Task<UserLogin> Register(UserLogin login, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            login.PasswordHash = passwordHash;
            login.PasswordSalt = passwordSalt;

            await _context.UserLogin.AddAsync(login);
            await _context.SaveChangesAsync();

            return login;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.UserLogin.AnyAsync(x => x.UserId == Int32.Parse(username)))
                return true;

            return false;
        }
    }
}
