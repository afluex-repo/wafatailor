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

        public DataTable CustomerName { get; set; }
        public DataTable CustomerAddress { get; set; }
        
        //public string InvoiceNo { get; set; }
        //public string PK_InvoiceNoID { get; set; }
        //public string InvoiceDate { get; set; }
        //public string LineStatus { get; set; }
        //public string SaleOrderNo { get; set; }


        public string SaleOrderNoEncrypt { get; set; }

        public DataSet SaveSaleOrderDetails()
        {
            SqlParameter[] para =
            {
                    new SqlParameter("@Fk_UserId",Fk_UserId),
                   new SqlParameter("@Fk_ShopeId",Fk_ShopeId),
                   new SqlParameter("@dtSaleOrderDetails",dtSaleOrderDetails),
                 new SqlParameter("@AddedBy",AddedBy)
            };
              DataSet ds = DBHelper.ExecuteQuery("SaveSaleOrderDetails", para);
            return ds;
        }

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
                new SqlParameter("@Pk_SaleOrderDetailsId",SaleOrderId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderDetails", para);
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

       
    }
}