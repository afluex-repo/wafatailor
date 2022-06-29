using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor
        
        public ActionResult VendorDashBoard()
        {
            return View();
        }
        public ActionResult VendorRegistration()
        {
            List<SelectListItem> Gender = Common.BindGender();
            ViewBag.Gender = Gender;
            return View();
        }
        [HttpPost]
        [ActionName("VendorRegistration")]
        public ActionResult VendorRegistration(Vendor model, HttpPostedFileBase postedFile)
        {
            try
            {
                if (postedFile != null)
                {
                    model.ProfilePic = "../VendorProfilePic/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(Path.Combine(Server.MapPath(model.ProfilePic)));
                }
                model.AddedBy = Session["Pk_userId"].ToString();
                Random rnd = new Random();
                string Pass = rnd.Next(111111, 999999).ToString();
                model.Password = Pass;
                model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/MM/yyyy");
                DataSet ds = model.VendorRegistration();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Vendor"] = "Vendor Registration Successfully!";
                    }
                    else
                    {
                        TempData["Vendor"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Vendor"] = ex.Message;
            }
            return RedirectToAction("VendorRegistration", "Vendor");
        }

        public ActionResult VendorChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VendorChangePassword(Vendor model)
        {
            try
            {
                model.AddedBy = Session["Pk_userId"].ToString();
                DataSet ds = model.ChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["VendorChangePassword"] = "Password Changed Successfully!";
                    }
                    else
                    {
                        TempData["VendorChangePassword"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["VendorChangePassword"] = ex.Message;
            }
            return RedirectToAction("VendorChangePassword", "Vendor");
        }

        public ActionResult VendorProfile(Vendor model)
        {
            model.PK_UserId = Session["Pk_userId"].ToString();
            DataSet ds = model.GetVendorProfileDetails();
            if (ds != null && ds.Tables.Count > 0)
            {
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                ViewBag.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                ViewBag.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                ViewBag.ContactNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewBag.Gender = ds.Tables[0].Rows[0]["Sex"].ToString();
                ViewBag.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
            }
            return View(model);
        }

    }
}