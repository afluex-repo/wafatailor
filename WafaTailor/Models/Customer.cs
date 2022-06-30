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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerAddress { get; set; }
        public string DOB { get; set; }
        public string ContactNo { get; set; }
        public string Emailid { get; set; }
        public string Gender { get; set; }
        public string UserID { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }



        public DataSet CustomerRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FirstName",FirstName),
                new SqlParameter("@LastName",LastName),
                new SqlParameter("@CustomerAddress",CustomerAddress),
                 new SqlParameter("@DOB",DOB),
                new SqlParameter("@ContactNo",ContactNo),
                 new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Password",Password),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("CustomerRegistration", para);
            return ds;
        }

        public DataSet GetCustomerDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_UserID",UserID)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerDetails", para);
            return ds;
        }
        public DataSet DeleteCustomer()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_UserID",UserID),
                 new SqlParameter("@DeletedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteCustomer", para);
            return ds;
        }

        public DataSet UpdateCustomerRegistration()
        {
            SqlParameter[] para =
            {
                 new SqlParameter("@PK_UserID",UserID),
               new SqlParameter("@FirstName",FirstName),
                new SqlParameter("@LastName",LastName),
                new SqlParameter("@CustomerAddress",CustomerAddress),
                 new SqlParameter("@DOB",DOB),
                new SqlParameter("@Mobile",ContactNo),
                 new SqlParameter("@Email",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Password",Password),
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