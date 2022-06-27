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
        public ActionResult CustomerRegistration()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CustomerRegistration")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveCustomerRegistration(Customer model)
        {
            try
            {
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
            return RedirectToAction("CustomerConfirmRegistration", "Customer");
        }

        public ActionResult CustomerConfirmRegistration()
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