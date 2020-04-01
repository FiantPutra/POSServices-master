using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSServices.PosMsgModels;
using POSServices.WebAPIModel;

namespace POSServices.WebAPIPOSMsgController
{
    [Route("homsg/updateSyncDownload")]
    [ApiController]
    public class UpdateSyncDetailDownloadController : Controller
    {
        [HttpPost]
        public IActionResult update([FromBody] bracketSyncDetail updateSyncs)
        {
            try
            {
                string ConnectionString = getConnection();

                SqlConnection con = new SqlConnection(ConnectionString);

                SqlConnection connection = con;
                try
                {
                    string queryString = "BEGIN TRAN " +
                        "IF NOT EXISTS (SELECT * FROM JobSynchDetailDownloadStatus WHERE SynchDetail = @SynchDetail) " +
                        "BEGIN " +
                        "INSERT INTO JobSynchDetailDownloadStatus(SynchDetail, RowFatch, RowApplied, Status, Downloadsessionid) " +
                        "VALUES(@SynchDetail, @RowFatch, @RowApplied, @Status, @downloadsessionid) " +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                        "UPDATE JobSynchDetailDownloadStatus SET RowFatch = @RowFatch, RowApplied = @RowApplied, Downloadsessionid = @downloadsessionid " +
                        "WHERE SynchDetail = @SynchDetail " +
                        "END " +
                        "COMMIT TRAN";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add("@SynchDetail", SqlDbType.BigInt);
                        command.Parameters.Add("@RowFatch", SqlDbType.Int);
                        command.Parameters.Add("@RowApplied", SqlDbType.Int);
                        command.Parameters.Add("@Status", SqlDbType.Int);
                        command.Parameters.Add("@downloadsessionid", SqlDbType.UniqueIdentifier);

                        List<updateSyncDetailDownload> detail = updateSyncs.syncDetailDownload;
                        for (int i = 0; i < detail.Count; i++)
                        {
                            command.Parameters[0].Value = Convert.ToInt64(detail[i].syncDetailsId);
                            command.Parameters[1].Value = Convert.ToInt32(detail[i].rowFatch);
                            command.Parameters[2].Value = Convert.ToInt32(detail[i].rowApplied);
                            command.Parameters[3].Value = Convert.ToInt32(detail[i].status);
                            command.Parameters[4].Value = Guid.Parse(detail[i].downloadSessionId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                APIResponse response = new APIResponse();
                response.code = "200";
                response.message = "OK";

                return Ok(response);
            }
            catch (Exception e)
            {
                APIResponse response = new APIResponse();
                response.code = "404";
                response.message = "Bad Request";

                return Ok(response);
            }
        }

        public static string getConnection()
        {
            return Startup.POSMsgConnString;
        }
    }
}