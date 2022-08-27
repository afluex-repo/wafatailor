using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Shop :Common
    {
        public List<Shop> lstList { get; set; }
        public List<Shop> lstShopRegistration { get; set; }
        public List<Shop> lstshopsaleorder { get; set; }
        public string CustomerId { get; set; }
        public string Mobile { get; set; }
        public string BillNo { get; set; }
        public string NoOfPiece { get; set; }
        public string PieceName { get; set; }
        public string OriginalPrice { get; set; }
        public string Discount { get; set; }
        public string NetAmount { get; set; }
        public string SaleOrderDate { get; set; }
        public string PriceName { get; set; }
        public string Pk_UserId { get; set;}
        public string Description { get; set; }
        public DataTable dt { get; set; }

        public string SaleOrderId { get; set; }
        public string SaleDate { get; set; }
        public string FinalPrice { get; set; }

        public string SalesOrderNo { get; set; }
        public string customerName { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        # region
        public string LoginId { get; set; }
        public string Advance { get; set; }
        public string RemainningBalance { get; set; }
        public string BillDate { get; set; }
        public string Name { get; set; }
        public string ShopId { get; set; }
        public string BillId { get; set; }
        public string Pk_BillPaymentId { get; set; }
        public decimal Balance { get; set; }
        public string ShopLoginId { get; set; }

        public string DeliveredPiece { get; set; }
        public string RemainingPiece { get; set; }
        public string Status { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }



        #endregion

        public DataSet GetCustomerDetails()
        {
             SqlParameter[] para =
             {
                new SqlParameter("@FK_CustomerId",Pk_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerForSaleOrder",para);
            return ds;
        }
        public DataSet SaveSaleOrder()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@Fk_ShopId",AddedBy),
                new SqlParameter("@Fk_Userid",AddedBy),
                new SqlParameter("@dt",dt),
                  new SqlParameter("@Name",LoginId),
                new SqlParameter("@Mobile",Mobile),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveShopSaleOrderDetails", para);
            return ds;
        }


        public DataSet GetShopSaleOrderDetails()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@ShopLoginId",ShopLoginId),
                new SqlParameter("@CustomerLoginId",LoginId),
                //new SqlParameter("@Fk_ShopId",AddedBy),
                  //new SqlParameter("@LoginId", LoginId),
                  new SqlParameter("@Name", Name),
                  new SqlParameter("@Mobile", Mobile),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderForShop", para);
            return ds;
        }
        public DataSet PrintShopSO()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_SaleOrderId", SaleOrderId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintSaleOrder", para);
            return ds;
        }
        //public DataSet DeleteShopSaleOrder()
        //{
        //    SqlParameter[] para =
        //    {
        //        new SqlParameter("@Pk_SaleOrderId", SaleOrderId)
        //    };
        //    DataSet ds = DBHelper.ExecuteQuery("DeleteShopSaleOrder", para);
        //    return ds;
        //}

        public DataSet GetCustomerDetail()
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
                new SqlParameter("@Fk_ShopId",AddedBy),
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
                new SqlParameter("@AddedBy",AddedBy)
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

        public DataSet ShopChangePassword()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@OldPassword",Password),
                new SqlParameter("@NewPassword",NewPassword),
                 new SqlParameter("@UpdatedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("ShopChangePassword", para);
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
    }
}