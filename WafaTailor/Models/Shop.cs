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
                new SqlParameter("@Fk_CustomerId",CustomerId),
                new SqlParameter("@dt",dt)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveShopSaleOrderDetails", para);
            return ds;
        }


        public DataSet GetShopSaleOrderDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_SaleOrderDetailsId",SaleOrderId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderDetails", para);
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
    }
}