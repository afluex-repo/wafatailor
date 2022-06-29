using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor



        public ActionResult VendorDashBoard()
        {
            return View();
        }
        
        public ActionResult VendorRegistration()
        {
            return View();
        }
        [HttpPost]
        [ActionName("VendorRegistration")]
        public ActionResult VendorRegistration(Vendor model)
        {
            try
            {
                model.AddedBy = "1";
                DataSet ds = model.VendorRegistration();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Vendor"] = "Vendor Registration Successfully!";
                    }
                    else
                    {
                        TempData["Vendor"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Vendor"] = ex.Message;
            }
            return RedirectToAction("VendorRegistration", "Vendor");
        }
    }
}