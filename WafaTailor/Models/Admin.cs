using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Admin
    {
        public string LoginId { get; set; }

        public DataSet GetAdminDashBoardDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetAdminDashBoardDetails");
            return ds;
        }

        public DataSet GetAdminProfileDetails()
        {
            SqlParameter[] para =
       {
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAdminProfileDetails", para);
            return ds;
        }



    }
}