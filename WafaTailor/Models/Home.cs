using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Home
    {
        public String LoginId { get; set; }
        public String Password { get; set; }

        public DataSet Login()
        {
            SqlParameter[] para ={
                                new SqlParameter ("@LoginId",LoginId),
                                new SqlParameter("@Password",Password)
            };
            DataSet ds = DBHelper.ExecuteQuery("Login", para);
            return ds;
        }
    }
}