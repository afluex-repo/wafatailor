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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CustomerDashBoard()
        {
            return View();
        }
        public ActionResult CustomerProfile(Customer model)
        {
            model.LoginId = Session["LoginId"].ToString();
            DataSet ds = model.GetCustomerProfileDetails();
            if (ds != null && ds.Tables.Count > 0)
            {
                ViewBag.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                ViewBag.EmployeeName = ds.Tables[0].Rows[0]["EmployeeName"].ToString();
                ViewBag.EmployeeAddress = ds.Tables[0].Rows[0]["EmployeeAddress"].ToString();
                ViewBag.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                ViewBag.Emailid = ds.Tables[0].Rows[0]["Emailid"].ToString();
                ViewBag.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                ViewBag.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                ViewBag.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                ViewBag.Profile = ds.Tables[0].Rows[0]["Profile"].ToString();
            }
            return View(model);
        }
        public ActionResult CustomerRegistration(String UserID)
        {
            Customer obj = new Customer();

            if (UserID != null)
            {
                obj.UserID = UserID;
                DataSet ds = obj.GetCustomerDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.UserID = ds.Tables[0].Rows[0]["PK_UserID"].ToString();
                    obj.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    obj.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    obj.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    obj.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                    obj.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                    obj.ContactNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    obj.Emailid = ds.Tables[0].Rows[0]["Email"].ToString();
                    obj.Gender = ds.Tables[0].Rows[0]["Sex"].ToString();
                }
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("CustomerRegistration")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveCustomerRegistration(Customer model)
        {
           string FormName= "";
            string Controller = "";
            try
            {
                Random RND = new Random();
                String pass = RND.Next(111111,999999).ToString();
                model.Password = Crypto.Encrypt(pass);
                //model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "mm/dd/yyyy");
                DataSet ds = model.CustomerRegistration();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        //TempData["Customer"] = "Customer Registration Saved Successfully";
                        //Session["Name"] = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        Session["CustomerLoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString(); 
                        Session["CustomerPassword"] = Crypto.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString());
                        FormName = "CustomerConfirmRegistration";
                        Controller = "Customer";
                    }
                    else
                    {
                        TempData["Customer"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "CustomerRegistration";
                        Controller = "Customer";
                    }

                }
                else
                {
                    TempData["Customer"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "CustomerRegistration";
                    Controller = "Customer";
                }

            }
            catch (Exception ex)
            {
                TempData["Customer"] = ex.Message;
            }
            return RedirectToAction(FormName,Controller);
        }
        public ActionResult CustomerConfirmRegistration()
        {
            return View();
        }
        public ActionResult CustomerRegistrationList(Customer model)
        {
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Customer> lst = new List<Customer>();
            DataSet ds = model.GetCustomerDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Customer obj = new Customer();
                    obj.UserID = r["PK_UserID"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.FirstName = r["FirstName"].ToString();
                    obj.LastName = r["LastName"].ToString();
                    obj.CustomerDetails = r["CustomerDetails"].ToString();
                    obj.Password = Crypto.Decrypt(r["Password"].ToString());
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.CustomerAddress = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    obj.Emailid = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("CustomerRegistration")]
        [OnAction(ButtonName = "update")]
        public ActionResult UpdateCustomerRegistration(Customer model)
        {
            try
            {
                if(model.UserID != null)
                {
                    Random RND = new Random();
                    String pass = RND.Next(111111, 999999).ToString();
                    model.Password = Crypto.Encrypt(pass);
                    model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/mm/yyyy");
                    DataSet ds = model.UpdateCustomerRegistration();
                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            TempData["Customer"] = "Customer Registration Updated Successfully";
                        }
                        else
                        {
                            TempData["Customer"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }

                    }
                    else
                    {
                        TempData["Customer"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Customer"] = ex.Message;
            }
            return RedirectToAction("CustomerRegistration", "Customer");
        }
        public ActionResult DeleteCustomerRegistration(string UserID)
        {
            Customer obj = new Customer();
            try
            {
                obj.UserID = UserID;
                DataSet ds = obj.DeleteCustomer();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Customer"] = "Customer Deleted Successfully!";
                    }
                    else
                    {
                        TempData["Customer"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Customer"] = ex.Message;
            }
            return RedirectToAction("CustomerRegistrationList", "Customer");
        }
        [HttpPost]
        [ActionName("CustomerRegistrationList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult CustomerRegistrationListBy(Customer model)
        {
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Customer> lst = new List<Customer>();
            DataSet ds = model.GetCustomerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Customer obj = new Customer();
                    obj.UserID = r["PK_UserID"].ToString();
                    //obj.FirstName = r["FirstName"].ToString();
                    //obj.LastName = r["LastName"].ToString();
                    obj.CustomerDetails = r["CustomerDetails"].ToString();
                    obj.Password = Crypto.Decrypt(r["Password"].ToString());
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    obj.CustomerAddress = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    obj.Emailid = r["Email"].ToString();
                    obj.Gender = r["Sex"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }

    }
}