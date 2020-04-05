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
        public async Task<IActionResult> getUploadSession(String StoreId)
        {
            try
            {
                object uploadSessionObj = new object();

                var uploadSession = (from us in _context.JobTabletoSynchDetailUpload.Where(x => x.StoreId == StoreId).OrderByDescending(x => x.SynchDetail).Take(100)
                                       select new
                                       {
                                           SyncDetail = us.SynchDetail,
                                           Store = us.StoreId,
                                           TableName = us.TableName,
                                           SyncDate = us.Synchdate,
                                           rowFatch = us.RowFatch,
                                           rowApplied = us.RowApplied,
                                           status = us.Status,
                                           JobId = us.JobId
                                       }).ToList();

                if (uploadSession.Count > 0)
                    uploadSessionObj = uploadSession;
                else
                    uploadSessionObj = "Data not foud";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = uploadSessionObj

                });
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
        public async Task<IActionResult> getUploadSessionStatus(String StoreId)
        {
            try
            {
                object uploadSessionStatusObj = new object();

                var uploadSessionStatus = (from us in _context.JobTabletoSynchDetailUpload.OrderByDescending(x => x.SynchDetail).Take(100)
                                             join ust in _context.JobSynchDetailUploadStatus
                                             on us.SynchDetail equals ust.SynchDetail
                                             where us.StoreId == StoreId 
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

                if (uploadSessionStatus.Count > 0)
                    uploadSessionStatusObj = uploadSessionStatus;
                else
                    uploadSessionStatusObj = "Data not found";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = uploadSessionStatusObj

                });
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