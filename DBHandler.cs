using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ConnectionLib
{
    public class DBHandler
    {
        public SqlConnection GetConnection()
        {
            string cs = ConfigurationManager.ConnectionStrings["ElectricityBillDB"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            return con;
        }
    }
}