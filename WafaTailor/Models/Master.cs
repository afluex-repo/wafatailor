using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Master
    {
        public List<Master> lstRegistration { get; set; }
        public String @ShopName { get; set; }
        public String Address { get; set; }
        public String MaterialName { get; set; }
        public String MaterialType { get; set; }

        public String ShopId { get; set; }
        public String MaterialId { get; set; }
        public String Status { get; set; }

        public string LoginId { get; set; }
        public string AddedBy { get; set; }


        public DataSet ShopMaster()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@ShopName",ShopName),
                new SqlParameter("@ShopAddress",Address),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("ShopMaster", para);
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
        public DataSet DeleteShopMaster()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_ShopId",ShopId),
                new SqlParameter("@DeletedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteShopMaster", para);
            return ds;
        }


        public DataSet SaveMaterial()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@MaterialName",MaterialName),
                new SqlParameter("@MaterialType",MaterialType),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("Material", para);
            return ds;
        }

        public DataSet GetMaterialDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_MaterialId",MaterialId),
                //new SqlParameter("@Status", Status),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetMaterialDetails", para);
            return ds;
        }

        public DataSet DeleteMaterial()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_MaterialId",MaterialId),
                 new SqlParameter("@DeletedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteMaterial", para);
            return ds;
        }

        public DataSet ActiveShop()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@ShopId", ShopId),
                                      new SqlParameter("@ApprovedBy", AddedBy)
                                     };
            DataSet ds = DBHelper.ExecuteQuery("ActiveShop", para);
            return ds;
        }

        public DataSet InactiveShop()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@ShopId", ShopId),
                                      new SqlParameter("@RejectedBy", AddedBy)
                                     };
            DataSet ds = DBHelper.ExecuteQuery("InactiveShop", para);
            return ds;
        }
    }
}