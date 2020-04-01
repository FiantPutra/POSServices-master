using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSServices.PosMsgModels;

namespace POSServices.WebAPIPOSMsgController
{
    [Route("homsg/download")]
    [ApiController]
    public class DownloadSyncDetailController : Controller
    {
        [HttpGet]
        public List<syncDownloadDetail> syncDetail(string storeCode)
        {
            List<syncDownloadDetail> syncDownloadDet = new List<syncDownloadDetail>();
            try
            {
                string ConnectionString = getConnection();

                SqlConnection con = new SqlConnection(ConnectionString);

                SqlConnection connection = con;
                try
                {

                    string queryString = "SELECT * FROM JobTabletoSynchDetailDownload WHERE (StoreID = '" + storeCode + "' OR StoreID = '') " +
                                            "AND SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailDownloadStatus WHERE Status = 0)";
                    //string queryString = "SELECT * FROM JobTabletoSynchDetailDownload WHERE (StoreID = 'DAD' OR StoreID = '') " +
                    //                        "AND SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailDownloadStatus WHERE Status = 0)";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Guid downloadSession = Guid.NewGuid();

                            while (reader.Read())
                            {
                                syncDownloadDetail temp = new syncDownloadDetail();
                                temp.IdDetail = Convert.ToString(reader["SynchDetail"]);
                                temp.JobId = Convert.ToString(reader["JobID"]);
                                temp.StoreId = Convert.ToString(reader["StoreID"]);
                                temp.TableName = Convert.ToString(reader["TableName"]);
                                temp.DownloadPath = Convert.ToString(reader["DownloadPath"]);
                                temp.SyncDate = Convert.ToString(reader["Synchdate"]);
                                temp.createTable = Convert.ToString(reader["CreateTable"]);
                                temp.RowFatch = Convert.ToString(reader["RowFatch"]);
                                temp.syncType = Convert.ToInt32(reader["SyncType"]);
                                temp.tablePrimaryKey = Convert.ToString(reader["TablePrimaryKey"]);

                                syncDownloadDet.Add(temp);
                            }
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

                return syncDownloadDet;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string getConnection()
        {
            return Startup.POSMsgConnString;
        }
    }
}