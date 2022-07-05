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

        public ActionResult PrintSaleOrder(string SaleOrderId)
        {
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();
            SaleOrder model = new SaleOrder();
            model.SaleOrderId = SaleOrderId;
            DataSet ds = model.PrintSO();
            if (ds !=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.CustomerName = ds.Tables[0].Rows[0]["Name"].ToString();
                ViewBag.CustomerMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                ViewBag.Email = ds.Tables[0].Rows[0]["Email"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    model.Amount = ds.Tables[0].Rows[0]["Amount"].ToString();
                    model.OrderDate = ds.Tables[0].Rows[0]["OrderDate"].ToString();
                    model.DeliveryDate = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();
                    lstSaleOrderDetails.Add(model);
                }
                model.lstsaleorder = lstSaleOrderDetails;
                //ViewBag.Amount = double.Parse(ds.Tables[1].Compute("sum(Amount)", "").ToString()).ToString("n2");
            }

                return View(model);
        }

       

        //public ActionResult GenerateInvoice()
        //{
        //    SaleOrder obj = new SaleOrder();
        //    List<SaleOrder> lst = new List<SaleOrder>();

        //    DataSet ds = obj.GetInvoiceNoList();
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            SaleOrder objInvoice = new SaleOrder();

        //            //objInvoice.PK_InvoiceNoID = Crypto.Encrypt(dr["PK_InvoiceNoID"].ToString());
        //            //objInvoice.SaleOrderNoEncrypt = Crypto.Encrypt(dr["InvoiceNo"].ToString());
        //            //objInvoice.InvoiceNo = dr["InvoiceNo"].ToString();
        //            //objInvoice.InvoiceDate = dr["InvoiceDate"].ToString();
        //            //objInvoice.LineStatus = dr["Status"].ToString();

        //            lst.Add(objInvoice);
        //        }
        //        obj.lstInvoiceNo = lst;
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //[ActionName("GenerateInvoice")]
        //[OnAction(ButtonName = "btnGenerateInvoiceNo")]
        //public ActionResult GenerateInvoiceNo(SaleOrder model)
        //{
        //    try
        //    {
        //        model.AddedBy = Session["UserID"].ToString();
        //        DataSet ds = model.GenerateInvoiceNo();
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {
        //                TempData["Class"] = "alert alert-success";
        //                TempData["InvoiceNo"] = "Invoice Number generated successfully";
        //            }
        //            else if (ds.Tables[0].Rows[0][0].ToString() == "0")
        //            {
        //                TempData["Class"] = "alert alert-danger";
        //                TempData["InvoiceNo"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Class"] = "alert alert-danger";
        //        TempData["InvoiceNo"] = "ERROR : " + ex.Message;
        //    }
        //    return RedirectToAction("InvoiceNo");
        //}

        //public ActionResult GenerateBill(string invid, string no)
        //{
        //    SaleOrder model = new Models.SaleOrder();
        //    model.PK_InvoiceNoID = Crypto.Decrypt(invid);
        //    model.InvoiceNo = Crypto.Decrypt(no);

        //    return View(model);
        //}


    }
}