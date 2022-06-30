using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class SellOrder
    {
        public string SellOrderId { get; set; }
        public string Fk_UserId { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string AddedBy { get; set; }
        public string LoginId { get; set; }
        public string OnWakingUp { get; set; }
        
        public DataTable dtSellOrderDetails { get; set; }
        
        public DataSet SaveSellOrderDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@dtSellOrderDetails",dtSellOrderDetails),
                 new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveSellOrderDetails", para);
            return ds;
        }


    }
}