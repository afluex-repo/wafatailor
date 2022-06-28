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
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeDashBoard()
        {
            return View();
        }
        public ActionResult EmployeeRegistration(String EmployeeId)
        {
            Employee obj = new Employee();

            if (EmployeeId != null)
            {
                obj.EmployeeId = EmployeeId;
                DataSet ds = obj.GetEmployeeDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.EmployeeId = ds.Tables[0].Rows[0]["Pk_EmployeeId"].ToString();
                    obj.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                    obj.EmployeeName = ds.Tables[0].Rows[0]["EmployeeName"].ToString();
                    obj.EmployeeAddress = ds.Tables[0].Rows[0]["EmployeeAddress"].ToString();
                    obj.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                    obj.ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    obj.Emailid = ds.Tables[0].Rows[0]["Emailid"].ToString();
                    obj.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                }
            }
            return View(obj);
        }

        [HttpPost]
        [ActionName("EmployeeRegistration")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveEmployeeRegistration(Employee model)
        {
            try
            {
                DataSet ds = model.EmployeeRegistration();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["Employee"] = "Employee Registration Saved Successfully";
                    }
                    else
                    {
                        TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else
                {
                    TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }

            }
            catch (Exception ex)
            {
                TempData["Employee"] = ex.Message;
            }
            return RedirectToAction("EmployeeRegistration", "Employee");
        }

        public ActionResult ConfirmRegistration()
        {
            return View();
        }


        public ActionResult EmployeeRegistrationList(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            DataSet ds = model.GetEmployeeDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.EmployeeId = r["Pk_EmployeeId"].ToString();
                    obj.ShopName = r["ShopName"].ToString();
                    obj.EmployeeName = r["EmployeeName"].ToString();
                    obj.EmployeeAddress = r["EmployeeAddress"].ToString();
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
        [ActionName("EmployeeRegistration")]
        [OnAction(ButtonName = "update")]
        public ActionResult UpdateEmployeeRegistration(Employee model)
        {
            try
            {
                if(model.EmployeeId != null)
                {
                    DataSet ds = model.updateEmployeeRegistration();
                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                        {
                            TempData["Employee"] = "Employee Registration Updated Successfully";
                        }
                        else
                        {
                            TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }

                    }
                    else
                    {
                        TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Employee"] = ex.Message;
            }
            return RedirectToAction("EmployeeRegistration", "Employee");
        }

        public ActionResult DeleteEmployeeRegistration(string EmployeeId)
        {
            Employee obj = new Employee();
            try
            {
                obj.EmployeeId = EmployeeId;
                DataSet ds = obj.DeleteEmployee();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Employee"] = "Employee Deleted Successfully!";
                    }
                    else
                    {
                        TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Employee"] = ex.Message;
            }
            return RedirectToAction("EmployeeRegistrationList", "Employee");
        }

        public ActionResult EmployeeChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmployeeChangePassword(Employee model)
        {
            try
            {
                DataSet ds = model.EmployeeChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["EmployeeChangePassword"] = "Employee Changed Password Successfully!";
                    }
                    else
                    {
                        TempData["EmployeeChangePassword"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["EmployeeChangePassword"] = ex.Message;
            }
            return RedirectToAction("EmployeeChangePassword", "Employee");
        }



        public ActionResult Profile(Employee model)
        {
            model.LoginId = "Employee2";
            DataSet ds = model.GetProfileDetails();
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



    }
}