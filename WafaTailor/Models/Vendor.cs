using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WafaTailor.Models
{
    public class Vendor
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ProfilePic { get; set; }
        public string AddedBy { get; set; }
        public string Fk_RoleId { get; set; }

        public DataSet VendorRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FirstName",FirstName),
                   new SqlParameter("@LastName",LastName),
                new SqlParameter("@DOB",DOB),
                   new SqlParameter("@Sex",Sex),
                new SqlParameter("@Address",Address),
                   new SqlParameter("@PinCode",PinCode),
                new SqlParameter("@State",State),
                   new SqlParameter("@City",City),
                new SqlParameter("@Mobile",Mobile),
                   new SqlParameter("@Email",Email),
                new SqlParameter("@ProfilePic",ProfilePic),
                   new SqlParameter("@Fk_RoleId",Fk_RoleId),
                new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("VendorRegistration", para);
            return ds;
        }

    }
}