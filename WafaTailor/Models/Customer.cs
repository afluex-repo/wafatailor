using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Customer: Common
    {
        public List<Customer> lstRegistration { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string DOB { get; set; }
        public string ContactNo { get; set; }
        public string Emailid { get; set; }
        public string Gender { get; set; }
        public string CustomerId { get; set; }
        public string LoginId { get; set; }
        


        public DataSet CustomerRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@CustomerName",CustomerName),
                new SqlParameter("@CustomerAddress",CustomerAddress),
                 new SqlParameter("@DOB",DOB),
                new SqlParameter("@ContactNo",ContactNo),
                 new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveCustomerRegistration", para);
            return ds;
        }

        public DataSet GetCustomerDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_CustomerId",CustomerId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerDetails", para);
            return ds;
        }
        public DataSet DeleteCustomer()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_CustomerId",CustomerId),
                 new SqlParameter("@DeletedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteCustomer", para);
            return ds;
        }

        public DataSet UpdateCustomerRegistration()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@Pk_CustomerId",CustomerId),
                new SqlParameter("@CustomerName",CustomerName),
                new SqlParameter("@CustomerAddress",CustomerAddress),
                 new SqlParameter("@DOB",DOB),
                new SqlParameter("@ContactNo",ContactNo),
                 new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@UpdatedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateCustomerRegistration", para);
            return ds;
        }

        public DataSet GetCustomerProfileDetails()
        {
            SqlParameter[] para =
       {
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerProfileDetails", para);
            return ds;
        }

    }
}