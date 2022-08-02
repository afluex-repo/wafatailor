using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WafaTailor.Filter;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminDashBoard(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetAdminDashBoardDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                ViewBag.TotalEmployee = ds.Tables[0].Rows[0]["TotalEmployee"].ToString();
                ViewBag.TotalCustomer = ds.Tables[1].Rows[0]["TotalCustomer"].ToString();
                ViewBag.TotalVendor = ds.Tables[2].Rows[0]["TotalVendor"].ToString();
                ViewBag.TotalSaleOrder = ds.Tables[3].Rows[0]["TotalSaleOrder"].ToString();
            }
            if (ds != null && ds.Tables[4].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[4].Rows)
                {
                    Admin obj = new Admin();
                    obj.FirstName = dr["FirstName"].ToString();
                    obj.LastName = dr["LastName"].ToString();
                    obj.SalesOrderNo = dr["SalesOrderNo"].ToString();
                    obj.BillNo = dr["BillNo"].ToString();
                    obj.SaleOrderDate = dr["AddedOn"].ToString();
                    lst.Add(obj);
                }
                model.lstsaleorder = lst;
            }
            return View(model);
        }

        public ActionResult AdminProfile(Admin model)
        {
            model.EmployeeId = Session["Pk_EmployeeId"].ToString();
            DataSet ds = model.GetAdminProfileDetails();
            if (ds != null && ds.Tables.Count > 0)
            {
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                ViewBag.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                ViewBag.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                ViewBag.ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewBag.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                ViewBag.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
            }
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = model.ChangePassword();
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
            return RedirectToAction("ChangePassword", "Admin");
        }


        public ActionResult VendorListForAdmin()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetVendorList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.FK_UserId = r["PK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Password = Crypto.Decrypt(r["Password"].ToString());
                    obj.Name = r["Name"].ToString();
                    obj.Address = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    lst.Add(obj);
                }
                model.lstVendor = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("VendorListForAdmin")]
        public ActionResult VendorListForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetVendorList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.FK_UserId = r["PK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Password =Crypto.Decrypt(r["Password"].ToString());
                    obj.Name = r["Name"].ToString();
                    obj.Address = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    lst.Add(obj);
                }
                model.lstVendor = lst;
            }
            return View(model);
        }

        public ActionResult BillEntry(Admin model)
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
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds = model.GetCustomerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
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
            return View(model);
        }

        [HttpPost]
        [ActionName("BillEntry")]
        [OnAction(ButtonName = "SaveBill")]
        public ActionResult BillEntryAction(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = new DataSet();
                ds = model.SaveBillEntry();
                if (ds != null  && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            catch(Exception ex)
            {
                TempData["BillEntry"] = ex.Message;
            }
            return RedirectToAction("BillEntry", "Admin");
        }

        public ActionResult BillList(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetBillDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
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
            List<Admin> lstbill = new List<Admin>();
            Admin model = new Admin();
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
        public ActionResult BillPayment(string BillId , string PaymentId)
        {
            Admin model = new Admin();
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
            DataSet ds = model.GetBillDetails();
            if (ds !=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                model.ShopId = ds.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                model.BillId = ds.Tables[0].Rows[0]["Pk_BillId"].ToString();
                model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                model.RemainningBalance = ds.Tables[0].Rows[0]["RemainingBalance"].ToString();
                model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                model.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                //model.BillDate = ds.Tables[0].Rows[0]["BillDate"].ToString();
                model.LoginId = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.TotalPaid= ds.Tables[0].Rows[0]["TotalPaid"].ToString();
                model.FK_UserId = ds.Tables[0].Rows[0]["Fk_UserId"].ToString();
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("BillPayment")]
        [OnAction(ButtonName = "btnbill")]
        public ActionResult BillPayment(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = new DataSet();
                ds = model.BillPayment();
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
            return RedirectToAction("BillPayment", "Admin");
        }
    }
}