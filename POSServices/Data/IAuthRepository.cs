using POSServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.Data
{
    public interface IAuthRepository
    {
        Task<UserLogin> Register(UserLogin login, string password);
        Task<UserLogin> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}
