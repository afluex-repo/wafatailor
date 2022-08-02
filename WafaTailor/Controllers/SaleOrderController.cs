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
        public ActionResult SaleOrder(SaleOrder obj)
        {
            #region Shop
            List<SelectListItem> ddlShop = new List<SelectListItem>();
            DataSet ds1 = obj.GetShopNameDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlShop.Add(new SelectListItem { Text = "Select Shop", Value = "0" });
                    }
                    ddlShop.Add(new SelectListItem { Text = r["ShopName"].ToString(), Value = r["Pk_ShopId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlShop = ddlShop;
            #endregion
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds = obj.GetCustomerDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlcustomer.Add(new SelectListItem { Text = "Select Customer", Value = "0" });
                    }
                    ddlcustomer.Add(new SelectListItem { Text = r["CustomerName"].ToString(), Value = r["PK_UserId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlcustomer = ddlcustomer;
            #endregion
            return View(obj);
        }


        [HttpPost]
        public JsonResult SaveSaleOrderDetails(SaleOrder order, string dataValue)
        {
            try
            {
                //order.SaleOrderDate = string.IsNullOrEmpty(order.SaleOrderDate) ? null : Common.ConvertToSystemDate(order.SaleOrderDate, "dd/MM/yyyy");
                string Name = "";
                string Piece = "";
                string OriginalPrice = "";
                string Discount = "";
                string FinalPrice = "";
                string SaleDate = "";
                string Description = "";
                //int rowsno = 0;
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);

                DataTable dtorder = new DataTable();
                dtorder.Columns.Add("Name");
                dtorder.Columns.Add("Piece");
                dtorder.Columns.Add("OriginalPrice");
                dtorder.Columns.Add("Discount");
                dtorder.Columns.Add("FinalPrice");
                dtorder.Columns.Add("SaleDate");
                dtorder.Columns.Add("Description");
                //dtorder.Columns.Add("rowsno");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["dataValue"]);
                int numberOfRecords = dt.Rows.Count;
                //foreach (DataRow row in dt.Rows)

                foreach (DataRow row in dt.Rows)
                {
                    Name = row["Name"].ToString();
                    Piece = row["Piece"].ToString();
                    OriginalPrice = row["OriginalPrice"].ToString();
                    Discount = row["Discount"].ToString();
                    FinalPrice = row["NetAmount"].ToString();
                    SaleDate = string.IsNullOrEmpty(row["SaleDate"].ToString()) ? null : Common.ConvertToSystemDate(row["SaleDate"].ToString(), "dd/MM/yyyy");
                    Description = row["Description"].ToString();

                    //rowsno = rowsno + 1;
                    dtorder.Rows.Add(Name, Piece, OriginalPrice, Discount, FinalPrice, SaleDate, Description);
                }
                order.dt = dtorder;
                order.AddedBy = Session["Pk_EmployeeId"].ToString();

                DataSet ds = new DataSet();
                ds = order.SaveSaleOrder();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        order.Result = "Yes";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {

                order.Result = ex.Message;
            }

            return new JsonResult { Data = new { status = order.Result } };
        }
        //public ActionResult SaveSaleOrderDetails(SaleOrder order, string SaleorderdataValue)
        //{

        //    try
        //    {
        //        string Description = "";
        //        string Amount = "";
        //        string DeliveryDate = "";
        //        var isValidModel = TryUpdateModel(order);
        //        var jss = new JavaScriptSerializer();
        //        var jdv = jss.Deserialize<dynamic>(SaleorderdataValue);
        //        DataTable dtSaleOrderDetails = new DataTable();
        //        dtSaleOrderDetails.Columns.Add("Description");
        //        dtSaleOrderDetails.Columns.Add("Amount");
        //        dtSaleOrderDetails.Columns.Add("DeliveryDate");

        //        DataTable dt = new DataTable();
        //        dt = JsonConvert.DeserializeObject<DataTable>(jdv["SaleorderAddData"]);
        //        int numberOfRecords = dt.Rows.Count;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            Description = row["Description"].ToString();
        //            Amount = row["Amount"].ToString();
        //            DeliveryDate = row["DeliveryDate"].ToString();

        //            DeliveryDate = string.IsNullOrEmpty(row["DeliveryDate"].ToString()) ? null : Common.ConvertToSystemDate(row["DeliveryDate"].ToString(), "dd/MM/yyyy");                  
        //            dtSaleOrderDetails.Rows.Add(Description, Amount, DeliveryDate);
        //        }
        //        order.dtSaleOrderDetails = dtSaleOrderDetails;
        //        order.AddedBy = "1";
        //        DataSet ds = new DataSet();
        //        ds = order.SaveSaleOrderDetails();
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {
        //                TempData["SaleOrder"] = "Sale Order Details saved successfully";

        //            }
        //            else if (ds.Tables[0].Rows[0][0].ToString() == "0")
        //            {
        //                TempData["SaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //        else
        //        {
        //            TempData["SaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        TempData["SaleOrder"] = ex.Message;
        //    }

        //    return Json(order, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetcustomerList()
        {
            SaleOrder obj = new SaleOrder();
            List<SaleOrder> lst = new List<SaleOrder>();
            DataSet ds = obj.GetCustomerDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SaleOrder objList = new SaleOrder();
                    objList.Name = dr["CustomerName"].ToString();
                    objList.Mobile = dr["Mobile"].ToString();
                    lst.Add(objList);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
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
                    obj.SaleOrderId = r["Pk_SaleOrderId"].ToString();
                    obj.ShopName = r["ShopName"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.SalesOrderNo = r["SalesOrderNo"].ToString();
                    obj.CustomerName = r["customerName"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
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
                ViewBag.BillNo = ds.Tables[0].Rows[0]["BillNo"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    model.SaleDate = ds.Tables[1].Rows[0]["SaleDate"].ToString();
                    model.PieceName = ds.Tables[1].Rows[0]["PieceName"].ToString();
                    model.NoOfPiece = ds.Tables[1].Rows[0]["NoOfPiece"].ToString();
                    model.OriginalPrice = ds.Tables[1].Rows[0]["OriginalPrice"].ToString();
                    model.Discount = ds.Tables[1].Rows[0]["Discount"].ToString();
                    model.FinalPrice = ds.Tables[1].Rows[0]["FinalPrice"].ToString();
                    lstSaleOrderDetails.Add(model);
                }
                model.lstsaleorder = lstSaleOrderDetails;
                ViewBag.FinalPrice = double.Parse(ds.Tables[1].Compute("sum(FinalPrice)", "").ToString()).ToString("n2");
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