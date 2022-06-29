using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminDashBoard(Admin model)
        {
            DataSet ds = model.GetAdminDashBoardDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                ViewBag.TotalEmployee = ds.Tables[0].Rows[0]["TotalEmployee"].ToString();
                ViewBag.TotalCustomer = ds.Tables[1].Rows[0]["TotalCustomer"].ToString();
                ViewBag.TotalVendor = ds.Tables[2].Rows[0]["TotalVendor"].ToString();
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
                    obj.Password = r["Password"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Address = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
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
            DataSet ds = model.GetVendorList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.FK_UserId = r["PK_UserId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Password = r["Password"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.Address = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    obj.Email = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
                    lst.Add(obj);
                }
                model.lstVendor = lst;
            }
            return View(model);
        }

    }
}