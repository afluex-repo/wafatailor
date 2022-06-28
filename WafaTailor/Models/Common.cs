using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WafaTailor.Models
{
    public class Common
    {
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ReferBy { get; set; }
        public string Result { get; set; }
        public string DisplayName { get; set; }
        public string AddedOn { get; set; }
        public string Fk_UserId { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Fk_Paymentid { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Package { get; set; }
        public string Leg { get; set; }
        public string ProfilePic { get; set; }
        public string Pk_PPPId { get; set; }
        public string Monthid { get; set; }
        public static string GenerateRandom()
        {
            Random r = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
            {
                s = string.Concat(s, r.Next(10).ToString());
            }
            return s;
        }
        public static string ConvertToSystemDate(string InputDate, string InputFormat)
        {
            string DateString = "";
            DateTime Dt;

            string[] DatePart = (InputDate).Split(new string[] { "-", @"/" }, StringSplitOptions.None);

            if (InputFormat == "dd-MMM-yyyy" || InputFormat == "dd/MMM/yyyy" || InputFormat == "dd/MM/yyyy" || InputFormat == "dd-MM-yyyy" || InputFormat == "DD/MM/YYYY" || InputFormat == "dd/mm/yyyy")
            {
                string Day = DatePart[0];
                string Month = DatePart[1];
                string Year = DatePart[2];

                if (Month.Length > 2)
                    DateString = InputDate;
                else
                    DateString = Month + "/" + Day + "/" + Year;
            }
            else if (InputFormat == "MM/dd/yyyy" || InputFormat == "MM-dd-yyyy")
            {
                DateString = InputDate;
            }
            else
            {
                throw new Exception("Invalid Date");
            }

            try
            {
                //Dt = DateTime.Parse(DateString);
                //return Dt.ToString("MM/dd/yyyy");
                return DateString;
            }
            catch
            {
                throw new Exception("Invalid Date");
            }

        }
        public static List<SelectListItem> AssociateStatus()
        {
            List<SelectListItem> AssociateStatus = new List<SelectListItem>();
            AssociateStatus.Add(new SelectListItem { Text = "All", Value = null });
            AssociateStatus.Add(new SelectListItem { Text = "Active", Value = "P" });
            AssociateStatus.Add(new SelectListItem { Text = "Inactive", Value = "T" });
            //AssociateStatus.Add(new SelectListItem { Text = "TopUp ID", Value = "P" });
            return AssociateStatus;
        }
        public static List<SelectListItem> LegType()
        {
            List<SelectListItem> LegType = new List<SelectListItem>();
            LegType.Add(new SelectListItem { Text = "All", Value = null });
            LegType.Add(new SelectListItem { Text = "Left", Value = "L" });
            LegType.Add(new SelectListItem { Text = "Right", Value = "R" });

            return LegType;
        }
        public static List<SelectListItem> BindTopupStatus()
        {
            List<SelectListItem> IncomeStatus = new List<SelectListItem>();
            IncomeStatus.Add(new SelectListItem { Text = "All", Value = null });
            IncomeStatus.Add(new SelectListItem { Text = "Calculated", Value = "1" });
            IncomeStatus.Add(new SelectListItem { Text = "Pending", Value = "0" });

            return IncomeStatus;
        }
        public static List<SelectListItem> BindGender()
        {
            List<SelectListItem> Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem { Text = "Male", Value = "M" });
            Gender.Add(new SelectListItem { Text = "Female", Value = "F" });
            return Gender;
        }

        public static List<SelectListItem> BindPaymentMode()
        {
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem { Text = "Select Payment Mode", Value = "0" });
            PaymentMode.Add(new SelectListItem { Text = "Cash", Value = "Cash" });
            PaymentMode.Add(new SelectListItem { Text = "Cheque", Value = "Cheque" });
            PaymentMode.Add(new SelectListItem { Text = "NEFT", Value = "NEFT" });
            PaymentMode.Add(new SelectListItem { Text = "RTGS", Value = "RTGS" });
            PaymentMode.Add(new SelectListItem { Text = "Demand Draft", Value = "DD" });
            return PaymentMode;
        }

        public static List<SelectListItem> BindPaymentType()
        {
            List<SelectListItem> PaymentType = new List<SelectListItem>();
            PaymentType.Add(new SelectListItem { Text = "Offline", Value = "Offline" });
            //PaymentType.Add(new SelectListItem { Text = "Online", Value = "Online" });
            return PaymentType;
        }


        public DataSet GetMemberDetails()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@LoginId", ReferBy),

                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetMemberName", para);

            return ds;
        }
        public DataSet GetStateCity()
        {
            SqlParameter[] para = { 
                                      new SqlParameter("@PinCode", PinCode),
                                    
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetStateCity", para);

            return ds;
        }
        public int GenerateRandomNo()
        {
            int _min = 0000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public DataSet BindProduct()
        {
            SqlParameter[] para =
            {
                  new SqlParameter("@ProductId", Package),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetProductList",para);
            return ds;
        }
        public DataSet BindPackageType()
        {
            
            DataSet ds = DBHelper.ExecuteQuery("GetPackageType");
            return ds;
        }
        public DataSet PaymentList()
        {
            SqlParameter[] para =
            {
                  new SqlParameter("@FK_paymentID", Fk_Paymentid),
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPaymentModeList", para);
            return ds;
        }
        public DataSet BindProductForTopUp()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetProductListForTopUp");
            return ds;
        }
        public DataSet BindProductForJoining()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetProductListForJoining");
            return ds;
        }
        public DataSet BindUserTypeForRegistration()
        {

            DataSet ds = DBHelper.ExecuteQuery("GetUserTypeForRegistration");

            return ds;

        }
        public DataSet BindFormTypeMaster()
        {
            SqlParameter[] para = {
                new SqlParameter("@Parameter", 4)
            };
            DataSet ds = DBHelper.ExecuteQuery("FormTypeMasterManage", para);

            return ds;

        }
        public DataSet GetWalletBalance()
        {
            SqlParameter[] para = { new SqlParameter("@PK_USerID", Fk_UserId) };
            DataSet ds = DBHelper.ExecuteQuery("GetWalletBalance", para);

            return ds;

        }
        public DataSet BindTenureMonth()
        {
            SqlParameter[] para = 
            {
                new SqlParameter("@Pk_PPPId", Pk_PPPId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetPPPList", para);
            return ds;
        }
        public DataSet BindProductForJoiningForUser()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetProductListForJoiningUser");
            return ds;
        }
    }
}