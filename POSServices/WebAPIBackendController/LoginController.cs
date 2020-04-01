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
    [Route("api/Login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        private readonly IAuthRepository _repo;

        public LoginController(DB_BIENSI_POSContext context, IAuthRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> userLogin(string username, string password)
        {
            try
            {
                var userFromRepo = await _repo.Login(username, password);
                if (userFromRepo == null)
                {
                    return StatusCode(404, new
                    {
                        status = "404",
                        login = false,
                        message = "Username or password is incorrect."
                    });
                }

                return StatusCode(200, new
                {
                    status = "200",
                    login = true,
                    message = "Login success!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",
                    login = false,
                    message = ex.ToString()
                });
            }                        
        }
    }
}