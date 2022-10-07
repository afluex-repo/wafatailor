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
    public class ShopController : ShopBaseController
    {
        // GET: Shop
        public ActionResult ShopDashBoard()
        {
            return View();
        }
        public ActionResult ShopSaleOrder(Shop obj)
        {
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds1 = obj.GetCustomerDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlcustomer.Add(new SelectListItem { Text = "Select Customer", Value = "0" });
                    }
                    ddlcustomer.Add(new SelectListItem { Text = r["CustomerName"].ToString(), Value = r["PK_UserId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlcustomer = ddlcustomer;
            #endregion
            return View(obj);
        }
        [HttpPost]
        public JsonResult SaveSaleOrder(Shop order, string dataValue)
        {

            try
            {
                //order.SaleOrderDate = string.IsNullOrEmpty(order.SaleOrderDate) ? null : Common.ConvertToSystemDate(order.SaleOrderDate, "dd/MM/yyyy");
                string Name = "";
                string Piece = "";
                string OriginalPrice = "";
                string Discount = "";
                string FinalPrice = "";
                string SaleDate = "";
                string Description = "";
                //int rowsno = 0;
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtorder = new DataTable();
                dtorder.Columns.Add("Name");
                dtorder.Columns.Add("Piece");
                dtorder.Columns.Add("OriginalPrice");
                dtorder.Columns.Add("Discount");
                dtorder.Columns.Add("FinalPrice");
                dtorder.Columns.Add("SaleDate");
                dtorder.Columns.Add("Description");
                //dtorder.Columns.Add("rowsno");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;
                //foreach (DataRow row in dt.Rows)

                foreach (DataRow row in dt.Rows)
                {
                    //Name = row["Name"].ToString();
                    Name = "";
                    Piece = row["Piece"].ToString();
                    OriginalPrice = row["OriginalPrice"].ToString();
                    Discount = row["Discount"].ToString();
                    FinalPrice = row["NetAmount"].ToString();
                    //SaleDate = row["SaleDate"].ToString();
                  SaleDate = string.IsNullOrEmpty(row["SaleDate"].ToString()) ? null : Common.ConvertToSystemDate(row["SaleDate"].ToString(), "dd/MM/yyyy");
                    Description = row["Description"].ToString();
                    //
                    //rowsno = rowsno + 1;
                    dtorder.Rows.Add(Name, Piece, OriginalPrice, Discount, FinalPrice, SaleDate,Description);
                }
                order.dt = dtorder;
               
                order.AddedBy = Session["Pk_userId"].ToString();
                DataSet ds = order.SaveSaleOrder();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        order.Result = "Yes";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {

                order.Result = ex.Message;
            }

            return new JsonResult { Data = new { status = order.Result } };
        }
        public ActionResult ShopSaleOrderList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            model.AddedBy = Session["Pk_userId"].ToString();
            model.ShopLoginId = Session["LoginId"].ToString();
            DataSet ds = model.GetShopSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
                    obj.SaleOrderId = r["Fk_SaleOrderId"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.SalesOrderNo = r["SalesOrderNo"].ToString();
                    obj.customerName = r["customerName"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    lst.Add(obj);
                }
                model.lstShopRegistration = lst;
            }
            return View(model);
        }
        public ActionResult PrintShopSaleOrder(String SaleOrderId)
        {
            List<Shop> lstShopSaleOrderDetails = new List<Shop>();
            Shop model = new Shop();
            model.SaleOrderId = SaleOrderId;
            DataSet ds = model.PrintShopSO();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    model.SaleDate = ds.Tables[1].Rows[0]["SaleDate"].ToString();
                    model.PieceName = ds.Tables[1].Rows[0]["PieceName"].ToString();
                    model.NoOfPiece = ds.Tables[1].Rows[0]["NoOfPiece"].ToString();
                    model.OriginalPrice = ds.Tables[1].Rows[0]["OriginalPrice"].ToString();
                    model.Discount = ds.Tables[1].Rows[0]["Discount"].ToString();
                    model.FinalPrice = ds.Tables[1].Rows[0]["FinalPrice"].ToString();
                    lstShopSaleOrderDetails.Add(model);
                }
                model.lstshopsaleorder = lstShopSaleOrderDetails;
               ViewBag.FinalPrice = double.Parse(ds.Tables[1].Compute("sum(FinalPrice)", "").ToString()).ToString("n2");
            }

            return View(model);
        }
        //public ActionResult DeleteShopSaleOrder(String SaleOrderId)
        //{
        //    Shop obj = new Shop();
        //    try
        //    {
        //        obj.SaleOrderId = SaleOrderId;
        //        DataSet ds = obj.DeleteShopSaleOrder();
        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {
        //                TempData["ShopSaleOrder"] = "Shop Sale Order Deleted Successfully!";
        //            }
        //            else
        //            {
        //                TempData["ShopSaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ShopSaleOrder"] = ex.Message;
        //    }
        //    return RedirectToAction("ShopSaleOrderList", "Shop");
        //}
        public ActionResult GetcustomerList()
        {
            Shop obj = new Shop();
            List<Shop> lst = new List<Shop>();
            DataSet ds = obj.GetCustomerDetail();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Shop objList = new Shop();
                    objList.Name = dr["CustomerName"].ToString();
                    objList.Mobile = dr["Mobile"].ToString();
                    objList.LoginId = dr["LoginId"].ToString();
                    lst.Add(objList);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BillEntry(Shop model, string BillId, string PaymentId)
        {
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds1 = model.GetCustomerDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlcustomer.Add(new SelectListItem { Text = "Select Customer", Value = "0" });
                    }
                    ddlcustomer.Add(new SelectListItem { Text = r["CustomerName"].ToString(), Value = r["PK_UserId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlcustomer = ddlcustomer;
            #endregion

            if (BillId != null && PaymentId != null)
            {
                model.BillId = BillId;
                model.Pk_BillPaymentId = PaymentId;
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
               
                DataSet ds = model.GetBillDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.ShopId = ds.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                    model.LoginId = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    model.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                    model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                    model.DeliveredPiece = ds.Tables[0].Rows[0]["DeliveredPiece"].ToString();
                    model.RemainingPiece = ds.Tables[0].Rows[0]["RemainingPiece"].ToString();
                    model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                    model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                    model.Advance = ds.Tables[0].Rows[0]["AdavanceAmount"].ToString();
                    model.RemainningBalance = ds.Tables[0].Rows[0]["RemainingBalance"].ToString();
                    model.BillDate = ds.Tables[0].Rows[0]["BillDate"].ToString();
                    model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                }
            }

            List<SelectListItem> Status = Common.BindStatus();
            ViewBag.BindStatus = Status;
            return View(model);
        }
        [HttpPost]
        [ActionName("BillEntry")]
        [OnAction(ButtonName = "Save")]
        public ActionResult BillEntryAction(Shop model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                DataSet ds = model.SaveBillEntry();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BillEntry"] = "Bill Entry saved Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["BillEntry"] = ex.Message;
            }
            return RedirectToAction("BillEntry", "Shop");
        }
        public ActionResult BillList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            model.AddedBy = Session["Pk_userId"].ToString();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetBillDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
                    obj.BillId = r["Pk_BillId"].ToString();
                    obj.Pk_BillPaymentId = r["Pk_BillPaymentId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.NoOfPiece = r["NoOfPiece"].ToString();
                    obj.OriginalPrice = r["OriginalPrice"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.BillDate = r["BillDate"].ToString();
                    obj.Balance = Convert.ToDecimal(r["RemainingBalance"].ToString());
                    lst.Add(obj);
                }
                model.lstList = lst;
            }
            return View(model);
        }
        public ActionResult PrintBill(string BillId, string PaymentId)
        {
            List<Shop> lstbill = new List<Shop>();
            Shop model = new Shop();
            model.BillId = BillId;
            model.Pk_BillPaymentId = PaymentId;
            DataSet ds = model.PrintBill();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                //ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                //ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();

                model.BillDate = ds.Tables[0].Rows[0]["BillDate"].ToString();
                model.Advance = ds.Tables[0].Rows[0]["AdavanceAmount"].ToString();
                model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                model.Discount = ds.Tables[0].Rows[0]["Discount"].ToString();
                model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                lstbill.Add(model);
            }
            model.lstList = lstbill;

            return View(model);
        }
        public ActionResult ShopChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShopChangePassword(Shop model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                DataSet ds = model.ShopChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["ChangePassword"] = "Password Changed Successfully!";
                    }
                    else
                    {
                        TempData["ChangePassword"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ChangePassword"] = ex.Message;
            }
            return RedirectToAction("ShopChangePassword", "Shop");
        }
        [HttpPost]
        [ActionName("BillEntry")]
        [OnAction(ButtonName = "UpdateBill")]
        public ActionResult UpdateBillEntry(Admin model, string BillId, string Pk_BillPaymentId)
        {
            try
            {
                if (BillId != null && Pk_BillPaymentId != null)
                {
                    model.BillId = BillId;
                    model.Pk_BillPaymentId = Pk_BillPaymentId;
                    model.ShopId = Session["Pk_userId"].ToString();
                    model.AddedBy = Session["Pk_userId"].ToString();
                    DataSet ds = model.UpdateBillEntry();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            TempData["BillEntry"] = "Bill Details Updated Successfully !!";
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }
                    }
                    else
                    {
                        TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BillEntry"] = ex.Message;
            }
            return RedirectToAction("BillEntry", "Shop");
        }
        public ActionResult ShopExpense()
        {
            Employee obj = new Employee();
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
        public JsonResult ActionShopExpense(Shop model, string dataValue)
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
                model.AddedBy = Session["Pk_userId"].ToString();
                DataSet ds = new DataSet();
                ds = model.SaveShopExpense();
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
        public ActionResult ShopExpenseList()
        {
            Shop model = new Shop();
            List<Shop> lst = new List<Shop>();
            model.AddedBy = Session["Pk_userId"].ToString();
            DataSet ds = model.GetShopExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
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
        [ActionName("ShopExpenseList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult ExpenseList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            model.Expensetype = model.Expensetype == "0" ? null : model.Expensetype;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet ds = model.GetShopExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
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

        public ActionResult BillPayment(string BillId, string PaymentId)
        {
            Shop model = new Shop();
            model.BillId = BillId;
            model.Pk_BillPaymentId = PaymentId;
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

            List<SelectListItem> ItemStatus = Common.BindStatus();
            ViewBag.ItemStatus = ItemStatus;

            DataSet ds = model.GetBillDetail();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.ShopId = ds.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                model.BillId = ds.Tables[0].Rows[0]["Pk_BillId"].ToString();
                model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                model.RemainningBalance = ds.Tables[0].Rows[0]["RemainingBalance"].ToString();
                //model.Balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["RemainingBalance"].ToString());
                model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                model.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                //model.BillDate = ds.Tables[0].Rows[0]["BillDate"].ToString();
                model.RemainingPiece = ds.Tables[0].Rows[0]["RemainingPiece"].ToString();
                model.TotalDeliveredPiece = ds.Tables[0].Rows[0]["TotalDeliveredPiece"].ToString();
                model.LoginId = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.TotalPaid = ds.Tables[0].Rows[0]["TotalPaid"].ToString();
                model.Fk_UserId = ds.Tables[0].Rows[0]["Fk_UserId"].ToString();
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("BillPayment")]
        [OnAction(ButtonName = "btnbill")]
        public ActionResult BillPayment(Shop model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                DataSet ds = model.BillPayment();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BillEntry"] = "Payment Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["BillEntry"] = ex.Message;
            }
            return RedirectToAction("BillPayment", "Shop");
        }
        public ActionResult GetUserDetails(string LoginId, string Mobile)
        {
            SaleOrder model = new SaleOrder();
            model.LoginId = LoginId;
            model.Mobile = Mobile;
            DataSet ds = model.GetUserDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    model.Result = "yes";
                    model.Fk_UserId = ds.Tables[0].Rows[0]["Pk_UserId"].ToString();
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShopOrderRefund()
        {
            return View();
        }
        [HttpPost]
        [ActionName("ShopOrderRefund")]
        [OnAction(ButtonName = "Save")]
        public ActionResult ActionOrderRefund(Shop model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                model.RefundDate = string.IsNullOrEmpty(model.RefundDate) ? null : Common.ConvertToSystemDate(model.RefundDate, "dd/MM/yyyy");
                DataSet ds = model.OrderRefund();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["msg"].ToString() == "1")
                    {
                        TempData["Order"] = "Order Refund saved Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0]["ErrorMessage"].ToString() == "0")
                    {
                        TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Order"] = ex.Message;
            }
            return RedirectToAction("ShopOrderRefund", "Shop");
        }
        public ActionResult ShopOrderRefundList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            DataSet ds = model.GetOrderRefundDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
                    obj.RefundId = r["Pk_RefundId"].ToString();
                    //obj.PieceName = r["PieceName"].ToString();
                    obj.NoOfPiece = r["RefundPiece"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.Balance = Convert.ToDecimal(r["Amount"].ToString());
                    obj.RefundDate = r["RefundDate"].ToString();
                    lst.Add(obj);
                }
                model.lstList = lst;
            }
            return View(model);
        }

        //public ActionResult GetAvailableBill(string BillNo)
        //{
        //    Shop obj = new Shop();
        //    try
        //    {
        //        obj.BillNo = BillNo;
        //        DataSet ds = obj.GetBill();
        //        if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
        //            {
        //                obj.NoOfPiece = ds.Tables[0].Rows[0]["AvailablePiece"].ToString();
        //                obj.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
        //                obj.Result = "yes";
        //            }
        //            else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
        //            {
        //                obj.NoOfPiece = "0";
        //                obj.Mobile = "";
        //                obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //        else
        //        {
        //            obj.NoOfPiece = "0";
        //            obj.Mobile = "";
        //            obj.Result = "no";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obj.Result = ex.Message;
        //    }
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult PrintShopOrderRefund(string RefundId)
        {
            Shop model = new Shop();
            model.RefundId = RefundId;
            DataSet ds = model.PrintOrderRefundBill();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                //model.PieceName = ds.Tables[0].Rows[0]["PieceName"].ToString();
                model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.Balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"].ToString());
                model.NoOfPiece = ds.Tables[0].Rows[0]["ReturnPiece"].ToString();
            }
            return View(model);
        }

        public ActionResult GetAvailableBill(string BillNo)
        {
            Shop obj = new Shop();
            try
            {
                obj.BillNo = BillNo;
                DataSet ds = obj.GetBill();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["msg"].ToString() == "1")
                    {
                        obj.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                        obj.NoOfPiece = ds.Tables[0].Rows[0]["TotalDeliveredPiece"].ToString();
                        //obj.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                        obj.Result = "yes";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "0")
                    {
                        obj.NoOfPiece = "0";
                        obj.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    obj.NoOfPiece = "0";
                    //obj.Result = "no";
                    obj.Result = "Invalid Bill Number Please Enter Correct Bill Number !!";
                }
            }
            catch (Exception ex)
            {
                obj.Result = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefundShopSaleOrder()
        {
            return View();

        }

        [HttpPost]
        [ActionName("RefundShopSaleOrder")]
        [OnAction(ButtonName = "Save")]
        public ActionResult ActionRefundShopSaleOrder(Shop model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                model.RefundDate = string.IsNullOrEmpty(model.RefundDate) ? null : Common.ConvertToSystemDate(model.RefundDate, "dd/MM/yyyy");
                DataSet ds = model.RefundShopSaleOrder();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["msg"].ToString() == "1")
                    {
                        TempData["Order"] = "Order Refund saved Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0]["ErrorMessage"].ToString() == "0")
                    {
                        TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Order"] = ex.Message;
            }
            return RedirectToAction("RefundShopSaleOrder", "Shop");
        }
        public ActionResult RefundShopSaleOrderList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            model.AddedBy = Session["Pk_userId"].ToString();
            model.ShopLoginId = Session["LoginId"].ToString();
            DataSet ds = model.GetRefundShopSaleOrderList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
                    obj.RefundId = r["PK_SaleOrderRefundId"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.NoOfPiece = r["RefundPiece"].ToString();
                    //obj.Mobile = r["Mobile"].ToString();
                    //obj.BillNo = r["BillNo"].ToString();
                    obj.Balance = Convert.ToDecimal(r["RefundAmount"].ToString());
                    obj.RefundDate = r["RefundDate"].ToString();
                    lst.Add(obj);
                }
                model.lstList = lst;
            }
            return View(model);
        }

        public ActionResult PrintRefundShopSaleOrder(string RefundId)
        {
            Shop model = new Shop();
            model.RefundId = RefundId;
            DataSet ds = model.PrintSaleOrderRefundBill();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                model.Balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["RefundAmount"].ToString());
                model.NoOfPiece = ds.Tables[0].Rows[0]["RefundPiece"].ToString();
                model.RefundDate = ds.Tables[0].Rows[0]["RefundDate"].ToString();
            }
            return View(model);
        }
    }
}