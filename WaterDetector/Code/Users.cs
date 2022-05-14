using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterDetector.Code
{
    
    public class Users
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["WDetector"].ConnectionString);
        public List<UsersDetails> Login()
        {
            List<UsersDetails> usersDetails = new List<UsersDetails>();
            using (conn)
            {
                conn.Open();
                string query = "select * from dbo.USERS";

                SqlCommand sqlCommand = new SqlCommand(query, conn);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlCommand.CommandTimeout = 0;
                dataAdapter.SelectCommand = sqlCommand;
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    UsersDetails item = new UsersDetails();
                    item.USERNAME = row["USERNAME"].ToString();
                    item.PASSWORD = row["PASSWORD"].ToString();
                    usersDetails.Add(item);
                }
               
            }
            return usersDetails;
        }
    }
    public class UsersDetails
    {
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}