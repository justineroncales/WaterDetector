using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WaterDetector.Code;

namespace WaterDetector
{
    public class NotificationComponent
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["WDetector"].ConnectionString);
        public void RegisterNotification(DateTime currentTime)
        {
            string connection = ConfigurationManager.ConnectionStrings["WDetector"].ConnectionString;
            string sqlCommand = @"SELECT [REPORTID],[MESSAGE] from [dbo].[REPORT]";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                //re-register notification
                RegisterNotification(DateTime.Now);
            }
        }

        public List<Contact> GetContacts(DateTime afterDate)
        {
            List<Contact> users = new List<Contact>();
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
                    var list = new Contact();
                    list.ContactName = row["FNAME"].ToString();
                    list.ContactNo = row["MESSAGE"].ToString();
                    users.Add(list);
                }
                return users;
            }
        }
    }
}