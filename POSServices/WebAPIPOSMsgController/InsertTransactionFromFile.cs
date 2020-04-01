using Microsoft.AspNetCore.Mvc;
using POSServices.WebAPIModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIPOSMsgController
{
    public class InsertTransactionFromFile
    {
        String uploadPath = "";
        String fileName = "";
        String id = "";
        String jobId = "";
        String storeId = "";        

        public APIResponse insertTransaction()
        {
            String uploadFilePath = "";
            String fileToExtract = "";

            try
            {
                string ConnectionString = getConnection();
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlConnection connection = con;

                String cmd = "SELECT JobID, StoreID, UploadPath, SynchDetail FROM JobTabletoSynchDetailUpload " +
                                "WHERE SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailUploadStatus)";
                
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand(cmd, connection);
                SqlDataReader sqlDataRd = command.ExecuteReader();

                if (sqlDataRd.HasRows)
                {
                    while (sqlDataRd.Read())
                    {
                        uploadPath = Convert.ToString(sqlDataRd["UploadPath"]);
                        id = Convert.ToString(sqlDataRd["SynchDetail"]);
                        jobId = Convert.ToString(sqlDataRd["JobID"]);
                        storeId = Convert.ToString(sqlDataRd["StoreID"]);

                        fileName = getFilename(uploadPath);
                        uploadFilePath = getFilePath(jobId, storeId);
                        if (!File.Exists(uploadFilePath + @"\" + fileName + ".bcp"))
                        {
                            fileToExtract = uploadFilePath + @"\" + fileName + ".zip";
                            ExtractFile(fileToExtract, uploadFilePath + @"\");
                        }
                    }
                }                

                APIResponse response = new APIResponse();
                response.code = "200";
                response.message = "OK";

                return response;
            }
            catch
            {
                APIResponse response = new APIResponse();
                response.code = "404";
                response.message = "Bad Request";

                return response;
            }
        }

        public static string getConnection()
        {
            return Startup.POSMsgConnString;
        }

        public String getFilePath(String jobId, String storeId)
        {
            String filePath = "";
            String ftpFolder = "";

            ftpFolder = getFtpFolder();

            if (storeId == "")
            {
                filePath = ftpFolder + @"Uploadfile\" + jobId;                
            }
            else
            {
                filePath = ftpFolder + @"Uploadfile\" + jobId + @"\" + storeId;                
            }

            return filePath;
        }

        public String getFtpFolder()
        {
            String ftpFolder = "";

            try
            {
                string ConnectionString = getConnection();
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlConnection connection = con;

                String cmd = "SELECT * FROM IntegrationParameter";

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand(cmd, connection);
                SqlDataReader sqlDataRd = command.ExecuteReader();

                if (sqlDataRd.HasRows)
                {
                    while (sqlDataRd.Read())
                    {
                        ftpFolder = Convert.ToString(sqlDataRd["ftpfolder"]);
                    }
                }

                return ftpFolder;
            }
            catch (Exception)
            {
                return ftpFolder;
            }            
        }

        public String getFilename(String uploadPath)
        {
            int charCount = 0;
            int charMatch = 0;
            int count = 0;
            int j = 0;
            string rText = "", pattern = "\\", fileName = "";

            while ((j = uploadPath.IndexOf(pattern, j)) != -1)
            {
                j += pattern.Length;
                count++;
            }
            charCount = count;

            for (int i = 0; i < uploadPath.Length; i++)
            {
                if (uploadPath[i].ToString() == @"\")
                {
                    charMatch++;
                }

                if (charMatch == charCount)
                {
                    rText += uploadPath[i].ToString();
                }
            }

            fileName = getBetween(rText, @"\", ".zip");

            return fileName;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public void ExtractFile(String source, String destination)
        {
            string zPath = @"C:\Program Files\7-Zip\7zG.exe";
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = "x \"" + source + "\" -o" + destination;
                Process x = Process.Start(pro);
            }
            catch (System.Exception Ex) { }
        }
    }
}
