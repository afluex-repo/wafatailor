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

            #region Vendor
            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            DataSet ds2 = obj.GetVendor();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendor.Add(new SelectListItem { Text = "", Value = "" });
                    }
                    ddlVendor.Add(new SelectListItem { Text = r["MaterialType"].ToString(), Value = r["Pk_MaterialId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlVendor = ddlVendor;
            #endregion
            return View();
        }


        [HttpPost]
        public JsonResult ActionExpense(Expense model, string dataValue)
        {
            try
            {
                model.ExpenseDate = string.IsNullOrEmpty(model.ExpenseDate) ? null : Common.ConvertToSystemDate(model.ExpenseDate, "dd/MM/yyyy");
                string Expensetype = "";
                string ExpenseRupee = "";
                string ExpenseDate = "";
                string Remark = "";
                string OtherExpensetype = "";
                string Fk_Vendorid = "";
                var isValidModel = TryUpdateModel(model);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtmodel = new DataTable();
                dtmodel.Columns.Add("Expensetype");
                dtmodel.Columns.Add("OtherExpensetype");
                dtmodel.Columns.Add("ExpenseDate");
                dtmodel.Columns.Add("ExpenseRupee");
                dtmodel.Columns.Add("Remark");
                dtmodel.Columns.Add("Fk_Vendorid");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;

                foreach (DataRow row in dt.Rows)
                {
                    Expensetype = row["Expensetype"].ToString();
                    OtherExpensetype = row["OtherExpensetype"].ToString();
                    model.OtherExpensetype = OtherExpensetype == "0" ? null : OtherExpensetype;
                    //ExpenseDate = row["ExpenseDate"].ToString();
                    ExpenseDate = string.IsNullOrEmpty(row["ExpenseDate"].ToString()) ? null : Common.ConvertToSystemDate(row["ExpenseDate"].ToString(), "dd/MM/yyyy");
                    ExpenseRupee = row["ExpenseRupee"].ToString();
                    Remark = row["Remark"].ToString();
                    Fk_Vendorid = row["Fk_Vendorid"].ToString();
                    //rowsno = rowsno + 1;
                    dtmodel.Rows.Add(Expensetype, OtherExpensetype, ExpenseDate, ExpenseRupee, Remark, Fk_Vendorid);
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
                    //obj.Vendor = r["Vendor"].ToString();
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
                    //obj.Vendor = r["Vendor"].ToString();
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

        public ActionResult OtherExpense()
        {
            return View();
        }

        [HttpPost]
        [ActionName("OtherExpense")]
        [OnAction(ButtonName = "btnSave")]
        public ActionResult ActionOtherExpense(Expense obj)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = obj.SaveOtherExpense();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Expense"] = "New Other Expense add successfully!";
                        FormName = "OtherExpense";
                        Controller = "Expense";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "OtherExpense";
                        Controller = "Expense";
                    }
                    else
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "OtherExpense";
                        Controller = "Expense";
                    }
                }
                else
                {
                    TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "OtherExpense";
                    Controller = "Expense";
                }
            }
            catch (Exception ex)
            {
                TempData["Expense"] = ex.Message;
            }
            return RedirectToAction(FormName, Controller);
        }

        public ActionResult OtherExpenseList()
        {
            Expense model = new Expense();
            List<Expense> lst = new List<Expense>();
            DataSet ds = model.GetOtherExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.OtherExpenseId = r["Pk_OtherExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            return View(model);
        }

        public ActionResult DeleteOtherExpense(string OtherExpenseId)
        {
            Expense obj = new Expense();

            string FormName = "";
            string Controller = "";
            try
            {
                obj.OtherExpenseId = OtherExpenseId;
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = obj.DeleteOtherExpense();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Expense"] = "Other Expense Type Deleted Successfully!";
                        FormName = "OtherExpenseList";
                        Controller = "Expense";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "OtherExpenseList";
                        Controller = "Expense";
                    }
                    else
                    {
                        TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "OtherExpenseList";
                        Controller = "Expense";
                    }
                }
                else
                {
                    TempData["Expense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "OtherExpenseList";
                    Controller = "Expense";
                }
            }
            catch (Exception ex)
            {
                TempData["Expense"] = ex.Message;
            }
            return RedirectToAction(FormName, Controller);
        }

        public ActionResult DailyExpenseReport(Expense model)
        {
            #region Shop
            List<SelectListItem> ddlShop = new List<SelectListItem>();
            DataSet ds1 = model.GetShopNameDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlShop.Add(new SelectListItem { Text = "Select Shop", Value = "0" });
                    }
                    ddlShop.Add(new SelectListItem { Text = r["ShopName"].ToString(), Value = r["Pk_ShopId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlShop = ddlShop;
            #endregion

            List<Expense> lst = new List<Expense>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetDailyExpenseReport();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.Pk_ExpenseId = r["Id"].ToString();
                    obj.Delivery = r["Delivery"].ToString();
                    obj.ExpenseDate = r["ExpenseDate"].ToString();
                    obj.Crystal = r["Crystal"].ToString();
                    obj.Worker = r["Worker"].ToString();
                    obj.Material = r["Material"].ToString();
                    obj.Other = r["Other"].ToString();
                    obj.Profit = r["Profit"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
                ViewBag.Delivery = double.Parse(ds.Tables[0].Compute("sum(Delivery)", "").ToString()).ToString("n2");
                ViewBag.Crystal = double.Parse(ds.Tables[0].Compute("sum(Crystal)", "").ToString()).ToString("n2");
                ViewBag.Worker = double.Parse(ds.Tables[0].Compute("sum(Worker)", "").ToString()).ToString("n2");
                ViewBag.Material = double.Parse(ds.Tables[0].Compute("sum(Material)", "").ToString()).ToString("n2");
                ViewBag.Other = double.Parse(ds.Tables[0].Compute("sum(Other)", "").ToString()).ToString("n2");
                ViewBag.Profit = double.Parse(ds.Tables[0].Compute("sum(Profit)", "").ToString()).ToString("n2");

            }
            return View(model);
        }

        public ActionResult DeliveryExpense()
        {
            return View();
        }


        [HttpPost]
        [ActionName("DeliveryExpense")]
        [OnAction(ButtonName ="save")]
        public ActionResult DeliveryExpenseAction(Expense model)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                model.Date = string.IsNullOrEmpty(model.Date) ? null : Common.ConvertToSystemDate(model.Date, "dd/MM/yyyy");
                DataSet ds = model.SaveDeliveryExpense();
                if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count >0)
                {
                    if(ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["DeliveryExpense"] = "Delivery Expense Save Successfully !!";
                        FormName = "DeliveryExpense";
                        Controller = "Expense";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "DeliveryExpense";
                        Controller = "Expense";
                    }
                    else
                    {
                        TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "DeliveryExpense";
                        Controller = "Expense";
                    }
                }
                else
                {
                    TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "DeliveryExpense";
                    Controller = "Expense";
                }
            }
            catch(Exception ex)
            {
                TempData["DeliveryExpense"] = ex.Message;
                FormName = "DeliveryExpense";
                Controller = "Expense";
            }
            return RedirectToAction(FormName, Controller);
        }

        public ActionResult DeliveryList(Expense model)
        {
            List<Expense> lst = new List<Expense>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetDeliveryDetalis();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.DeliveryId = r["Pk_DeliveryId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Date = r["Date"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            return View(model);
        }

        public ActionResult DeleteDeliveryExpense(string DeliveryId)
        {
            Expense obj = new Expense();
            string FormName = "";
            string Controller = "";
            try
            {
                obj.DeliveryId = DeliveryId;
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = obj.DeleteDeliveryExpense();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["DeliveryExpense"] = "Delivery Expense Details Deleted Successfully!";
                        FormName = "DeliveryList";
                        Controller = "Expense";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "OtherExpenseList";
                        Controller = "Expense";
                    }
                    else
                    {
                        TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "DeliveryList";
                        Controller = "Expense";
                    }
                }
                else
                {
                    TempData["DeliveryExpense"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "DeliveryList";
                    Controller = "Expense";
                }
            }
            catch (Exception ex)
            {
                TempData["DeliveryExpense"] = ex.Message;
            }
            return RedirectToAction(FormName, Controller);
        }
    }
}