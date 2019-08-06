using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace OfficeOnline.DAL
{
    public class DapperHelper
    {
        internal static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static IEnumerable<T> Query<T>(string sql, string userLogin, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                if (param == null)
                {
                    param = new DynamicParameters();
                }
                param.Add("@ActionUser", userLogin, DbType.String);
                return conn.Query<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static int Execute(string sql, string userLogin, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                if (param == null)
                {
                    param = new DynamicParameters();
                }

                param.Add("@ActionUser", userLogin, DbType.String);
                return conn.Execute(sql, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
