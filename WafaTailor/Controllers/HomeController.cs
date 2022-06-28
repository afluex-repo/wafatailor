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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sign_up()
        {
            return View();
        }

        public ActionResult Login()
        {
            Session.Abandon();
            //if (TempData["Login"] == null)
            //{
            //    ViewBag.errormsg = "none";
            //}
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [OnAction(ButtonName = "btnlogin")]
        public ActionResult Login(Home obj)
        {
            //if (obj.LoginId == null)
            //{
            //    ViewBag.errormsg = "";
            //    TempData["Login"] = "Please Enter LoginId";
            //    return RedirectToAction("Login");

            //}
            //if (obj.Password == null)
            //{
            //    ViewBag.errormsg = "";
            //    TempData["Login"] = "Please Enter Password";
            //    return RedirectToAction("Login");
            //}
            try
            {
                DataSet ds = obj.Login();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1" && ds.Tables[0].Rows[0]["Fk_UserTypeId"].ToString() == "1")
                    //{
                    //    ViewBag.errormsg = "";
                    //    Session["AdminID"] = ds.Tables[0].Rows[0]["Pk_Id"].ToString();
                    //    Session["AdminLoginID"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                    //    Session["AdminName"] = ds.Tables[0].Rows[0]["Name"].ToString();

                    //    return RedirectToAction("AdminDashboard", "Admin");

                    //}
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1" && ds.Tables[0].Rows[0]["UserType"].ToString() == "Employee")
                    {
                        ViewBag.errormsg = "";
                        Session["Pk_EmployeeId"] = ds.Tables[0].Rows[0]["Pk_EmployeeId"].ToString();
                        Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        Session["Password"] = ds.Tables[0].Rows[0]["Password"].ToString();
                        Session["EmployeeName"] = ds.Tables[0].Rows[0]["Name"].ToString();

                        return RedirectToAction("EmployeeDashBoard", "Employee");

                    }
                    else
                    {
                        ViewBag.errormsg = "";
                        TempData["Login"] = "Incorrect LoginId Or Password";
                        return RedirectToAction("Login");

                    }
                }
                else
                {
                    ViewBag.errormsg = "";
                    TempData["Login"] = "Incorrect LoginId Or Password";
                    return RedirectToAction("Login","Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = "";
                TempData["Login"] = ex.Message;
                return RedirectToAction("Login", "Home");

            }

        }
    }
}