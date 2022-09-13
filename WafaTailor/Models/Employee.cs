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
        public DataTable dtTable { get; set; }
        public List<Employee> lstRegistration { get; set; }
        public List<Employee> lstSalary { get; set; }
        public List<Employee> lstList { get; set; }
        public List<Employee> lstsaleorder { get; set; }
        public List<Employee> lstexpense { get; set; }
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
        public string RemainingSalary { get; set; }
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

        public string AttendanceDate { get; set; }
        public string IsPresent { get; set; }
        public string WHLimit { get; set; }
        public string Attendance { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ISHalfDay { get; set; }
        public string OverTime { get; set; }
        public string TotalHRWork { get; set; }

        public string ShopId { get; set; }
        public string Pk_UserId { get; set; }
        public string BillNo { get; set; }
        public string NoOfPiece { get; set; }
        public string DeliveredPiece { get; set; }
        public string OriginalPrice { get; set; }
        public string RemainingPiece { get; set; }
        public string FinalPrice { get; set; }
        public string Advance { get; set; }
        public string RemainningBalance { get; set; }
        public string BillDate { get; set; }
        public string Status { get; set; }
        public string BillId { get; set; }
        public string Pk_BillPaymentId { get; set; }
        public string Discount { get; set; }
        public string NetAmount { get; set; }
        public string SaleOrderDate { get; set; }
        public string Description { get; set; }
        public string PaymentId { get; set; }
        public string Fk_UserId { get; set; }
        public string Result { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Expensetype { get; set; }
        public string OtherExpensetype { get; set; }
        public string Vendor { get; set; }
        public string ExpenseRupee { get; set; }
        public string ExpenseDate { get; set; }
        public DataTable dt { get; set; }
        public string GeneratedPiece { get; set; }
        public string GeneratedAmount { get; set; }
        public decimal Balance { get; set; }
        public string TotalDeliveredPiece { get; set; }
        public string TotalPaid { get; set; }
        public string SaleOrderId { get; set; }
        public string SalesOrderNo { get; set; }
        public string CustomerName { get; set; }
        public string ShopLoginId { get; set; }
        public string SaleDate { get; set; }
        public string PieceName { get; set; }
        public string Pk_ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public string Expenses { get; set; }
        public string OtherExpenseName { get; set; }
        public string RefundId { get; set; }
        public string RefundDate { get; set; }
        public string AvailableNoOfPiece { get; set; }


        public DataSet EmployeeRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_ShopId",Fk_ShopId),
                 new SqlParameter("@Fk_UserTypeId",UserTypeId),
                new SqlParameter("@EmployeeName",EmployeeName),
                 new SqlParameter("@EmployeeAddress",EmployeeAddress),
                //new SqlParameter("@DOB",DOB),
                 new SqlParameter("@ContactNo",ContactNo),
                //new SqlParameter("@Emailid",Emailid),
                //new SqlParameter("@Gender",Gender),
                new SqlParameter("@Salary",Salary),
                new SqlParameter("@AddedBy",AddedBy)
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
                 new SqlParameter("@DeletedBy",AddedBy)
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
                new SqlParameter("@UpdatedBy",AddedBy)
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
                new SqlParameter("@AddedBy",AddedBy)
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
                new SqlParameter("@Pk_EmpId",EmployeeId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetSalaryDetails", para);
            return ds;
        }
        public DataSet GetSalaryLedger()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_EmpId",EmployeeId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeSalaryLedger", para);
            return ds;
        }

        public DataSet GetPaymentMode()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetPaymentModeList");
            return ds;
        }

        public DataSet EmployeeListForAttendance()
        {
            SqlParameter[] para =
                            {
                                new SqlParameter("@EmployeeName",EmployeeName),
                                //new SqlParameter("@EmployeeCode",EmployeeLoginId),
                                new SqlParameter("@IsPresent",IsPresent)
                            };
            DataSet ds = DBHelper.ExecuteQuery("GetEmployeeForAttendance", para);
            return ds;

        }

        public DataSet SaveEmployeeDailyAttendance()
        {
            SqlParameter[] para =
                            {
                                new SqlParameter("@EmployeeAttendance",dtTable),
                                new SqlParameter("@AttendanceDate",AttendanceDate),
                                  new SqlParameter("@AddedBy",AddedBy),
                            };
            DataSet ds = DBHelper.ExecuteQuery("DailyAttendancePosting", para);
            return ds;

        }

        public DataSet DateWiseAttendanceReportBy()
        {
            SqlParameter[] para =
                            {
                                new SqlParameter("@EmployeeName",EmployeeName),
                                new SqlParameter("@FromDate",FromDate),
                                new SqlParameter("@ToDate",ToDate),
                            };
            DataSet ds = DBHelper.ExecuteQuery("DateWiseAttendanceReport", para);
            return ds;

        }
        public DataSet GetCustomerDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FK_CustomerId",Pk_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCustomerForSaleOrder", para);
            return ds;
        }

        public DataSet SaveEmployeeBillEntry()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@OriginalPrice",OriginalPrice),
                new SqlParameter("@AdvanceAmount",Advance),
                new SqlParameter("@FinalPrice",FinalPrice),
                new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@DeliveredPiece",DeliveredPiece),
                 //new SqlParameter("@RemainingPiece",RemainingPiece),
                new SqlParameter("@BillDate",BillDate),
                new SqlParameter("@Name",LoginId),
                new SqlParameter("@LoginId",Pk_UserId),
                new SqlParameter("@Mobile",ContactNo),
                 new SqlParameter("@Status",Status),
                new SqlParameter("@AddedBy",AddedBy),
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
                 new SqlParameter("@Mobile", ContactNo), 
                 new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetBillDetails", para);
            return ds;
        }

        public DataSet BillPayment()
        {
            SqlParameter[] para =
            {
                //new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@Fk_billId",BillId),
                new SqlParameter("@DeliveredPiece",DeliveredPiece),
                new SqlParameter("@AdvanceAmount",Advance),
                new SqlParameter("@BillDate",BillDate),
                new SqlParameter("@FK_UserId",Fk_UserId),
                new SqlParameter("@Status",Status),
                new SqlParameter("@AddedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("BillPayment", para);
            return ds;
        }

        public DataSet EmployeePrintBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",BillId),
                new SqlParameter("@Fk_BillPaymentId",Pk_BillPaymentId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintBill", para);
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

        public DataSet GetExpenseType()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseType");
            return ds;
        }

        public DataSet GetOtherExpenseType()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetOtherExpenseType");
            return ds;
        }

        public DataSet GetVendor()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetVendor");
            return ds;
        }

        public DataSet SaveEmployeeExpense()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@dt",dt)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpenseDetails", para);
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
                new SqlParameter("@Mobile",ContactNo),
                 new SqlParameter("@Status",Status),
                new SqlParameter("@UpdatedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateBillingDetails", para);
            return ds;
        }

        public DataSet SaveEmployeeSaleOrder()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@Fk_ShopId",ShopId),
                new SqlParameter("@Name",LoginId),
             new SqlParameter("@Fk_Userid",Pk_UserId),
                new SqlParameter("@Mobile",ContactNo),
                new SqlParameter("@dt",dt),
                 new SqlParameter("@BillId",BillId),
                new SqlParameter("@PaymentId",PaymentId),
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveShopSaleOrderDetails", para);
            return ds;
        }

        public DataSet GetEmployeeSaleOrderDetails()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@ShopLoginId",ShopLoginId),
                //new SqlParameter("@CustomerLoginId",LoginId),
                new SqlParameter("@Name",Name),
                new SqlParameter("@Mobile",ContactNo),
                new SqlParameter("@AddedBy", AddedBy)
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

        public DataSet GetEmployeeExpenseList()
        {
            SqlParameter[] para ={
                new SqlParameter("@Fk_ExpensetypeId",Expensetype),
                new SqlParameter("@FromDate",FromDate),
                new SqlParameter("@ToDate",ToDate),
                new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseList", para);
            return ds;
        }

        public DataSet GetEmployeeOrderRefundList()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_RefundId",RefundId),
                new SqlParameter("@AddedBy", AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetOrderRefundDetails", para);
            return ds;
        }

        public DataSet EmployeeOrderRefund()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@BillNo",BillNo),
                new SqlParameter("@PieceName",PieceName),
                 new SqlParameter("@AvailableNoOfPiece",AvailableNoOfPiece),
                  new SqlParameter("@NoOfPiece",NoOfPiece),
                new SqlParameter("@Mobile",ContactNo),
                new SqlParameter("@Amount",Balance),
                new SqlParameter("@RefundDate",RefundDate),
                new SqlParameter("@AddedBy",AddedBy),
            };
            DataSet ds = DBHelper.ExecuteQuery("OrderRefund", para);
            return ds;
        }

        public DataSet PrintEmployeeOrderRefundBill()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_BillId",RefundId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPrintOrderRefund", para);
            return ds;
        }
    }
}