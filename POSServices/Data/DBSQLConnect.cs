using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using POSServices.WebAPIModel;

namespace POSServices.Data
{
    public class DBSQLConnect
    {
        private static string conString = "";
        public string config = "";
        public string msgSqlCon = "";
        public string conSQL = "";

        public SqlDataReader sqlDataRd = null;
        public SqlDataReader sqlDataRdHeader = null;
        public SqlDataReader sqlDataRdLine = null;

        public SqlConnection sqlCon()
        {
            String connectionString = "";
            //connectionString = "Data Source=10.5.50.41;Initial Catalog=DB_BIENSI_POS;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=p@$$w0rd";
            connectionString = getConnection();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            return sqlCon;
        }

        public SqlDataReader ExecuteDataReader(String query, SqlConnection sqlCon)
        {
            if (sqlCon == null || sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            SqlCommand sqlCmd = null;
            SqlDataReader sqlDataRd = null;

            sqlCmd = new SqlCommand(query);
            sqlCmd.Connection = sqlCon;
            sqlDataRd = sqlCmd.ExecuteReader();

            return sqlDataRd;
        }

        public SqlDataReader ExecuteDataReaderWithParams(String query, SqlConnection sqlCon, List<ObjectAPIModel> listParams)
        {
            if (sqlCon == null || sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            SqlCommand sqlCmd = null;
            SqlDataReader sqlDataRd = null;
            SqlParameter sqlParam = null;

            sqlCmd = new SqlCommand(query);
            sqlCmd.Connection = sqlCon;

            if (listParams.Count > 0)
            {
                foreach (var param in listParams)
                {
                    sqlParam = new SqlParameter(param.param, param.value);
                    sqlParam.Direction = ParameterDirection.Input;
                    sqlParam.DbType = param.typeValue;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }

            sqlDataRd = sqlCmd.ExecuteReader();

            return sqlDataRd;
        }

        public static string getConnection()
        {
            return Startup.BackendConnString;
        }
    }
}
