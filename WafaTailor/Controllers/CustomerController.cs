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



        public ActionResult CustomerRegistration(String CustomerId)
        {
            Customer obj = new Customer();

            if (CustomerId != null)
            {
                obj.CustomerId = CustomerId;
                DataSet ds = obj.GetCustomerDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.CustomerId = ds.Tables[0].Rows[0]["Pk_CustomerId"].ToString();
                    obj.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    obj.CustomerAddress = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                    obj.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                    obj.ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    obj.Emailid = ds.Tables[0].Rows[0]["Emailid"].ToString();
                    obj.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                }
            }
            return View(obj);
        }

        [HttpPost]
        [ActionName("CustomerRegistration")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveCustomerRegistration(Customer model)
        {
            try
            {
                model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/mm/yyyy");
                DataSet ds = model.CustomerRegistration();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["Customer"] = "Customer Registration Saved Successfully";
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
            catch (Exception ex)
            {
                TempData["Customer"] = ex.Message;
            }
            return RedirectToAction("CustomerRegistration", "Customer");
        }

        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        public ActionResult CustomerRegistrationList(Customer model)
        {
            List<Customer> lst = new List<Customer>();
            DataSet ds = model.GetCustomerDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Customer obj = new Customer();
                    obj.CustomerId = r["Pk_CustomerId"].ToString();
                    obj.CustomerName = r["CustomerName"].ToString();
                    obj.CustomerAddress = r["CustomerAddress"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.ContactNo = r["ContactNo"].ToString();
                    obj.Emailid = r["Emailid"].ToString();
                    obj.Gender = r["Gender"].ToString();
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
                if(model.CustomerId != null)
                {
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

        public ActionResult DeleteCustomerRegistration(string CustomerId)
        {
            Customer obj = new Customer();
            try
            {
                obj.CustomerId = CustomerId;
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
    }
}