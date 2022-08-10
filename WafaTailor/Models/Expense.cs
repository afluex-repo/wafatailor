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
                new SqlParameter("@Pk_ExpenseId",ExpenseId),
                 new SqlParameter("@DeletedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteExpense", para);
            return ds;
        }
    }
}