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
    public class ShopController : ShopBaseController
    {
        // GET: Shop
        public ActionResult ShopDashBoard()
        {
            return View();
        }
        public ActionResult ShopSaleOrder(Shop obj)
        {
            #region Customer
            List<SelectListItem> ddlcustomer = new List<SelectListItem>();
            DataSet ds1 = obj.GetCustomerDetails();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
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
        public JsonResult SaveSaleOrder(Shop order, string dataValue)
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
                    Description = "";
                    //row["Description"].ToString()
                    //rowsno = rowsno + 1;
                    dtorder.Rows.Add(Name, Piece, OriginalPrice, Discount, FinalPrice, SaleDate,Description);
                }
                order.dt = dtorder;
                order.AddedBy = Session["Pk_userId"].ToString();
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




        public ActionResult ShopSaleOrderList(Shop model)
        {
            List<Shop> lst = new List<Shop>();
            model.AddedBy = Session["Pk_userId"].ToString();
            DataSet ds = model.GetShopSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Shop obj = new Shop();
                    obj.SaleOrderId = r["Pk_SaleOrderId"].ToString();
                    obj.BillNo = r["BillNo"].ToString();
                    obj.SalesOrderNo = r["SalesOrderNo"].ToString();
                    obj.customerName = r["customerName"].ToString();
                    obj.Mobile = r["Mobile"].ToString();
                    lst.Add(obj);
                }
                model.lstShopRegistration = lst;
            }
            return View(model);
        }
        public ActionResult PrintShopSaleOrder(String SaleOrderId)
        {
            List<Shop> lstShopSaleOrderDetails = new List<Shop>();
            Shop model = new Shop();
            model.SaleOrderId = SaleOrderId;
            DataSet ds = model.PrintShopSO();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                    model.SaleDate = ds.Tables[0].Rows[0]["SaleDate"].ToString();
                    model.PieceName = ds.Tables[0].Rows[0]["PieceName"].ToString();
                    model.NoOfPiece = ds.Tables[0].Rows[0]["NoOfPiece"].ToString();
                    model.OriginalPrice = ds.Tables[0].Rows[0]["OriginalPrice"].ToString();
                    model.Discount = ds.Tables[0].Rows[0]["Discount"].ToString();
                    model.FinalPrice = ds.Tables[0].Rows[0]["FinalPrice"].ToString();
                    lstShopSaleOrderDetails.Add(model);
                }
                model.lstshopsaleorder = lstShopSaleOrderDetails;
                ViewBag.FinalPrice = double.Parse(ds.Tables[1].Compute("sum(FinalPrice)", "").ToString()).ToString("n2");
            }

            return View(model);
        }

        //public ActionResult DeleteShopSaleOrder(String SaleOrderId)
        //{
        //    Shop obj = new Shop();
        //    try
        //    {
        //        obj.SaleOrderId = SaleOrderId;
        //        DataSet ds = obj.DeleteShopSaleOrder();
        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {
        //                TempData["ShopSaleOrder"] = "Shop Sale Order Deleted Successfully!";
        //            }
        //            else
        //            {
        //                TempData["ShopSaleOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ShopSaleOrder"] = ex.Message;
        //    }
        //    return RedirectToAction("ShopSaleOrderList", "Shop");
        //}
    }
}