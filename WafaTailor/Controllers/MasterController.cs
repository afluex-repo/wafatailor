using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WafaTailor.Filter;
using WafaTailor.Models;

namespace WafaTailor.Controllers
{
    public class MasterController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShopMaster(string ShopId)
        {
            Master obj = new Master();
            if (ShopId != null)
            {
                obj.ShopId = ShopId;
                DataSet ds = obj.GetShopMaster();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.ShopId = ds.Tables[0].Rows[0]["Pk_ShopId"].ToString();
                    obj.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                    obj.Address = ds.Tables[0].Rows[0]["ShopAddress"].ToString();
                }
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("ShopMaster")]
        [OnAction(ButtonName = "Save")]
        public ActionResult SaveShopMaster(Master model)
        {
            try
            {
                DataSet ds = model.ShopMaster();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["Shop"] = "Shop Saved Successfully";
                    }
                    else
                    {
                        TempData["Shop"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else
                {
                    TempData["Shop"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }

            }
            catch (Exception ex)
            {
                TempData["Shop"] = ex.Message;
            }
            return RedirectToAction("ConfirmRegistration", "Master");
        }

        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        public ActionResult ShopMasterList(Master model)
        {
           List<Master> lst = new List<Master>();
            DataSet ds = model.GetShopMaster();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Master obj = new Master();
                    obj.ShopId = r["Pk_ShopId"].ToString();
                    obj.ShopName = r["ShopName"].ToString();
                    obj.Address = r["ShopAddress"].ToString();
                    //obj.Status = r["Status"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }

        public ActionResult DeleteShopMaster(string ShopId)
        {
            Master obj = new Master();
            try
            {
                obj.ShopId = ShopId;
                DataSet ds = obj.DeleteShopMaster();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Shop"] = "Shop Master Deleted Successfully!";
                    }
                    else
                    {
                        TempData["Shop"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Shop"] = ex.Message;
            }
            return RedirectToAction("ShopMasterList", "Master");
        }
        public ActionResult Material(string MaterialId)
        {
            Master obj = new Master();
            if (MaterialId != null)
            {
                obj.MaterialId = MaterialId;
                DataSet ds = obj.GetShopMaster();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    obj.MaterialId = ds.Tables[0].Rows[0]["Pk_MaterialId"].ToString();
                    obj.MaterialName = ds.Tables[0].Rows[0]["MaterialName"].ToString();
                    obj.MaterialType = ds.Tables[0].Rows[0]["MaterialType"].ToString();
                }
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("Material")]
        [OnAction(ButtonName = "btnsave")]
        public ActionResult SaveMaterial(Master model)
        {
            try
            {
                DataSet ds = model.SaveMaterial();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["Material"] = "Material Saved Successfully";
                    }
                    else
                    {
                        TempData["Material"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else
                {
                    TempData["Material"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }

            }
            catch (Exception ex)
            {
                TempData["Material"] = ex.Message;
            }
            return RedirectToAction("MaterialConfirmRegistration", "Master");
        }

        public ActionResult MaterialConfirmRegistration()
        {
            return View();
        }

        public ActionResult MaterialList(Master model)
        {
            List<Master> lst = new List<Master>();
            DataSet ds = model.GetMaterialDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Master obj = new Master();
                    obj.MaterialId = r["Pk_MaterialId"].ToString();
                    obj.MaterialName = r["MaterialName"].ToString();
                    obj.MaterialType = r["MaterialType"].ToString();
                    //obj.Status = r["Status"].ToString();
                    lst.Add(obj);
                }
                model.lstRegistration = lst;
            }
            return View(model);
        }

        public ActionResult DeleteMaterial(string MaterialId)
        {
            Master obj = new Master();
            try
            {
                obj.MaterialId = MaterialId;
                DataSet ds = obj.DeleteMaterial();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Material"] = "Material Deleted Successfully!";
                    }
                    else
                    {
                        TempData["Material"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Material"] = ex.Message;
            }
            return RedirectToAction("MaterialList", "Master");
        }
    }
}



 