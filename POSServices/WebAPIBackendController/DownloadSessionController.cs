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
    [Route("api/DownloadSession")]
    [ApiController]
    public class DownloadSessionController : Controller
    {
        private readonly HO_MsgContext _context;

        public DownloadSessionController(HO_MsgContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getDownloadSession(String StoreId)
        {
            try
            {
                object downloadSessionObj = new object();

                var downloadSession = (from ds in _context.JobTabletoSynchDetailDownload.Where(x => x.StoreId == StoreId).OrderByDescending(x => x.SynchDetail).Take(100)
                                       select new
                                       {
                                           SyncDetail = ds.SynchDetail,
                                           Store = ds.StoreId,
                                           TableName = ds.TableName,
                                           SyncDate = ds.Synchdate,
                                           rowFatch = ds.RowFatch,
                                           JobId = ds.JobId
                                       }).ToList();

                if (downloadSession.Count > 0)
                    downloadSessionObj = downloadSession;
                else
                    downloadSessionObj = "Data not found";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = downloadSessionObj
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
        public async Task<IActionResult> getDownloadSessionStatus(String StoreId)
        {
            try
            {
                object downloadSessionStatusObj = new object();

                var downloadSessionStatus = (from ds in _context.JobTabletoSynchDetailDownload.OrderByDescending(x => x.SynchDetail).Take(100)
                                             join dst in _context.JobSynchDetailDownloadStatus
                                             on ds.SynchDetail equals dst.SynchDetail
                                             where ds.StoreId == StoreId
                                             select new
                                             {
                                                 SyncDetail = ds.SynchDetail,
                                                 Store = ds.StoreId,
                                                 TableName = ds.TableName,
                                                 SyncDate = ds.Synchdate,
                                                 RowFatch = ds.RowFatch,
                                                 RowApplied = dst.RowApplied,
                                                 Status = dst.Status
                                             }).ToList();

                if (downloadSessionStatus.Count > 0)
                    downloadSessionStatusObj = downloadSessionStatus;
                else
                    downloadSessionStatusObj = "Data not found";

                return StatusCode(1, new
                {
                    status = "1",
                    message = "Success",
                    data = downloadSessionStatusObj
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