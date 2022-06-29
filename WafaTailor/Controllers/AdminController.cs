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
                
            }
            return View(model);
        }

        public ActionResult AdminProfile(Admin model)
        {
            DataSet ds = model.GetAdminProfileDetails();
            if (ds != null && ds.Tables.Count > 0)
            {
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                ViewBag.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();

                


            }
            return View(model);
        }


    }
}