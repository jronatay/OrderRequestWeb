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
    public class OrderController : Controller
    {
        private BusinessLogicLayer.OrderService OrderService = new OrderService();
        //
        // GET: /Order/
        
        public ActionResult Index()
        {
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
                    ModelState.AddModelError("", "At Least One Product with at least 1 Quantity needed to Process Order");
                    return View(OrderService.OrderProductInputList());
                }
            }
            return View(OrderService.OrderProductInputList());
        }

        public ActionResult OrderCheck()
        {
            
            return View( OrderService.ReturnOrderProductsStored(OrderService.PopulatedOrderProductFromRequest((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"])));
        }
        public ActionResult OrderConfirmation()
        {
            int OrderNo=OrderService.NewOrder(int.Parse(User.Identity.Name.Split(',')[1]));
            OrderService.SaveOrder((EntityLibrary.OrderModels.OrderRequestInputModel)Session["model"], OrderNo);

            return View(OrderService.OrderConfirmation(OrderNo));
        }

    }
}
