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
        public List<Admin> lstList { get; set; }
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
        public string JoiningDate { get; set; }

        public string Pk_UserId { get; set; }
        public string ShopId { get; set; }
        public string BillNo { get; set; }
        public string NoOfPiece { get; set; }
        public string OriginalPrice { get; set; }
        public string Discount { get; set; }
        public string FinalPrice { get; set; }
        public string Advance { get; set; }
        public string RemainningBalance { get; set; }
        public string BillDate { get; set; }
        public string BillId { get; set; }
        public string ShopName { get; set; }

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

        public DataSet GetShopNameDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetShopNameDetails");
            return ds;
        }
        public DataSet GetCustomerDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FK_CustomerId",Pk_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerForSaleOrder", para);
            return ds;
        }

        public DataSet SaveBillEntry()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@OriginalPrice",OriginalPrice),
                new SqlParameter("@AdvanceAmount",Advance),
                new SqlParameter("@FinalPrice",FinalPrice),
                new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@BillDate",BillDate),
                new SqlParameter("@Name",LoginId),
                new SqlParameter("@Fk_Userid",FK_UserId),
                new SqlParameter("@Mobile",Mobile),
                new SqlParameter("@AddedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveBillingDetails", para);
            return ds;
        }

        public DataSet GetBillDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
                new SqlParameter("@FromDate", FromDate),
                new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBillDetails", para);
            return ds;
        }
        public DataSet PrintBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintBill", para);
            return ds;
        }
    }
}