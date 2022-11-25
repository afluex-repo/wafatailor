using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WafaTailor.Models
{
    public class Admin
    {
        public List<Admin> lstStockEntry { get; set; }
        public List<Admin> lstRegistration { get; set; }
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
        public string GeneratedPiece { get; set; }
        public string GeneratedAmount { get; set; }
        public string TotalDeliveredPiece { get; set; }
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

        public string FinalAmount { get; set; }
        public string RefundDate { get; set; }

        public string ProductName { get; set; }

        public List<SelectListItem> ddlproduct { get; set; }
        public List<Admin> lstProduct { get; set; }


        public string Fk_ProductId { get; set; }
        public string Fk_ShopId { get; set; }
        public string Amount { get; set; }
        public string PK_StockId { get; set; }
        public string Pk_ProductId { get; set; }
        public string Today { get; set; }
        public string Yesterday { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDateBD { get; set; }
        public string UpdatedDateBP { get; set; }
        public string CreatedDateNew { get; set; }


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
                new SqlParameter("@LoginId",Pk_UserId),
                new SqlParameter("@Mobile",Mobile),
                 new SqlParameter("@Status",Status),
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
                new SqlParameter("@LoginId",LoginId),
                new SqlParameter("@FromDate", FromDate),
                new SqlParameter("@ToDate", ToDate),
                 new SqlParameter("@Mobile", Mobile),
                //new SqlParameter("@Customername",Name)
                new SqlParameter("@BillNo",BillNo)
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
                new SqlParameter("@Status",Status),
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
                  new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@Mobile",Mobile),
                new SqlParameter("@Amount",Balance),
                new SqlParameter("@RefundDate",RefundDate),
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
            //DataSet ds = DBHelper.ExecuteQuery("GetAvailableOrder", para);
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderrefund", para);
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

        public DataSet UpdateBillEntry()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
                new SqlParameter("@Pk_BillPaymentId",Pk_BillPaymentId),
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@OriginalPrice",OriginalPrice),
                new SqlParameter("@AdvanceAmount",Advance),
                new SqlParameter("@FinalPrice",FinalPrice),
                new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@DeliveredPiece",DeliveredPiece),
                new SqlParameter("@BillDate",BillDate),
                new SqlParameter("@Name",LoginId),
                new SqlParameter("@Fk_Userid",Pk_UserId),
                new SqlParameter("@Mobile",Mobile),
                 new SqlParameter("@Status",Status),
                new SqlParameter("@UpdatedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateBillingDetails", para);
            return ds;
        }

        public DataSet GetShopMaster()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_ShopId",ShopId),
               //new SqlParameter("@Status", Status),
            };
            DataSet ds = DBHelper.ExecuteQuery("Getshopmaster", para);
            return ds;
        }



        public DataSet SaveProductMaster()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@ProductName",ProductName),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveProductMaster", para);
            return ds;
        }


        public DataSet UpateProductMaster()
        {
            SqlParameter[] para =
            {
                   new SqlParameter("@Pk_ProductId",Pk_ProductId),
                new SqlParameter("@ProductName",ProductName),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpateProductMaster", para);
            return ds;
        }
        
        public DataSet GetProductList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetProductList");
            return ds;
        }

        public DataSet SaveStockEntry()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_ProductId",Fk_ProductId),
                 new SqlParameter("@NoOfPiece",NoOfPiece),
                  new SqlParameter("@AmountPerPiece",Amount),
                   new SqlParameter("@Fk_ShopId",Fk_ShopId),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveStockEntry", para);
            return ds;
        }

        public DataSet GetStockEntryList()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_StockId",PK_StockId),
                new SqlParameter("@Fk_ProductId",Fk_ProductId),
                new SqlParameter("@Fk_ShopId",Fk_ShopId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetStockEntryList", para);
            return ds;
        }


        public DataSet DeleteStockEntry()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_StockId",PK_StockId),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteStockEntry", para);
            return ds;
        }


        public DataSet UpdateStockEntry()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@PK_StockId",PK_StockId),
                new SqlParameter("@Fk_ProductId",Fk_ProductId),
                 new SqlParameter("@NoOfPiece",NoOfPiece),
                  new SqlParameter("@AmountPerPiece",Amount),
                   new SqlParameter("@Fk_ShopId",Fk_ShopId),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateStockEntry", para);
            return ds;
        }


        public DataSet ProductList()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@Pk_ProductId",Pk_ProductId)
            };
            DataSet ds = DBHelper.ExecuteQuery("ProductList", para);
            return ds;
        }



        public DataSet DeleteProduct()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@Pk_ProductId",Pk_ProductId),
               new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteProduct", para);
            return ds;
        }



    }
}