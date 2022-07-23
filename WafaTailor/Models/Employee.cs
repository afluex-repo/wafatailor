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
        public List<Employee> lstSalary { get; set; }
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
        public string Fk_ShopId { get; set; }
        public string UserTypeId { get; set; }
        public string SalaryType { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string JoiningDate { get; set; }
        public string EmployeeDetails { get; set; }
        public string Salary { get; set; }
        public string Remark { get; set; }
        public string Date { get; set; }

        public string CrAmount { get; set; }
        public string DrAmount { get; set; }
        public string Type { get; set; }
        public string SalaryId { get; set; }
        public string Fk_EmployeeId { get; set; }
        public string PaymentMode { get; set; }
        public string BankBranch { get; set; }
        public string BankName { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }

        public DataSet EmployeeRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_ShopId",Fk_ShopId),
                 new SqlParameter("@Fk_UserTypeId",UserTypeId),
                new SqlParameter("@EmployeeName",EmployeeName),
                 new SqlParameter("@EmployeeAddress",EmployeeAddress),
                new SqlParameter("@DOB",DOB),
                 new SqlParameter("@ContactNo",ContactNo),
                new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Salary",Salary),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("EmployeeRegistration", para);
            return ds;
        }

        public DataSet GetEmployeeDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmployeeId",EmployeeId),
                new SqlParameter("@LoginId", LoginId),
                new SqlParameter("@FromDate", FromDate),
                new SqlParameter("@ToDate", ToDate),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeDetails", para);
            return ds;
        }


        public DataSet GetShopNameDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetShopNameDetails");
            return ds;
        }

        public DataSet GetUserTypeDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetUserTypeDetails");
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
        


        public DataSet updateEmployeeRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmployeeId",EmployeeId),
                new SqlParameter("@ShopName",ShopName),
                new SqlParameter("@Name",EmployeeName),
                 new SqlParameter("@Address",EmployeeAddress),
                new SqlParameter("@DOB",DOB),
                 new SqlParameter("@ContactNo",ContactNo),
                new SqlParameter("@Emailid",Emailid),
                new SqlParameter("@Gender",Gender),
                new SqlParameter("@Salary",Salary),
                new SqlParameter("@UpdatedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateEmployeeRegistration", para);
            return ds;
        }


        public DataSet SalaryManagement()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_EmployeeId",Fk_EmployeeId),
                 new SqlParameter("@SalaryType",SalaryType),
                new SqlParameter("@Amount",Salary),
                 new SqlParameter("@SalaryDate",Date),
                 new SqlParameter("@PaymentMode",PaymentMode),
                new SqlParameter("@PaymentRemarks",Remark),
                new SqlParameter("@BankName",BankName),
               new SqlParameter("@BranchName",BankBranch),
               new SqlParameter("@TransactionNo",TransactionNo),
               new SqlParameter("@TransactionDate",TransactionDate),
                new SqlParameter("@AddedBy",1)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveEmployeeSalary", para);
            return ds;
        }


        public DataSet GetEmployeeNameDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeNameDetails");
            return ds;
        }

        public DataSet GetSalaryTypeDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetSalaryTypeDetails");
            return ds;
        }

        public DataSet GetSalaryDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmpSalaryId",SalaryId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSalaryDetails", para);
            return ds;
        }

        public DataSet GetPaymentMode()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetPaymentModeList");
            return ds;
        }
    }
}