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
    public class EmployeeController : AdminBaseController
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
            #region Shop
            List<SelectListItem> ddlShop = new List<SelectListItem>();
            DataSet ds1 = obj.GetShopNameDetails();
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
            #region Type
            List<SelectListItem> ddlType = new List<SelectListItem>();
            DataSet ds2 = obj.GetUserTypeDetails();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlType.Add(new SelectListItem { Text = "Select Type", Value = "0" });
                    }
                    ddlType.Add(new SelectListItem { Text = r["UserType"].ToString(), Value = r["PK_UserTypeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlType = ddlType;
            #endregion

            if (EmployeeId != null)
            {
                obj.EmployeeId = EmployeeId;
                DataSet ds = obj.GetEmployeeDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.EmployeeId = ds.Tables[0].Rows[0]["Pk_EmployeeId"].ToString();
                    obj.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                    obj.Fk_ShopId = ds.Tables[0].Rows[0]["Pk_ShopId"].ToString();
                    obj.EmployeeName = ds.Tables[0].Rows[0]["Name"].ToString();
                    obj.EmployeeAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                    obj.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                    obj.ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    obj.Emailid = ds.Tables[0].Rows[0]["Emailid"].ToString();
                    obj.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                    obj.Salary = ds.Tables[0].Rows[0]["Salary"].ToString();
                }
            }
            return View(obj);
        }

        [HttpPost]
        [ActionName("EmployeeRegistration")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveEmployeeRegistration(Employee model)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                //model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/MM/yyyy");
                DataSet ds = model.EmployeeRegistration();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        //TempData["Employee"] = "Employee Registration Saved Successfully";
                        Session["EmployeeName"] = ds.Tables[0].Rows[0]["EmployeeName"].ToString();
                        Session["EmployeeLoginId"] = ds.Tables[0].Rows[0]["Loginid"].ToString();
                        Session["EmployeePassword"] = ds.Tables[0].Rows[0]["Password"].ToString();
                        FormName = "EmployeeConfirmRegistration";
                        Controller = "Employee";
                    }
                    else
                    {
                        TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        FormName = "EmployeeRegistration";
                        Controller = "Employee";
                    }

                }
                else
                {
                    TempData["Employee"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    FormName = "EmployeeRegistration";
                    Controller = "Employee";
                }

            }
            catch (Exception ex)
            {
                TempData["Employee"] = ex.Message;
            }
            return RedirectToAction(FormName,Controller);
        }

        public ActionResult EmployeeConfirmRegistration()
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
                    obj.EmployeeName = r["Name"].ToString();
                    obj.EmployeeDetails = r["EmployeeDetails"].ToString();
                    obj.EmployeeAddress = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.ContactNo = r["ContactNo"].ToString();
                    obj.Emailid = r["Emailid"].ToString();
                    obj.Gender = r["Gender"].ToString();
                    obj.Password = r["Password"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    //obj.LoginId = r["LoginId"].ToString();
                    obj.Salary = r["Salary"].ToString();
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
                   // model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "mm/dd/yyyy");
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
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
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
            model.LoginId = Session["LoginId"].ToString();
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

        [HttpPost]
        [ActionName("EmployeeRegistrationList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult EmployeeRegistrationListBy(Employee model)
        {
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "MM/dd/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "MM/dd/yyyy");
            List<Employee> lst = new List<Employee>();
            DataSet ds = model.GetEmployeeDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.EmployeeId = r["Pk_EmployeeId"].ToString();
                    obj.ShopName = r["ShopName"].ToString();
                    //obj.EmployeeName = r["Name"].ToString();
                    obj.EmployeeDetails = r["EmployeeDetails"].ToString();
                    obj.EmployeeAddress = r["Address"].ToString();
                    obj.DOB = r["DOB"].ToString();
                    obj.ContactNo = r["ContactNo"].ToString();
                    obj.Emailid = r["Emailid"].ToString();
                    obj.Gender = r["Gender"].ToString();
                    obj.Password = r["Password"].ToString();
                    obj.JoiningDate = r["JoiningDate"].ToString();
                    //obj.LoginId = r["LoginId"].ToString();
                    obj.Salary = r["Salary"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }

        public ActionResult EmployeeSalaryManagement()
        {
            Employee obj = new Employee();
            #region Name
            List<SelectListItem> ddlName = new List<SelectListItem>();
            DataSet ds1 = obj.GetEmployeeNameDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlName.Add(new SelectListItem { Text = "Select Name", Value = "0" });
                    }
                    ddlName.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_EmployeeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlName = ddlName;
            #endregion

            #region Type
            List<SelectListItem> ddlSaleryType = new List<SelectListItem>();
            DataSet ds2 = obj.GetSalaryTypeDetails();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlSaleryType.Add(new SelectListItem { Text = "Select Type", Value = "0" });
                    }
                    ddlSaleryType.Add(new SelectListItem { Text = r["SalaryType"].ToString(), Value = r["SalaryType"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlSaleryType = ddlSaleryType;
            #endregion

            #region Payment Mode
            List<SelectListItem> ddlpaymentmode = new List<SelectListItem>();
            ddlpaymentmode.Add(new SelectListItem { Text = "Select Payment Mode", Value = "0" });
            DataSet ds = obj.GetPaymentMode();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    ddlpaymentmode.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                }
            }
            ViewBag.ddlpaymentmode = ddlpaymentmode;
            #endregion

            return View();
        }
                
        [HttpPost]
        [ActionName("EmployeeSalaryManagement")]
        [OnAction(ButtonName = "Save")]
        public ActionResult EmployeeSalaryManagementAction(Employee model)
        {
            try
            {
                //model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/MM/yyyy");
                DataSet ds = model.SalaryManagement();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["Salary"] = "Salary Saved Successfully";
                    }
                    else
                    {
                        TempData["Salary"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Salary"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Salary"] = ex.Message;
            }
            return RedirectToAction("EmployeeSalaryManagement", "Employee");
        }




        public ActionResult SalaryList(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            DataSet ds = model.GetSalaryDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.SalaryId = r["Pk_EmpSalaryId"].ToString();
                    obj.EmployeeId = r["Fk_EmployeeId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Type = r["SalaryType"].ToString();
                    obj.Date = r["SalaryDate"].ToString();
                    obj.Remark = r["Remarks"].ToString();
                    lst.Add(obj);
                }
                model.lstSalary = lst;
            }
            return View(model);
        }

        public ActionResult SalaryLedger()
        {
            return View();
        }
    }
}