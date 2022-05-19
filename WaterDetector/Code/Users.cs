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
        public int Login(UsersDetails users)
        {
            int result = 0;
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", users.USERNAME);
                cmd.Parameters.AddWithValue("@Password", users.PASSWORD);
                cmd.Parameters.Add("@Result", SqlDbType.Int, 100).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@Result"].Value);
            }
            return result;
        }
        public bool Register(UsersDetails users)
        {
            bool result = false;
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spRegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", users.USERNAME);
                cmd.Parameters.AddWithValue("@Password", users.PASSWORD);
                cmd.Parameters.AddWithValue("@Email", users.EMAIL);
                cmd.Parameters.AddWithValue("@FirstName", users.FIRSTNAME);
                cmd.Parameters.AddWithValue("@LastName", users.LASTNAME);
                cmd.Parameters.Add("@Result", SqlDbType.Bit, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                result = Convert.ToBoolean(cmd.Parameters["@Result"].Value);
            }

            return result;
        }
        public void SendReports(UsersDetails users)
        {
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spSendReports", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", users.USERNAME);
                cmd.Parameters.AddWithValue("@Message", users.MESSAGE);
                cmd.Parameters.AddWithValue("@ID", users.ID);
                cmd.Parameters.AddWithValue("@LATITUDE", users.LATITUDE);
                cmd.Parameters.AddWithValue("@LONGITUDE", users.LONGITUDE);
                cmd.Parameters.AddWithValue("@Date", string.Format("{0:HH:mm:ss tt}", DateTime.Now));
                cmd.ExecuteNonQuery();
            }
        }
        public List<UsersDetails> GetReports()
        {
            List<UsersDetails> users = new List<UsersDetails>();

            using (conn)
            {
                DataTable dt = new DataTable();
                conn.Open();
                SqlCommand cmd = new SqlCommand("spGetReports", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlDataAdapter sqlData = new SqlDataAdapter();
                sqlData.SelectCommand = cmd;
                sqlData.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var list = new UsersDetails();
                    list.FULLNAME = row["FNAME"].ToString();
                    list.MESSAGE = row["MESSAGE"].ToString();
                    list.CREATED = row["CREATED"].ToString();
                    users.Add(list);
                }
            }
            return users;
        }
    }
    public class UsersDetails
    {
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public double LATITUDE { get; set; }
        public double LONGITUDE { get; set; }
        public string MESSAGE { get; set; }
        public string FULLNAME { get; set; }
        public int ID { get; set; }
        public string CREATED { get; set; }
    }
    public class UsersClass
    {
        public List<UsersDetails> details { get; set; }
    }
}