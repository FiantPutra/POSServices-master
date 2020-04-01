using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSServices.PosMsgModels;
using POSServices.WebAPIModel;

namespace POSServices.WebAPIPOSMsgController
{
    [Route("homsg/Upload")]
    [ApiController]
    public class UploadSyncDetailController : Controller
    {
        [HttpPost]
        public IActionResult insertUploadDetail([FromBody] bracketSyncUploadDetail uploadDetail)
        {
            String uploadFilePath = "";            
            
            try
            {
                string ConnectionString = getConnection();

                SqlConnection con = new SqlConnection(ConnectionString);

                SqlConnection connection = con;
                try
                {
                    String cmd_insert = "IF NOT EXISTS (SELECT * FROM JobTabletoSynchDetailUpload WHERE SynchDetail = @SynchDetail) " +
                                        "BEGIN " +
                                        "INSERT INTO JobTabletoSynchDetailUpload(SynchDetail, JobID, StoreID, TableName, UploadPath, Synchdate, CreateTable, RowFatch, MinId, MaxId, TablePrimaryKey, identityColumn) " +
                                        "VALUES(@SynchDetail, @JobID, @StoreID, @TableName, @UploadPath, @Synchdate, @CreateTable, @RowFatch, @MinId, @MaxId, @TablePrimaryKey,@IdentityColumn) " +
                                        "END";

                    using (SqlCommand command = new SqlCommand(cmd_insert, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add("@SynchDetail", SqlDbType.BigInt);
                        command.Parameters.Add("@JobID", SqlDbType.Int);
                        command.Parameters.Add("@StoreID", SqlDbType.VarChar);
                        command.Parameters.Add("@TableName", SqlDbType.VarChar);
                        command.Parameters.Add("@UploadPath", SqlDbType.VarChar);
                        command.Parameters.Add("@Synchdate", SqlDbType.DateTime);
                        command.Parameters.Add("@CreateTable", SqlDbType.VarChar);
                        command.Parameters.Add("@RowFatch", SqlDbType.Int);
                        command.Parameters.Add("@MinId", SqlDbType.Int);
                        command.Parameters.Add("@MaxId", SqlDbType.Int);
                        command.Parameters.Add("@TablePrimaryKey", SqlDbType.VarChar);
                        command.Parameters.Add("@IdentityColumn", SqlDbType.VarChar);

                        List<syncUploadDetail> detail = uploadDetail.uploadDetails;
                        for (int i = 0; i < detail.Count; i++)
                        {                            
                            command.Parameters[0].Value = Convert.ToInt64(detail[i].syncDetailsId);
                            command.Parameters[1].Value = Convert.ToInt32(detail[i].JobId);
                            command.Parameters[2].Value = Convert.ToString(detail[i].StoreId);
                            command.Parameters[3].Value = Convert.ToString(detail[i].TableName);
                            command.Parameters[4].Value = Convert.ToString(detail[i].UploadPath);
                            command.Parameters[5].Value = Convert.ToDateTime(detail[i].Synchdate);
                            command.Parameters[6].Value = Convert.ToString(detail[i].CreateTable);
                            command.Parameters[7].Value = Convert.ToInt32(detail[i].RowFatch);
                            command.Parameters[8].Value = Convert.ToInt32(detail[i].MinId);
                            command.Parameters[9].Value = Convert.ToInt32(detail[i].MaxId);
                            command.Parameters[10].Value = Convert.ToString(detail[i].TablePrimaryKey);
                            command.Parameters[11].Value = Convert.ToString(detail[i].identityColumn);
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
                response.message = e.ToString();

                return Ok(response);
            }
        }

        public static string getConnection()
        {
            return Startup.POSMsgConnString;
        }        
    }
}