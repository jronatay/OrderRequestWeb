using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLibrary;
using EntityLibrary.OrderModels;
using BusinessLogicLayer;
using System.Web.Routing;

namespace OrderRequestWeb.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private BusinessLogicLayer.OrderService OrderService = new OrderService();
        private EntityLibrary.OrderDAO OrderDAO = new OrderDAO();
        //
        // GET: /Order/
        
        public ActionResult Index()
        {
            Session["model"] =null;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(EntityLibrary.OrderModels.OrderRequestInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (!OrderService.Is_Other_Product_Data_Valid(OrderService.Populated_Order_Product_From_Request(model)))
                {
                    ModelState.AddModelError("", "Put valid info on product name or Description!");
                    return View();
                }
                else if (OrderService.Is_Requested_Stored_In_Temporary_Storage(OrderService.Populated_Order_Product_From_Request(model)))
                    {
                        Session["model"] = model;
                        return RedirectToAction("OrderCheck");
                    }
                    else
                    {
                        ModelState.AddModelError("", "At least 1 Quantity  of product/products needed to process an order");
                        return View();
                    }
                
            }
            ModelState.AddModelError("", "Plesase Enter a valid quantity!");
            return View();
        }

        public ActionResult OrderCheck()
        {
            if (Session["model"] !=null )
            {
                return View(OrderService.ReturnOrderProductsStored(OrderService.Populated_Order_Product_From_Request((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"])));
            }
            return RedirectToAction("Index");
        }

        public ActionResult OrderConfirmation()
        {
            int OrderNo = OrderService.New_Order_Id_and_update_order_number(int.Parse(User.Identity.Name.Split(',')[1]));
            OrderService.SaveOrder((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"], OrderNo);
            return View(OrderService.OrderConfirmation(OrderNo));
        }

        public ActionResult GetList()
        {
            
            return PartialView();
        }


        public ActionResult getproducts()
        {
            return PartialView(OrderDAO.Order_Product_List());
        }
        [HttpPost]
        public JsonResult ProducttoList(string Id)
        {
            var result = OrderService.get_and_set_product_data(int.Parse(Id));
            return Json(result);
        }

       
        public ActionResult OrderOverView()
        {
            return View(OrderDAO.Get_Order_By_Customer_ID(int.Parse(User.Identity.Name.Split(',')[1])));
        }
        
        public ActionResult OrderOverViewDetails(int id)
        {
            return View(OrderService.GetOrderProductsByOrderIdAndCustomerId(id,int.Parse(User.Identity.Name.Split(',')[1])));
        }

    }
}
