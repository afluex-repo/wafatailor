using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Employee
    {
        public List<Employee> lstRegistration { get; set; }
        public string ShopName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeAddress { get; set; }
        public string DOB { get; set; }
        public string ContactNo { get; set; }
        public string Emailid { get; set; }
        public string Gender { get; set; }
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string LoginId { get; set; }
        public string AddedBy { get; set; }
        

        public DataSet EmployeeRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@ShopName",ShopName),
                new SqlParameter("@EmployeeName",EmployeeName),
                 new SqlParameter("@EmployeeAddress",EmployeeAddress),
                new SqlParameter("@DOB",DOB),
                 new SqlParameter("@ContactNo",ContactNo),
                new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveEmployeeRegistration", para);
            return ds;
        }

        public DataSet GetEmployeeDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmployeeId",EmployeeId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeDetails", para);
            return ds;
        }

        public DataSet DeleteEmployee()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmployeeId",EmployeeId),
                 new SqlParameter("@DeletedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteEmployee", para);
            return ds;
        }
        public DataSet EmployeeChangePassword()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@OldPassword",Password),
                new SqlParameter("@NewPassword",NewPassword),
                 new SqlParameter("@UpdatedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("EmployeeChangePassword", para);
            return ds;
        }

        public DataSet GetProfileDetails()
        {
                 SqlParameter[] para =
            {
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetProfileDetails",para);
            return ds;
        }
      
    }
}