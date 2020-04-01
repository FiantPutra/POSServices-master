using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSServices.Data;
using POSServices.PosMsgModels;

namespace POSServices.WebAPIBackendController
{
    [Route("api/TableSync")]
    [ApiController]
    public class TableSyncController : Controller
    {
        private readonly HO_MsgContext _context;

        public TableSyncController(HO_MsgContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getTableSync()
        {
            try
            {
                var jobList = (from TableToSynch in _context.TableToSynch
                               select new
                               {
                                   TableName = TableToSynch.TableName                                   
                               }).ToList();

                return Json(jobList);
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
    }
}