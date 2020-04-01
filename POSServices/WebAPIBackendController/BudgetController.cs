using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Models;
using POSServices.WebAPIModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace POSServices.WebAPIBackendController
{
    [Route("api/Budget")]
    [ApiController]
    public class BudgetController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public BudgetController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getBudget()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var role = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
                var rols = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                if (rols == "Admin")
                {
                    return Json(new[] { _context.Budget.ToList() });
                }
                var budget =
                    (from login in _context.LoginStore.Where(item => item.LoginId == int.Parse(role))
                     join deliver in _context.Budget on login.StoreCode equals deliver.StoreCode
                     select new
                     {
                         Id = deliver.Id,
                         StoreId = deliver.StoreId,
                         StoreCode = deliver.StoreCode,
                         StoreName = deliver.StoreName,
                         Amount = deliver.Amount,
                         JournalNumber = deliver.JournalNumber,
                         TransactionDate = deliver.TransactionDate
                     }).ToList();

                return Json(budget);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "500",                    
                    message = ex.ToString()
                });
            }            
        }

        [HttpPost("Create")]
        public async Task<IActionResult> create(BudgetList budgetList)
        {
            try
            {
                List<Budget> list = budgetList.Budgets;

                for (int i = 0; i < list.Count; i++)
                {
                    Budget budget = new Budget();
                    budget.StoreCode = list[i].StoreCode;
                    budget.StoreId = list[i].StoreId;
                    budget.StoreName = list[i].StoreName;
                    budget.Amount = list[i].Amount;
                    budget.JournalNumber = list[i].JournalNumber;
                    budget.TransactionDate = list[i].TransactionDate;
                    _context.Add(budget);
                    _context.SaveChanges();
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
        public async Task<IActionResult> update(BudgetList budgetList)
        {
            try
            {
                List<Budget> list = budgetList.Budgets;

                for (int i = 0; i < list.Count; i++)
                {
                    bool discExist = false;
                    discExist = _context.Budget.Any(c => c.StoreCode == list[i].StoreCode);
                    if (discExist == true)
                    {
                        var budget = _context.Budget.Where(x => x.StoreCode == list[i].StoreCode).First();
                        budget.Amount = list[i].Amount;
                        budget.TransactionDate = list[i].TransactionDate;
                        budget.JournalNumber = list[i].JournalNumber;
                        _context.Budget.Update(budget);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return StatusCode(404, new
                        {
                            status = "404",
                            update = false,
                            message = "Discount code not found."
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
        public async Task<IActionResult> delete(BudgetList budgetList)
        {
            try
            {
                List<Budget> list = budgetList.Budgets;

                for (int i = 0; i < list.Count; i++)
                {
                    var budget = _context.Budget.Where(x => x.StoreCode == list[i].StoreCode).First();
                    _context.Budget.Remove(budget);
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
    }
}