using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class SellOrderController : Controller
    {
        // GET: SellOrder
        public ActionResult SellOrder()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveSellOrderDetails(SellOrder userDetail)
        {
            var profile = Request.Files;
            bool status = false;
            var datavaluesellorder = Request["sellorderdataValue"];
            var jsssellorder = new JavaScriptSerializer();
            var jdvsellorder = jsssellorder.Deserialize<dynamic>(Request["sellorderdataValue"]);
            DataTable dtSellOrderDetails = new DataTable();
            DataTable dt = new DataTable();
            dtSellOrderDetails.Columns.Add("Description", typeof(string));
            //dtSellOrderDetails.Columns.Add("Amount", typeof(string));
            //dtSellOrderDetails.Columns.Add("DeliveryDate", typeof(string));
            dt = JsonConvert.DeserializeObject<DataTable>(jdvsellorder["sellorderAddData"]);
            
            foreach (DataRow row in dt.Rows)
            {
                var Description = row["Description"].ToString();
                //var Amount = row["Amount"].ToString();
                //var DeliveryDate = row["DeliveryDate"].ToString();
                dtSellOrderDetails.Rows.Add(Description);
              
            }
            userDetail.dtSellOrderDetails = dtSellOrderDetails;
           


            userDetail.AddedBy = "1";
            userDetail.DeliveryDate = string.IsNullOrEmpty(userDetail.DeliveryDate) ? null : Common.ConvertToSystemDate(userDetail.DeliveryDate, "dd/MM/yyyy");
            DataSet ds = new DataSet();
            ds = userDetail.SaveSellOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["SellOrder"] = "Sell Order Details saved successfully";
                    status = true;
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    TempData["SellOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                TempData["SellOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return new JsonResult { Data = new { status = status } };
        }


        




    }
}