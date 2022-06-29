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
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [OnAction(ButtonName = "btnlogin")]
        public ActionResult Login(Home obj)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                DataSet ds = obj.Login();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1" && ds.Tables[0].Rows[0]["UserType"].ToString() == "Admin")
                    {
                        Session["Pk_EmployeeId"] = ds.Tables[0].Rows[0]["Pk_EmployeeId"].ToString();
                        Session["UsertypeName"] = ds.Tables[0].Rows[0]["UsertypeName"].ToString();
                        Session["Name"] = ds.Tables[0].Rows[0]["Name"].ToString();
                        Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        FormName = "AdminDashBoard";
                        Controller = "Admin";
                    }
                    else if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1" && ds.Tables[0].Rows[0]["UserType"].ToString() == "Customer")
                    {
                        if (ds.Tables[0].Rows[0]["Password"].ToString() == obj.Password)
                        {
                            Session["Pk_CustomerId"] = ds.Tables[0].Rows[0]["Pk_CustomerId"].ToString();
                            Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                            Session["Password"] = ds.Tables[0].Rows[0]["Password"].ToString();
                            Session["CustomerName"] = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            Session["ProfilePic"] = ds.Tables[0].Rows[0]["ProfilePic"].ToString();

                            FormName = "CustomerDashBoard";
                            Controller = "Customer";
                        }
                        else
                        {
                            TempData["Login"] = "Incorrect LoginId Or Password";
                            FormName = "Login";
                            Controller = "Home";
                        }
                    }
                    else
                    {
                        TempData["Login"] = "Incorrect LoginId Or Password";
                        FormName = "Login";
                        Controller = "Home";
                    }
                }
                else
                {
                    TempData["Login"] = "Incorrect LoginId Or Password";
                    FormName = "Login";
                    Controller = "Home";
                }
            }
            catch (Exception ex)
            {
                TempData["Login"] = ex.Message;
                FormName = "Login";
                Controller = "Home";

            }
            return RedirectToAction(FormName, Controller);

        }
    }
}