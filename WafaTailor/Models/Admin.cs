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
        public List<Admin> lstVendor { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string AddedBy { get; set; }
        public string EmployeeId { get; set; }
        public string FK_UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public DataSet GetAdminDashBoardDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetAdminDashBoardDetails");
            return ds;
        }

        public DataSet GetAdminProfileDetails()
        {
            SqlParameter[] para =
       {
                new SqlParameter("@Pk_EmployeeId",EmployeeId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAdminProfileDetails", para);
            return ds;
        }
        public DataSet ChangePassword()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@OldPassword",Password),
                new SqlParameter("@NewPassword",NewPassword),
                 new SqlParameter("@UpdatedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("AdminChangePassword", para);
            return ds;
        }

        public DataSet GetVendorList()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_UserId",FK_UserId),
                 new SqlParameter("@LoginId",LoginId),
                   new SqlParameter("@FromDate", FromDate),
                new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetVendorList", para);
            return ds;
        }
    }
}