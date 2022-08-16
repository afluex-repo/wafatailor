using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WafaTailor.Filter;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class ExpenseController : AdminBaseController
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


        public ActionResult ExpenseList()
        {
            Expense model = new Expense();
            List<Expense> lst = new List<Expense>();
            DataSet ds = model.GetExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.Pk_ExpenseId = r["Pk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.OtherExpenseName = r["OtherExpenseName"].ToString();
                    obj.Expenses = r["Expense"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.ExpenseDate = r["ExpenseDate"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds1 = model.GetExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
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
            return View(model);
        }

        [HttpPost]
        [ActionName("ExpenseList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult ExpenseList(Expense model)
        {
            List<Expense> lst = new List<Expense>();
            model.Expensetype = model.Expensetype == "0" ? null : model.Expensetype;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.Pk_ExpenseId = r["Pk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.OtherExpenseName = r["OtherExpenseName"].ToString();
                    obj.Expenses = r["Expense"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.ExpenseDate = r["ExpenseDate"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds1 = model.GetExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
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
            return View(model);
        }
        public ActionResult DeleteExpense(string Id)
        {
            Expense obj = new Expense();
            try
            {
                obj.Pk_ExpenseId = Id;
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = obj.DeleteExpense();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        obj.Result = "yes";
                        TempData["Expense"] = "Expense details deleted successfully!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                    else
                    {
                        obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                obj.Result = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}