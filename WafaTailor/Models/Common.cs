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
        public static List<SelectListItem> BindGender()
        {
            List<SelectListItem> Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem { Text = "-Select-", Value = "0" });
            Gender.Add(new SelectListItem { Text = "Male", Value = "M" });
            Gender.Add(new SelectListItem { Text = "Female", Value = "F" });
            return Gender;
        }
        public static List<SelectListItem> BindStatus()
        {
            List<SelectListItem> Status = new List<SelectListItem>();

            Status.Add(new SelectListItem { Text = "UnPaid/Not Delivered", Value = "UnPaid/Not Delivered" });
            Status.Add(new SelectListItem { Text = "Paid/ Not Delivered", Value = "Paid/ Not Delivered" });
            Status.Add(new SelectListItem { Text = "UnPaid/ Delivered", Value = "UnPaid/ Delivered" });
            Status.Add(new SelectListItem { Text = "Paid/ Delivered", Value = "Paid/ Delivered" });
            Status.Add(new SelectListItem { Text = "Sale Order", Value = "Sale Order" });
            Status.Add(new SelectListItem { Text = "Cancle", Value = "Cancle" });
            return Status;
        }
        public DataSet GetStateCity()
        {
            SqlParameter[] para = { 
                                      new SqlParameter("@PinCode", PinCode),
                                    
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetStateCity", para);
            return ds;
        }
    }
}