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
    [Route("api/UploadSession")]
    [ApiController]
    public class UploadSessionController : Controller
    {
        private readonly HO_MsgContext _context;

        public UploadSessionController(HO_MsgContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getUploadSession(int JobId)
        {
            try
            {
                var uploadSession = (from us in _context.JobTabletoSynchDetailUpload.Where(x => x.JobId == JobId)
                                       select new
                                       {
                                           SyncDetail = us.SynchDetail,
                                           Store = us.StoreId,
                                           TableName = us.TableName,
                                           SyncDate = us.Synchdate,
                                           rowFatch = us.RowFatch,
                                           JobId = us.JobId
                                       }).ToList();

                return Json(new[] { uploadSession });
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

        [HttpGet("Status")]
        public async Task<IActionResult> getUploadSessionStatus(int JobId)
        {
            try
            {
                var uploadSessionStatus = (from us in _context.JobTabletoSynchDetailUpload
                                             join ust in _context.JobSynchDetailUploadStatus
                                             on us.SynchDetail equals ust.SynchDetail
                                             where us.JobId == JobId
                                             select new
                                             {
                                                 SyncDetail = us.SynchDetail,
                                                 Store = us.StoreId,
                                                 TableName = us.TableName,
                                                 SyncDate = us.Synchdate,
                                                 RowFatch = us.RowFatch,
                                                 RowApplied = ust.RowApplied,
                                                 Status = ust.Status
                                             }).ToList();

                return Json(new[] { uploadSessionStatus });
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