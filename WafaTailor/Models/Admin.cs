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
        public List<Admin> lstsaleorder { get; set; }
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
        public string DeliveredPiece { get; set; }
        public string OriginalPrice { get; set; }
        public string Discount { get; set; }
        public string FinalPrice { get; set; }
        public string Advance { get; set; }
        public string RemainningBalance { get; set; }
        public string BillDate { get; set; }
        public string BillId { get; set; }
        public string Pk_BillPaymentId { get; set; }
        public string ShopName { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }
        public decimal Balance { get; set; }
        public string TotalPaid { get; set; }
        public string SalesOrderNo { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SaleOrderDate { get; set; }

        public string RefundId { get; set; }
        public string PieceName { get; set; }

        public string Pk_BillId { get; set; }
        public string AvailableNoOfPiece { get; set; }
        public string Result { get; set; }
        public string RemainingPiece { get; set; }
        public string Status { get; set; }
        


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
                new SqlParameter("@DeliveredPiece",DeliveredPiece),
                 //new SqlParameter("@RemainingPiece",RemainingPiece),
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
                new SqlParameter("@Fk_BillPaymentId",Pk_BillPaymentId),
                new SqlParameter("@LoginId",LoginId)
                //new SqlParameter("@FromDate", FromDate),
                //new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBillDetails", para);
            return ds;
        }
        public DataSet PrintBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
                new SqlParameter("@Fk_BillPaymentId",Pk_BillPaymentId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintBill", para);
            return ds;
        }
        public DataSet BillPayment()
        {
            SqlParameter[] para =
            {
                //new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@Fk_billId",BillId),
                new SqlParameter("@DeliveredPiece",DeliveredPiece),
                new SqlParameter("@AdvanceAmount",Advance),
                new SqlParameter("@BillDate",BillDate),
                new SqlParameter("@FK_UserId",FK_UserId),
                new SqlParameter("@AddedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("BillPayment", para);
            return ds;
        }

        public DataSet OrderRefund()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@PieceName",PieceName),
                 new SqlParameter("@AvailableNoOfPiece",AvailableNoOfPiece),
                  new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@Mobile",Mobile),
                new SqlParameter("@Amount",Balance),
                new SqlParameter("@AddedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("OrderRefund", para);
            return ds;
        }


        public DataSet GetOrderRefundDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_RefundId",RefundId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetOrderRefundDetails", para);
            return ds;
        }

        public DataSet GetBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@BillNo",BillNo)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAvailableOrder", para);
            return ds;
        }
        public DataSet PrintOrderRefundBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",RefundId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintOrderRefund", para);
            return ds;
        }

    }
}