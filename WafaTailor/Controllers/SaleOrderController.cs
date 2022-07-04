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
    public class SaleOrderController : AdminBaseController
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

        public ActionResult PrintSaleOrder()
        {
            return View();
        }

        #region PrintSaleOrder

        public ActionResult PrintSO(string SaleOrderId, string no)
        {
            SaleOrder model = new SaleOrder();
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();

            //ViewBag.CompanyName = CompanyProfile.CompanyName;
            //ViewBag.CompanyMobile = CompanyProfile.CompanyMobile;
            //ViewBag.CompanyEmail = CompanyProfile.CompanyEmail;
            //ViewBag.CompanyAddress = CompanyProfile.CompanyAddress;
            //ViewBag.BankName = CompanyProfile.BankName;
            //ViewBag.AccountNo = CompanyProfile.AccountNo;
            //ViewBag.IFSC = CompanyProfile.IFSC;

            model.SalesOrderNo = Crypto.Decrypt(no);
            DataSet ds = model.PrintSO();
            model.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
            model.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
            //ViewBag.SaleOrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();


            if (ds != null && ds.Tables[1].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                ViewBag.FinalAmount = 0;
                ViewBag.CGST = ViewBag.SGST = ViewBag.IGST = 0;
                ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                ViewBag.InvoiceNumber = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.Description = r["Description"].ToString();
                    obj.HSNCode = r["HSNCode"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();
                    ViewBag.HSNCode = r["HSNCode"].ToString();
                    ViewBag.AmountInWords = r["FinalAmountWords"].ToString();
                    ViewBag.Rate = r["Rate"].ToString();
                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);

                    ViewBag.FinalAmount = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(r["FinalAmount"].ToString());
                    //ViewBag.FinalAmountGST = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(ViewBag.IGST);

                    ViewBag.CentralTax = (Convert.ToDecimal(ViewBag.FinalAmount) * 9 / 100);
                    ViewBag.StateTax = (Convert.ToDecimal(ViewBag.FinalAmount) * 9 / 100);
                    ViewBag.TotalTax = Convert.ToDecimal(ViewBag.CentralTax) + Convert.ToDecimal(ViewBag.StateTax);

                    lstSaleOrderDetails.Add(obj);
                }
                model.lstsaleorder = lstSaleOrderDetails;
            }
            return View(model);
        }

        #endregion
    }
}