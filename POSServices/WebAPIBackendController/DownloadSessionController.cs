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
        public async Task<IActionResult> getDownloadSession(int JobId)
        {
            try
            {
                var downloadSession = (from ds in _context.JobTabletoSynchDetailDownload.Where(x => x.JobId == JobId)
                                       select new
                                       {
                                           SyncDetail = ds.SynchDetail,
                                           Store = ds.StoreId,
                                           TableName = ds.TableName,
                                           SyncDate = ds.Synchdate,
                                           rowFatch = ds.RowFatch,
                                           JobId = ds.JobId
                                       }).ToList();

                return Json(new[] { downloadSession });
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
        public async Task<IActionResult> getDownloadSessionStatus(int JobId)
        {
            try
            {
                var downloadSessionStatus = (from ds in _context.JobTabletoSynchDetailDownload
                                             join dst in _context.JobSynchDetailDownloadStatus
                                             on ds.SynchDetail equals dst.SynchDetail
                                             where ds.JobId == JobId
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

                return Json(new[] { downloadSessionStatus });
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