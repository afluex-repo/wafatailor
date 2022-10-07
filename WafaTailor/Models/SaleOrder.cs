using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class SaleOrder
    {
        public List<SaleOrder> lstList { get; set; }
        public List<SaleOrder> lstInvoiceNo { get; set; }
        public List<SaleOrder> lstRegistration { get; set; }
        public List<SaleOrder> lstsaleorder { get; set; }
        public List<SaleOrder> lstCompaign { get; set; }
        public string SaleOrderId { get; set; }
        public string Fk_UserId { get; set; }
        public string Fk_ShopeId { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string AddedBy { get; set; }
        public string LoginId { get; set; }
        public string Result { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DataTable dtSaleOrderDetails { get; set; }

        public string CustomerName { get; set; }
        //public DataTable CustomerName { get; set; }
        public DataTable CustomerAddress { get; set; }

        public string SaleDate { get; set; }
        public string PieceName{ get; set; }
        public string NoOfPiece { get; set; }
        public string OriginalPrice { get; set; }
        public string Discount { get; set; }
        public string FinalPrice { get; set; }

        public string ShopName { get; set; }
        public string BillNo { get; set; }
        public string SalesOrderNo { get; set; }


        public string SaleOrderNoEncrypt { get; set; }
        public string PriceName { get; set; }
        public string NetAmount { get; set; }
        public string SaleOrderDate { get; set; }
        public string CustomerId { get; set; }
        public string Pk_UserId { get; set; }
        public string TotalDeliveredPiece { get; set; }
        public DataTable dt { get; set; }
        public string ShopId { get; set; }
        public string ShopLoginId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
            
        public string AvailableNoOfPiece { get; set; }
        public decimal Balance { get; set; }
        public string RefundDate { get; set; }
        public string RefundId { get; set; }
        


        //public DataSet SaveSaleOrderDetails()
        //{
        //    SqlParameter[] para =
        //    {
        //            new SqlParameter("@Fk_UserId",Fk_UserId),
        //           new SqlParameter("@Fk_ShopeId",Fk_ShopeId),
        //           new SqlParameter("@dtSaleOrderDetails",dtSaleOrderDetails),
        //         new SqlParameter("@AddedBy",AddedBy)
        //    };
        //      DataSet ds = DBHelper.ExecuteQuery("SaveSaleOrderDetails", para);
        //    return ds;
        //}

        public DataSet GetUserDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetUserDetails", para);
            return ds;
        }


        public DataSet GetSaleOrderDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@ShopLoginId",ShopLoginId),
                new SqlParameter("@CustomerLoginId",LoginId),
                new SqlParameter("@Mobile",Mobile)
                //new SqlParameter("@FromDate", FromDate),
                //new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderForShop", para);
            return ds;
        }

        public DataSet PrintSO()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_SaleOrderId", SaleOrderId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintSaleOrder", para);
            return ds;
        }

        //public DataSet GenerateInvoiceNo()
        //{
        //    SqlParameter[] para = { new SqlParameter("@AddedBy", AddedBy), };
        //    DataSet ds = DBHelper.ExecuteQuery("GenerateInvoiceNo", para);
        //    return ds;
        //}

        //public DataSet GetInvoiceNoList()
        //{
        //    SqlParameter[] para = { new SqlParameter("@InvoiceNo", InvoiceNo), };
        //    DataSet ds = DBHelper.ExecuteQuery("GetInvoiceNoList", para);
        //    return ds;
        //}

        public DataSet GetCustomerDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FK_CustomerId",Pk_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerForSaleOrder", para);
            return ds;
        }

        public DataSet SaveSaleOrder()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@Name",LoginId),
             new SqlParameter("@Fk_Userid",Pk_UserId),
                new SqlParameter("@Mobile",Mobile),
                //new SqlParameter("@Fk_CustomerId",CustomerId),
                new SqlParameter("@dt",dt),
                 new SqlParameter("@BillId",BillId),
                new SqlParameter("@PaymentId",PaymentId),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveShopSaleOrderDetails", para);
            return ds;
        }
        public DataSet GetShopNameDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetShopNameDetails");
            return ds;
        }

        public DataSet GetBillDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
                new SqlParameter("@Fk_BillPaymentId",PaymentId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBillDetails", para);
            return ds;
        }

        public DataSet RefundSaleOrder()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@RefundPiece",NoOfPiece),
                //new SqlParameter("@Mobile",Mobile),
                new SqlParameter("@Amount",Balance),
                new SqlParameter("@RefundDate",RefundDate),
                new SqlParameter("@AddedBy",AddedBy),
                 new SqlParameter("@Fk_Shopid",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("RefundSaleOrder", para);
            return ds;
        }

        public DataSet GetOrderRefundDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_SaleOrderRefundId",RefundId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderRefundDetails", para);
            return ds;
        }
        public DataSet PrintSaleOrderRefundBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_SaleOrderRefundId",RefundId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintSaleOrderRefund", para);
            return ds;
        }

      
        public DataSet UpdateSaleOrder()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@OriginalPrice",OriginalPrice),
                 new SqlParameter("@SaleOrderId",SaleOrderId)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateShopSaleOrderDetails", para);
            return ds;
        }

        public DataSet SaveSaleOrdernew()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@Name",LoginId),
             new SqlParameter("@Fk_Userid",Pk_UserId),
                new SqlParameter("@Mobile",Mobile),
                //new SqlParameter("@Fk_CustomerId",CustomerId),
                new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@OriginalPrice",OriginalPrice),
                new SqlParameter("@Discount",Discount),
                new SqlParameter("@NetAmount",NetAmount),
                new SqlParameter("@SaleOrderDate",SaleOrderDate),
                new SqlParameter("@Description",Description),
                 new SqlParameter("@BillId",BillId),
                new SqlParameter("@PaymentId",PaymentId),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveShopSaleOrderDetailsNew", para);
            return ds;
        }
    }
}