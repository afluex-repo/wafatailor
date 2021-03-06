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
        public List<Vendor> lstVendor { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
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
        public string Gender { get; set; }
        public string PK_UserId { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }



        public DataSet VendorRegistration()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FirstName",FirstName),
                   new SqlParameter("@LastName",LastName),
                new SqlParameter("@DOB",DOB),
                   new SqlParameter("@Sex",Gender),
                new SqlParameter("@Address",Address),
                   new SqlParameter("@PinCode",PinCode),
                new SqlParameter("@State",State),
                   new SqlParameter("@City",City),
                new SqlParameter("@Mobile",Mobile),
                   new SqlParameter("@Email",Email),
                new SqlParameter("@ProfilePic",ProfilePic),
                   new SqlParameter("@Fk_RoleId",2),
                    new SqlParameter("@Password",Password),
                new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("VendorRegistration", para);
            return ds;
        }


        public DataSet ChangePassword()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@OldPassword",Password),
                new SqlParameter("@NewPassword",NewPassword),
                 new SqlParameter("@UpdatedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("VendorChangePassword", para);
            return ds;
        }



        public DataSet GetVendorProfileDetails()
        {
            SqlParameter[] para =
       {
                new SqlParameter("@PK_UserId",PK_UserId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetVendorProfileDetails", para);
            return ds;
        }


        public DataSet GetVendorList()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@PK_UserId",PK_UserId),
                  new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetVendorList", para);
            return ds;
        }

        public DataSet DeleteVendor()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_UserId",PK_UserId),
                  new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteVendor", para);
            return ds;
        }
    }
}

