using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Expense
    {
        public string Expensetype { get; set; }
        public string ExpenseDate { get; set; }
        public string Remark { get; set; }
        public string ExpenseRupee { get; set; }
        public string OtherExpensetype { get; set; }
        public DataTable dt { get; set; }
        public string AddedBy { get; set; }
        public string Result { get; set; }
        public string ExpenseId { get; set; }

        public List<Expense> lstexpense { get; set; }
        public string OtherExpense { get; set; }
        public string OtherExpenseId { get; set; }



        public string Pk_ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public string Expenses { get; set; }
        public string OtherExpenseName { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public string Delivery { get; set; }
        public string Crystal { get; set; }
        public string Worker { get; set; }
        public string Material { get; set; }
        public string Other { get; set; }
        public string Profit { get; set; }


        
        
        
        
        
         
         

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

        public DataSet SaveExpense()
        {
            SqlParameter[] para ={
                new SqlParameter("@AddedBy",AddedBy),
                new SqlParameter("@dt",dt)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpenseDetails", para);
            return ds;
        }

        public DataSet DeleteExpense()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_ExpenseId",Pk_ExpenseId),
                 new SqlParameter("@DeletedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteExpense", para);
            return ds;
        }


        public DataSet GetExpenseList()
        {
            SqlParameter[] para ={
                new SqlParameter("@Fk_ExpensetypeId",Expensetype),
                new SqlParameter("@FromDate",FromDate),
                new SqlParameter("@ToDate",ToDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseList", para);
            return ds;
        }

        public DataSet SaveOtherExpense()
        {
            SqlParameter[] para ={

                new SqlParameter("@OtherExpense",OtherExpense),
                new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveOtherExpenseDetails", para);
            return ds;
        }

        public DataSet GetOtherExpenseList()
        {
            SqlParameter[] para ={
                new SqlParameter("@OtherExpensetypeId",OtherExpense)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetOtherExpenseList", para);
            return ds;
        }

        public DataSet DeleteOtherExpense()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@OtherExpensetypeId",OtherExpenseId),
                 new SqlParameter("@DeletedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteOtherExpense", para);
            return ds;
        }

        public DataSet GetDailyExpenseReport()
        {
            SqlParameter[] para ={
                new SqlParameter("@FromDate",FromDate),
                new SqlParameter("@ToDate",ToDate)
            };
            DataSet ds = DBHelper.ExecuteQuery("getDailyExpenseReport", para);
            return ds;
        }
    }
}