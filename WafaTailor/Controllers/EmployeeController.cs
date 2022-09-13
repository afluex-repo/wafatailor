using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
                obj.DOB = string.IsNullOrEmpty(obj.DOB) ? null : Common.ConvertToSystemDate(obj.DOB, "dd/MM/yyyy");
                DataSet ds = obj.GetEmployeeDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.EmployeeId = ds.Tables[0].Rows[0]["Pk_EmployeeId"].ToString();
                    obj.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                    obj.Fk_ShopId = ds.Tables[0].Rows[0]["Pk_ShopId"].ToString();
                    obj.EmployeeName = ds.Tables[0].Rows[0]["Name"].ToString();
                    obj.EmployeeAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                    //obj.DOB = ds.Tables[0].Rows[0]["DOB"].ToString();
                    obj.ContactNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    //obj.Emailid = ds.Tables[0].Rows[0]["Emailid"].ToString();
                    //obj.Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
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
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/MM/yyyy");
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
                    model.AddedBy = Session["Pk_EmployeeId"].ToString();
                    model.DOB = string.IsNullOrEmpty(model.DOB) ? null : Common.ConvertToSystemDate(model.DOB, "dd/MM/yyyy");
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
                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
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
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
          
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
                model.Date = string.IsNullOrEmpty(model.Date) ? null : Common.ConvertToSystemDate(model.Date, "dd/MM/yyyy");
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
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
                    obj.EmployeeId = r["pk_EmployeeId"].ToString();
                    obj.Salary= r["Salary"].ToString();
                    obj.EmployeeName = r["Name"].ToString();
                    obj.CrAmount = r["TotalCrAmount"].ToString();
                    obj.DrAmount = r["TotalDrAmount"].ToString();
                    obj.RemainingSalary = r["RemainingSalary"].ToString();
                    //obj.Type = r["SalaryType"].ToString();
                    //obj.Date = r["SalaryDate"].ToString();
                    //obj.Remark = r["Remarks"].ToString();
                    lst.Add(obj);
                }
                model.lstSalary = lst;
            }
            return View(model);
        }
        public ActionResult SalaryLedger(string SalaryId)
        {
            Employee model = new Employee();
            model.EmployeeId = SalaryId;
            List<Employee> lst = new List<Employee>();
            DataSet ds = model.GetSalaryLedger();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                   
                    obj.Salary = r["LeftAmount"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    obj.PaymentMode = r["PaymentMode"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.RemainingSalary = r["RemainingSalary"].ToString();
                    obj.Type = r["SalaryType"].ToString();
                    obj.Date = r["SalaryDate"].ToString();
                    obj.Remark = r["Remarks"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    lst.Add(obj);
                }
                model.lstSalary = lst;
            }
            return View(model);
        }

        public ActionResult DailyAttendance(Employee model)
        {
            List<Employee> lst = new List<Employee>();

            DataSet ds1 = model.EmployeeListForAttendance();

            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.EmployeeId = r["PK_EmployeeID"].ToString();
                    obj.EmployeeName = r["Name"].ToString();
                    //obj.EmployeeLoginId = r["LoginID"].ToString();
                    //r["Hours"].ToString()
                    obj.WHLimit = "8";
                    obj.TotalHRWork = r["Hours"].ToString();
                    obj.InTime = r["InTime"].ToString();
                    obj.OutTime = r["OutTime"].ToString();
                    obj.ISHalfDay = r["IshalfDay"].ToString();
                    obj.OverTime = r["Overtime"].ToString();
                    obj.TotalHRWork = r["Hours"].ToString();
                    obj.Attendance = r["Status"].ToString();
                    lst.Add(obj);
                }
            }
            model.lstList = lst;
            return View(model);
        }

        public ActionResult AllPresent(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            model.IsPresent = "1";
            DataSet ds1 = model.EmployeeListForAttendance();

            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.EmployeeId = r["PK_EmployeeID"].ToString();
                    obj.EmployeeName = r["EmployeeName"].ToString();
                    //obj.EmployeeLoginId = r["LoginID"].ToString();
                    obj.WHLimit = r["Hours"].ToString();
                    obj.Attendance = r["Status"].ToString();

                    obj.InTime = r["InTime"].ToString();
                    obj.OutTime = r["OutTime"].ToString();
                    obj.ISHalfDay = r["IshalfDay"].ToString();
                    obj.OverTime = r["Overtime"].ToString();
                    obj.TotalHRWork = r["Hours"].ToString();
                    lst.Add(obj);
                }
            }
            model.lstList = lst;
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("DailyAttendance")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveDailyAttendance(Employee obj)
        {
            string FormName = "";
            string Controller = "";


            obj.AttendanceDate = string.IsNullOrEmpty(obj.AttendanceDate) ? null : Common.ConvertToSystemDate(obj.AttendanceDate, "dd/MM/yyyy");

            try
            {

                string noofrows = Request["hdrows"].ToString();
                string Empid = "";
                string attend = "";
                string intime = "";
                string outtime = "";
                string totalhr = "";
                string overtime = "";
                string ishalfdy = "";

                DataTable dtst = new DataTable();

                dtst.Columns.Add("FK_EmployeeID ", typeof(string));
                dtst.Columns.Add("AttendanceStatus", typeof(string));
                dtst.Columns.Add("InTime", typeof(string));
                dtst.Columns.Add("OutTime ", typeof(string));
                dtst.Columns.Add("TotalHoursWork", typeof(string));
                dtst.Columns.Add("OverTime", typeof(string));
                dtst.Columns.Add("IsHalfDay", typeof(string));


                for (int i = 1; i <= int.Parse(noofrows) - 1; i++)
                {
                    if (Request["txtattend " + i].ToString() == "A")
                    {
                        intime = "";
                        outtime = "";
                        totalhr = "";
                        overtime = "";
                        ishalfdy = "";
                        attend = Request["txtattend " + i].ToString();
                    }
                    else if (Request["txtattend " + i].ToString() == "")
                    {
                        intime = "";
                        outtime = "";
                        totalhr = "";
                        overtime = "";
                        ishalfdy = "";
                        attend = "A";
                    }
                    else
                    {
                        intime = Request["txtintime " + i].ToString();
                        outtime = Request["txtouttime " + i].ToString();
                        totalhr = Request["txttotalhrs " + i].ToString();
                        overtime = Request["txtovertime " + i].ToString();
                        ishalfdy = Request["txthd " + i].ToString();
                        attend = Request["txtattend " + i].ToString();
                    }

                   Empid = Request["empid " + i].ToString();

                    dtst.Rows.Add(Empid, attend, intime, outtime, totalhr, overtime, ishalfdy);

                }

                obj.AddedBy = Session["Pk_EmployeeId"].ToString();
                obj.dtTable = dtst;

                DataSet ds = obj.SaveEmployeeDailyAttendance();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Attendance"] = "Daily Attendance Saved successfully !";

                    }
                    else
                    {
                        TempData["Attendance"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Attendance"] = ex.Message;
            }
            FormName = "DailyAttendance";
            Controller = "Employee";

            return RedirectToAction(FormName, Controller);
        }

        public ActionResult DateWiseAttendanceReport(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds1 = model.DateWiseAttendanceReportBy();

            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.EmployeeId = r["FK_EmployeeID"].ToString();
                    obj.EmployeeName = r["Name"].ToString();
                    obj.ISHalfDay = r["IsHalfDay"].ToString();
                    obj.Attendance = r["Status"].ToString();
                    obj.AttendanceDate = r["AttendanceDate"].ToString();
                    lst.Add(obj);
                }
            }
            model.lstList = lst;
            return View(model);
        }

        public ActionResult EmployeeBillEntry(Employee model, string BillId, string PaymentId)
        {
            #region Shop
            List<SelectListItem> ddlShop = new List<SelectListItem>();
            DataSet ds1 = model.GetShopNameDetails();
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
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds = model.GetCustomerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlcustomer.Add(new SelectListItem { Text = "Select Customer", Value = "0" });
                    }
                    ddlcustomer.Add(new SelectListItem { Text = r["CustomerName"].ToString(), Value = r["PK_UserId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlcustomer = ddlcustomer;
            #endregion

            if (BillId != null && PaymentId != null)
            {
                model.BillId = BillId;
                model.Pk_BillPaymentId = PaymentId;
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                DataSet ds2 = model.GetBillDetails();
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    model.ShopId = ds2.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                    model.LoginId = ds2.Tables[0].Rows[0]["Name"].ToString();
                    model.ContactNo = ds2.Tables[0].Rows[0]["Mobile"].ToString();
                    model.BillNo = ds2.Tables[0].Rows[0]["BillNo"].ToString();
                    model.NoOfPiece = ds2.Tables[0].Rows[0]["NoOfPiece"].ToString();
                    model.DeliveredPiece = ds2.Tables[0].Rows[0]["DeliveredPiece"].ToString();
                    model.RemainingPiece = ds2.Tables[0].Rows[0]["RemainingPiece"].ToString();
                    model.OriginalPrice = ds2.Tables[0].Rows[0]["OriginalPrice"].ToString();
                    model.FinalPrice = ds2.Tables[0].Rows[0]["FinalAmount"].ToString();
                    model.Advance = ds2.Tables[0].Rows[0]["AdavanceAmount"].ToString();
                    model.RemainningBalance = ds2.Tables[0].Rows[0]["RemainingBalance"].ToString();
                    model.BillDate = ds2.Tables[0].Rows[0]["BillDate"].ToString();
                    model.Status = ds2.Tables[0].Rows[0]["Status"].ToString();
                }
            }

            List<SelectListItem> Status = Common.BindStatus();
            ViewBag.BindStatus = Status;
            return View(model);
        }

        [HttpPost]
        [ActionName("EmployeeBillEntry")]
        [OnAction(ButtonName = "SaveBill")]
        public ActionResult BillEntryAction(Employee model)
        {
            try
            {
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = model.SaveEmployeeBillEntry();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["EmployeeBillEntry"] = "Bill Entry saved Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["EmployeeBillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["EmployeeBillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["EmployeeBillEntry"] = ex.Message;
            }
            return RedirectToAction("EmployeeBillEntry", "Employee");
        }

        [HttpPost]
        [ActionName("EmployeeBillEntry")]
        [OnAction(ButtonName = "UpdateBill")]
        public ActionResult UpdateEmployeeBillEntry(Employee model, string BillId, string Pk_BillPaymentId)
        {
            try
            {
                if (BillId != null && Pk_BillPaymentId != null)
                {
                    model.BillId = BillId;
                    model.Pk_BillPaymentId = Pk_BillPaymentId;
                    model.AddedBy = Session["Pk_EmployeeId"].ToString();
                    model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                    DataSet ds = model.UpdateBillEntry();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            TempData["EmployeeBillEntry"] = "Bill Details Updated Successfully !!";
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            TempData["EmployeeBillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        }
                    }
                    else
                    {
                        TempData["EmployeeBillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["EmployeeBillEntry"] = ex.Message;
            }
            return RedirectToAction("EmployeeBillEntry", "Employee");
        }

        public ActionResult EmployeeBillList(Employee model, string LoginId)
        {

            List<Employee> lst = new List<Employee>();
            if (LoginId != "")
            {
                model.LoginId = LoginId;
            }
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            model.AddedBy = Session["Pk_EmployeeId"].ToString();
            DataSet ds = model.GetBillDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.BillId = r["Pk_BillId"].ToString();
                    obj.Pk_BillPaymentId = r["Pk_BillPaymentId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    obj.NoOfPiece = r["NoOfPiece"].ToString();
                    //obj.DeliveredPiece = r["DeliveredPiece"].ToString();
                    //obj.RemainingPiece = r["RemainingPiece"].ToString();
                    obj.OriginalPrice = r["OriginalPrice"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.BillDate = r["BillDate"].ToString();
                    obj.Advance = r["AdavanceAmount"].ToString();
                    obj.RemainingPiece = r["RemainingPiece"].ToString();
                    obj.DeliveredPiece = r["DeliveredPiece"].ToString();
                    obj.GeneratedAmount = r["GeneratedAmount"].ToString();
                    obj.GeneratedPiece = r["GeneratedPiece"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Balance = Convert.ToDecimal(r["RemainingBalance"].ToString());
                    lst.Add(obj);
                }
                model.lstList = lst;
                ViewBag.NoOfPiece = double.Parse(ds.Tables[1].Rows[0]["TotalPiece"].ToString());
                ViewBag.DeliveredPiece = ds.Tables[0].Compute("sum(DeliveredPiece)", "").ToString();
                ViewBag.RemainingPiece = (Convert.ToInt32((ViewBag.NoOfPiece)) - Convert.ToInt32((ViewBag.DeliveredPiece)));
                ViewBag.OriginalPrice = double.Parse(ds.Tables[1].Rows[0]["TotalOriginalPrice"].ToString()).ToString("n2");
                ViewBag.Advance = double.Parse(ds.Tables[0].Compute("sum(AdavanceAmount)", "").ToString()).ToString("n2");
                ViewBag.Balance = (Convert.ToDecimal((ViewBag.OriginalPrice)) - Convert.ToDecimal((ViewBag.Advance)));
            }
            return View(model);
        }

        //public ActionResult EmployeePrintBill(string BillId, string PaymentId)
        //{
        //    List<Employee> lstbill = new List<Employee>();
        //    Employee model = new Employee();
        //    model.BillId = BillId;
        //    model.Pk_BillPaymentId = PaymentId;
        //    DataSet ds = model.EmployeePrintBill();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
        //        ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
        //        //ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
        //        //ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
        //        ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();

        //        model.BillDate = ds.Tables[0].Rows[0]["BillDate"].ToString();
        //        model.Advance = ds.Tables[0].Rows[0]["AdavanceAmount"].ToString();
        //        model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
        //        model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
        //        model.Discount = ds.Tables[0].Rows[0]["Discount"].ToString();
        //        model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
        //        lstbill.Add(model);
        //    }
        //    model.lstList = lstbill;

        //    return View(model);
        //}
        public ActionResult BillPayment(string BillId, string PaymentId)
        {
            Employee model = new Employee();
            model.BillId = BillId;
            model.Pk_BillPaymentId = PaymentId;
            #region Shop
            List<SelectListItem> ddlShop = new List<SelectListItem>();
            DataSet ds1 = model.GetShopNameDetails();
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

            List<SelectListItem> ItemStatus = Common.BindStatus();
            ViewBag.ItemStatus = ItemStatus;

            DataSet ds = model.GetBillDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.ShopId = ds.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                model.BillId = ds.Tables[0].Rows[0]["Pk_BillId"].ToString();
                model.FinalPrice = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                model.RemainningBalance = ds.Tables[0].Rows[0]["RemainingBalance"].ToString();
                model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                model.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                model.RemainingPiece = ds.Tables[0].Rows[0]["RemainingPiece"].ToString();
                model.TotalDeliveredPiece = ds.Tables[0].Rows[0]["TotalDeliveredPiece"].ToString();
                model.LoginId = ds.Tables[0].Rows[0]["Name"].ToString();
                model.ContactNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.TotalPaid = ds.Tables[0].Rows[0]["TotalPaid"].ToString();
                model.Fk_UserId = ds.Tables[0].Rows[0]["Fk_UserId"].ToString();
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("BillPayment")]
        [OnAction(ButtonName = "btnbill")]
        public ActionResult BillPayment(Employee model)
        {
            try
            {
                model.BillDate = string.IsNullOrEmpty(model.BillDate) ? null : Common.ConvertToSystemDate(model.BillDate, "dd/MM/yyyy");
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = new DataSet();
                ds = model.BillPayment();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BillEntry"] = "Payment Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["BillEntry"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["BillEntry"] = ex.Message;
            }
            return RedirectToAction("BillPayment", "Employee");
        }

        public ActionResult EmployeeSaleOrder(Employee obj, string BillId, string paymentid)
        {
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
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds = obj.GetCustomerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlcustomer.Add(new SelectListItem { Text = "Select Customer", Value = "0" });
                    }
                    ddlcustomer.Add(new SelectListItem { Text = r["CustomerName"].ToString(), Value = r["PK_UserId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlcustomer = ddlcustomer;
            #endregion

            if (BillId != null)
            {
                obj.BillId = BillId;
                obj.PaymentId = paymentid;
                DataSet ds2 = obj.GetBillDetails();
                if (ds2 != null && ds2.Tables[0].Rows.Count > 0 && ds2.Tables.Count > 0)
                {
                    obj.BillId = ds2.Tables[0].Rows[0]["Pk_BillId"].ToString();
                    obj.ShopId = ds2.Tables[0].Rows[0]["Fk_Shopid"].ToString();
                    obj.LoginId = ds2.Tables[0].Rows[0]["Name"].ToString();
                    obj.ContactNo = ds2.Tables[0].Rows[0]["Mobile"].ToString();
                    obj.BillNo = ds2.Tables[0].Rows[0]["BillNo"].ToString();
                    obj.NoOfPiece = ds2.Tables[0].Rows[0]["NoOfPiece"].ToString();
                    obj.OriginalPrice = ds2.Tables[0].Rows[0]["OriginalPrice"].ToString();
                    obj.NetAmount = ds2.Tables[0].Rows[0]["FinalAmount"].ToString();
                    obj.Pk_UserId = ds2.Tables[0].Rows[0]["Fk_UserId"].ToString();
                    obj.TotalDeliveredPiece = ds2.Tables[0].Rows[0]["TotalDeliveredPiece"].ToString();
                }
            }
            return View(obj);
        }

        [HttpPost]
        public JsonResult SaveEmployeeSaleOrderDetails(Employee order, string dataValue)
        {
            try
            {
                string Name = "";
                string Piece = "";
                string OriginalPrice = "";
                string Discount = "";
                string FinalPrice = "";
                string SaleDate = "";
                string Description = "";
                //int rowsno = 0;
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtorder = new DataTable();
                dtorder.Columns.Add("Name");
                dtorder.Columns.Add("Piece");
                dtorder.Columns.Add("OriginalPrice");
                dtorder.Columns.Add("Discount");
                dtorder.Columns.Add("FinalPrice");
                dtorder.Columns.Add("SaleDate");
                dtorder.Columns.Add("Description");
                //dtorder.Columns.Add("rowsno");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;
                //foreach (DataRow row in dt.Rows)

                foreach (DataRow row in dt.Rows)
                {
                    Name = "";
                    Piece = row["Piece"].ToString();
                    OriginalPrice = row["OriginalPrice"].ToString();
                    Discount = row["Discount"].ToString();
                    FinalPrice = row["NetAmount"].ToString();
                    SaleDate = string.IsNullOrEmpty(row["SaleDate"].ToString()) ? null : Common.ConvertToSystemDate(row["SaleDate"].ToString(), "dd/MM/yyyy");
                    Description = row["Description"].ToString();

                    //rowsno = rowsno + 1;
                    dtorder.Rows.Add(Name, Piece, OriginalPrice, Discount, FinalPrice, SaleDate, Description);
                }
                order.dt = dtorder;
                order.Pk_UserId = order.Pk_UserId == "" ? null : order.Pk_UserId;
                order.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = order.SaveEmployeeSaleOrder();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        order.Result = "Yes";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {

                order.Result = ex.Message;
            }

            return new JsonResult { Data = new { status = order.Result } };
        }

        public ActionResult GetcustomerList()
        {
            Employee obj = new Employee();
            List<Employee> lst = new List<Employee>();
            DataSet ds = obj.GetCustomerDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Employee objList = new Employee();
                    objList.Name = dr["CustomerName"].ToString();
                    objList.ContactNo = dr["Mobile"].ToString();
                    objList.LoginId = dr["LoginId"].ToString();
                    lst.Add(objList);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserDetails(string LoginId, string Mobile)
        {
            Employee model = new Employee();
            model.LoginId = LoginId;
            model.ContactNo = Mobile;
            DataSet ds = model.GetUserDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    model.Result = "yes";
                    model.Fk_UserId = ds.Tables[0].Rows[0]["Pk_UserId"].ToString();
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    model.ContactNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeExpense()
        {
            Employee obj = new Employee();
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds = obj.GetExpenseType();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlExpensetype.Add(new SelectListItem { Text = "Expense Type", Value = "0" });
                    }
                    ddlExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlExpensetype = ddlExpensetype;
            #endregion

            #region OtherExpenseType
            List<SelectListItem> ddlOtherExpensetype = new List<SelectListItem>();
            DataSet ds1 = obj.GetOtherExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlOtherExpensetype.Add(new SelectListItem { Text = "", Value = "" });
                    }
                    ddlOtherExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["Pk_OtherExpenseId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlOtherExpensetype = ddlOtherExpensetype;
            #endregion

            #region Vendor
            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            DataSet ds2 = obj.GetVendor();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendor.Add(new SelectListItem { Text = "", Value = "" });
                    }
                    ddlVendor.Add(new SelectListItem { Text = r["MaterialType"].ToString(), Value = r["Pk_MaterialId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlVendor = ddlVendor;
            #endregion
            return View();
        }

        [HttpPost]
        public JsonResult ActionExpense(Employee model, string dataValue)
        {
            try
            {
                model.ExpenseDate = string.IsNullOrEmpty(model.ExpenseDate) ? null : Common.ConvertToSystemDate(model.ExpenseDate, "dd/MM/yyyy");
                string Expensetype = "";
                string ExpenseRupee = "";
                string ExpenseDate = "";
                string Remark = "";
                string OtherExpensetype = "";
                var isValidModel = TryUpdateModel(model);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtmodel = new DataTable();
                dtmodel.Columns.Add("Expensetype");
                dtmodel.Columns.Add("OtherExpensetype");
                dtmodel.Columns.Add("ExpenseDate");
                dtmodel.Columns.Add("ExpenseRupee");
                dtmodel.Columns.Add("Remark");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;

                foreach (DataRow row in dt.Rows)
                {
                    Expensetype = row["Expensetype"].ToString();
                    OtherExpensetype = row["OtherExpensetype"].ToString();
                    model.OtherExpensetype = OtherExpensetype == "0" ? null : OtherExpensetype;
                    //ExpenseDate = row["ExpenseDate"].ToString();
                    ExpenseDate = string.IsNullOrEmpty(row["ExpenseDate"].ToString()) ? null : Common.ConvertToSystemDate(row["ExpenseDate"].ToString(), "dd/MM/yyyy");
                    ExpenseRupee = row["ExpenseRupee"].ToString();
                    Remark = row["Remark"].ToString();
                    //rowsno = rowsno + 1;
                    dtmodel.Rows.Add(Expensetype, OtherExpensetype, ExpenseDate, ExpenseRupee, Remark);
                }
                model.dt = dtmodel;
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                DataSet ds = new DataSet();
                ds = model.SaveEmployeeExpense();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        model.Result = "Yes";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                model.Result = ex.Message;
            }
            return new JsonResult { Data = new { status = model.Result } };
        }

        public ActionResult EmployeeSaleOrderList(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            model.AddedBy = Session["Pk_EmployeeId"].ToString();
            DataSet ds = model.GetEmployeeSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.SaleOrderId = r["Pk_SaleOrderId"].ToString();
                    obj.ShopName = r["ShopName"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.SalesOrderNo = r["SalesOrderNo"].ToString();
                    obj.CustomerName = r["customerName"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }

        public ActionResult PrintSaleOrder(String SaleOrderId)
        {
            List<Employee> lstSaleOrderDetails = new List<Employee>();
            Employee model = new Employee();
            model.SaleOrderId = SaleOrderId;
            DataSet ds = model.PrintSO();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    Employee obj = new Employee();
                    obj.SaleDate = r["SaleDate"].ToString();
                    obj.PieceName = r["PieceName"].ToString();
                    obj.NoOfPiece = r["NoOfPiece"].ToString();
                    obj.OriginalPrice = r["OriginalPrice"].ToString();
                    obj.Discount = r["Discount"].ToString();
                    obj.FinalPrice = r["FinalPrice"].ToString();
                    lstSaleOrderDetails.Add(obj);
                }
                model.lstsaleorder = lstSaleOrderDetails;
                ViewBag.FinalPrice = double.Parse(ds.Tables[1].Compute("sum(FinalPrice)", "").ToString()).ToString("n2");
            }

            return View(model);
        }

        public ActionResult EmployeeExpenseList()
        {
            Employee model = new Employee();
            List<Employee> lst = new List<Employee>();
            model.AddedBy = Session["Pk_EmployeeId"].ToString();
            DataSet ds = model.GetEmployeeExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.Pk_ExpenseId = r["Pk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.OtherExpenseName = r["OtherExpenseName"].ToString();
                    //obj.Vendor = r["Vendor"].ToString();
                    obj.Expenses = r["Expense"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.ExpenseDate = r["ExpenseDate"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds1 = model.GetExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlExpensetype.Add(new SelectListItem { Text = "Expense Type", Value = "0" });
                    }
                    ddlExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlExpensetype = ddlExpensetype;
            #endregion
            return View(model);
        }

        [HttpPost]
        [ActionName("EmployeeExpenseList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult ExpenseList(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            model.Expensetype = model.Expensetype == "0" ? null : model.Expensetype;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet ds = model.GetEmployeeExpenseList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.Pk_ExpenseId = r["Pk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.OtherExpenseName = r["OtherExpenseName"].ToString();
                    obj.Expenses = r["Expense"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    //obj.Vendor = r["Vendor"].ToString();
                    obj.ExpenseDate = r["ExpenseDate"].ToString();
                    lst.Add(obj);
                }
                model.lstexpense = lst;
            }
            #region ExpenseType
            List<SelectListItem> ddlExpensetype = new List<SelectListItem>();
            DataSet ds1 = model.GetExpenseType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlExpensetype.Add(new SelectListItem { Text = "Expense Type", Value = "0" });
                    }
                    ddlExpensetype.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlExpensetype = ddlExpensetype;
            #endregion
            return View(model);
        }

        [HttpPost]
        [ActionName("EmployeeBillList")]
        [OnAction(ButtonName = "btnSearch")]
        public ActionResult EmployeeBillListSearch(Employee model, string LoginId)
        {
            List<Employee> lst = new List<Employee>();
            if (LoginId != "")
            {
                model.LoginId = LoginId;
            }
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet ds = model.GetBillDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.BillId = r["Pk_BillId"].ToString();
                    obj.Pk_BillPaymentId = r["Pk_BillPaymentId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    obj.NoOfPiece = r["NoOfPiece"].ToString();
                    obj.OriginalPrice = r["OriginalPrice"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.BillDate = r["BillDate"].ToString();
                    obj.Advance = r["AdavanceAmount"].ToString();
                    obj.RemainingPiece = r["RemainingPiece"].ToString();
                    obj.DeliveredPiece = r["DeliveredPiece"].ToString();
                    obj.GeneratedAmount = r["GeneratedAmount"].ToString();
                    obj.GeneratedPiece = r["GeneratedPiece"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.Balance = Convert.ToDecimal(r["RemainingBalance"].ToString());
                    lst.Add(obj);
                }
                model.lstList = lst;
                ViewBag.NoOfPiece = double.Parse(ds.Tables[1].Rows[0]["TotalPiece"].ToString());
                ViewBag.DeliveredPiece = ds.Tables[0].Compute("sum(DeliveredPiece)", "").ToString();
                ViewBag.RemainingPiece = (Convert.ToInt32((ViewBag.NoOfPiece)) - Convert.ToInt32((ViewBag.DeliveredPiece)));
                ViewBag.OriginalPrice = double.Parse(ds.Tables[1].Rows[0]["TotalOriginalPrice"].ToString()).ToString("n2");
                ViewBag.Advance = double.Parse(ds.Tables[0].Compute("sum(AdavanceAmount)", "").ToString()).ToString("n2");
                ViewBag.Balance = (Convert.ToDecimal((ViewBag.OriginalPrice)) - Convert.ToDecimal((ViewBag.Advance)));
            }
            return View(model);
        }

        public ActionResult EmployeeOrderRefund()
        {
            return View();
        }

        [HttpPost]
        [ActionName("EmployeeOrderRefund")]
        [OnAction(ButtonName = "Save")]
        public ActionResult ActionEmployeeOrderRefund(Employee model)
        {
            try
            {
                model.AddedBy = Session["Pk_EmployeeId"].ToString();
                model.RefundDate = string.IsNullOrEmpty(model.RefundDate) ? null : Common.ConvertToSystemDate(model.RefundDate, "dd/MM/yyyy");
                DataSet ds = model.EmployeeOrderRefund();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["msg"].ToString() == "1")
                    {
                        TempData["Order"] = "Order Refund saved Successfully !!";
                    }
                    else if (ds.Tables[0].Rows[0]["ErrorMessage"].ToString() == "0")
                    {
                        TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["Order"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Order"] = ex.Message;
            }
            return RedirectToAction("EmployeeOrderRefund", "Employee");
        }



        public ActionResult EmployeeOrderRefundList(Employee model)
        {
            List<Employee> lst = new List<Employee>();
            model.AddedBy = Session["Pk_EmployeeId"].ToString();
            DataSet ds = model.GetEmployeeOrderRefundList();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Employee obj = new Employee();
                    obj.RefundId = r["Pk_RefundId"].ToString();
                    //obj.PieceName = r["PieceName"].ToString();
                    obj.NoOfPiece = r["RefundPiece"].ToString();
                    obj.ContactNo = r["Mobile"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.Balance = Convert.ToDecimal(r["Amount"].ToString());
                    obj.RefundDate = r["RefundDate"].ToString();
                    lst.Add(obj);
                }
                model.lstList = lst;
            }
            return View(model);
        }

        public ActionResult PrintEmployeeOrderRefund(string RefundId)
        {
            Employee model = new Employee();
            model.RefundId = RefundId;
            DataSet ds = model.PrintEmployeeOrderRefundBill();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
                //model.PieceName = ds.Tables[0].Rows[0]["PieceName"].ToString();
                model.ContactNo = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.Balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"].ToString());
                model.NoOfPiece = ds.Tables[0].Rows[0]["ReturnPiece"].ToString();
            }
            return View(model);
        }
    }
}