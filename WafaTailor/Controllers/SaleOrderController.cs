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
    public class SaleOrderController : Controller
    {
        // GET: SaleOrder
        public ActionResult SaleOrder()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveSaleOrderDetails(SaleOrder userDetail)
        {
            var profile = Request.Files;
            bool status = false;
            var datavaluewaking = Request["SaleorderdataValue"];
            var jssSaleorder = new JavaScriptSerializer();
            var jdvSaleorder = jssSaleorder.Deserialize<dynamic>(Request["SaleorderdataValue"]);
            DataTable dtSaleOrderDetails = new DataTable();
            DataTable dt = new DataTable();
            dtSaleOrderDetails.Columns.Add("Description", typeof(string));
            dtSaleOrderDetails.Columns.Add("Amount", typeof(string));
            dtSaleOrderDetails.Columns.Add("DeliveryDate", typeof(string));
            dt = JsonConvert.DeserializeObject<DataTable>(jdvSaleorder["SaleorderAddData"]);
            
            foreach (DataRow row in dt.Rows)
            {
                var Description = row["Description"].ToString();
                var Amount = row["Amount"].ToString();
                var DeliveryDate = row["DeliveryDate"].ToString();
                dtSaleOrderDetails.Rows.Add(Description);
                dtSaleOrderDetails.Rows.Add(Amount);
                dtSaleOrderDetails.Rows.Add(DeliveryDate);
            }
            userDetail.dtSaleOrderDetails = dtSaleOrderDetails;
            userDetail.AddedBy = "1";
            userDetail.DeliveryDate = string.IsNullOrEmpty(userDetail.DeliveryDate) ? null : Common.ConvertToSystemDate(userDetail.DeliveryDate, "dd/MM/yyyy");
            DataSet ds = new DataSet();
            ds = userDetail.SaveSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["SaleOrder"] = "Sale Order Details saved successfully";
                    status = true;
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    TempData["SaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                TempData["SaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult SaleOrderList(SaleOrder model)
        {
            List<SaleOrder> lst = new List<SaleOrder>();
            DataSet ds = model.GetSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.SaleOrderId = r["Pk_SaleOrderDetailsId"].ToString();
                    obj.Description = r["Description"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.OrderDate = r["OrderDate"].ToString();
                    obj.DeliveryDate = r["DeliveryDate"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }
    }
}