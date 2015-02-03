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
        //
        // GET: /Order/
        
        public ActionResult Index()
        {
            Session["model"] =" ";
            return View(OrderService.OrderProductInputList());
        }

        [HttpPost]
        public ActionResult Index(EntityLibrary.OrderModels.OrderRequestInputModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (OrderService.IsRequestedStoredInTemporaryStorage(OrderService.PopulatedOrderProductFromRequest(model)))
                {
                    
                    Session["model"] = model;
                    return RedirectToAction("OrderCheck");
                }
                else
                {
                    ModelState.AddModelError("", "At least 1 Quantity  of product/products needed to process an order");
                    return View(OrderService.OrderProductInputList());
                }
            }
            ModelState.AddModelError("", "Plesase Enter a valid quantity!");
            return View(OrderService.OrderProductInputList());
        }

        public ActionResult OrderCheck()
        {
            if (Session["model"] !=null)
            {
                return View(OrderService.ReturnOrderProductsStored(OrderService.PopulatedOrderProductFromRequest((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"])));
            }
            return RedirectToAction("Index");
        }

        public ActionResult OrderConfirmation()
        {
            int OrderNo=OrderService.NewOrder(int.Parse(User.Identity.Name.Split(',')[1]));
            OrderService.SaveOrder((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"], OrderNo);
            return View(OrderService.OrderConfirmation(OrderNo));
        }
        
        public ActionResult OrderOverView()
        {
            return View(OrderService.GetOrderByCustomerID(int.Parse(User.Identity.Name.Split(',')[1])));
        }
        
        public ActionResult OrderOverViewDetails(int id)
        {
            return View(OrderService.GetOrderProductsByOrderIdAndCustomerId(id,int.Parse(User.Identity.Name.Split(',')[1])));
        }

    }
}
