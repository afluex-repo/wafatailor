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

        public ActionResult SaveSaleOrderDetails(SaleOrder order, string SaleorderdataValue)
        {

            try
            {
                string Description = "";
                string Amount = "";
                string DeliveryDate = "";
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(SaleorderdataValue);
                DataTable dtSaleOrderDetails = new DataTable();
                dtSaleOrderDetails.Columns.Add("Description");
                dtSaleOrderDetails.Columns.Add("Amount");
                dtSaleOrderDetails.Columns.Add("DeliveryDate");
              
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["SaleorderAddData"]);
                int numberOfRecords = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    Description = row["Description"].ToString();
                    Amount = row["Amount"].ToString();
                    DeliveryDate = row["DeliveryDate"].ToString();

                    DeliveryDate = string.IsNullOrEmpty(row["DeliveryDate"].ToString()) ? null : Common.ConvertToSystemDate(row["DeliveryDate"].ToString(), "dd/MM/yyyy");                  
                    dtSaleOrderDetails.Rows.Add(Description, Amount, DeliveryDate);
                }
                order.dtSaleOrderDetails = dtSaleOrderDetails;
                order.AddedBy = "1";
                DataSet ds = new DataSet();
                ds = order.SaveSaleOrderDetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["SaleOrder"] = "Sale Order Details saved successfully";

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
            }
            catch (Exception ex)
            {

                TempData["SaleOrder"] = ex.Message;
            }

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserDetails(string LoginId)
        {
            SaleOrder model = new SaleOrder();
            model.LoginId = LoginId;
            DataSet ds = model.GetUserDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    model.Result = "yes";
                    model.Fk_UserId = ds.Tables[0].Rows[0]["Pk_UserId"].ToString();
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
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
        

    }
}