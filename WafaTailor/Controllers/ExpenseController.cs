using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Expense()
        {
            Expense obj = new Expense();
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds = obj.GetExpenseType();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlExpensetype.Add(new SelectListItem { Text = "Expense Type", Value = "0" });
                    }
                    ddlExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlExpensetype = ddlExpensetype;
            #endregion

            #region OtherExpenseType
            List<SelectListItem> ddlOtherExpensetype = new List<SelectListItem>();
            DataSet ds1 = obj.GetOtherExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlOtherExpensetype.Add(new SelectListItem { Text = "", Value = "" });
                    }
                    ddlOtherExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["Pk_OtherExpenseId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlOtherExpensetype = ddlOtherExpensetype;
            #endregion
            return View();
        }


        [HttpPost]
        public JsonResult ActionExpense(Expense model, string dataValue)
        {
            try  
            {
                //model.ExpenseDate = string.IsNullOrEmpty(model.ExpenseDate) ? null : Common.ConvertToSystemDate(model.ExpenseDate, "dd/MM/yyyy");
                string Expensetype = "";
                string ExpenseRupee = "";
                string ExpenseDate = "";
                string Remark = "";
                string OtherExpensetype = "";
                var isValidModel = TryUpdateModel(model);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtmodel = new DataTable();
                dtmodel.Columns.Add("Expensetype");
                dtmodel.Columns.Add("OtherExpensetype");
                dtmodel.Columns.Add("ExpenseDate");
                dtmodel.Columns.Add("ExpenseRupee");
                dtmodel.Columns.Add("Remark");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;

                foreach (DataRow row in dt.Rows)
                {
                    Expensetype = row["Expensetype"].ToString();
                    OtherExpensetype = row["OtherExpensetype"].ToString();
                    model.OtherExpensetype = OtherExpensetype == "0" ? null : OtherExpensetype;

                    ExpenseDate = row["ExpenseDate"].ToString();
                    ExpenseRupee = row["ExpenseRupee"].ToString();
                    Remark = row["Remark"].ToString();
                    //rowsno = rowsno + 1;
                    dtmodel.Rows.Add(Expensetype, OtherExpensetype, ExpenseDate, ExpenseRupee, Remark);
                }
                model.dt = dtmodel;
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = new DataSet();
                ds = model.SaveExpense();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        model.Result = "Yes";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                model.Result = ex.Message;
            }
            return new JsonResult { Data = new { status = model.Result } };
        }

        public ActionResult DeleteExpense(string Pk_ExpenseId)
        {
            Expense obj = new Expense();
            try
            {
                obj.ExpenseId = Pk_ExpenseId;
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = obj.DeleteExpense();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Expense"] = "Expense Deleted Successfully!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                    else
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Expense"] = ex.Message;
            }
            return RedirectToAction("ExpenseList", "Expense");
        }

    }
}