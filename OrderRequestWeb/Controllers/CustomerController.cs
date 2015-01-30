using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using EntityLibrary;
using OrderRequestWeb.Models;

namespace OrderRequestWeb.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
       
        private BusinessLogicLayer.CustomerService CustomerService = new CustomerService();
        
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]

        public ActionResult register()
        {
            OrderRequestWeb.Models.CustomerModel.CountryViewModel Country = new Models.CustomerModel.CountryViewModel();
            ViewData["Country"] = Country.LoadCountries();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(EntityLibrary.Customer Customer)
        {
            if (ModelState.IsValid)
            {
                if (CustomerService.IsCustomerDataValid(Customer))
                {
                    CustomerService.Save(Customer);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Please check your email or other inputs");
                          
            }
            ModelState.AddModelError("","Please put valid information");
            OrderRequestWeb.Models.CustomerModel.CountryViewModel Country = new Models.CustomerModel.CountryViewModel();
            ViewData["Country"] = Country.LoadCountries();
            return View(Customer);
        }

        [Authorize]
        public JsonResult IsEmailExist(string EmailAddress)
        {
           
            if (CustomerService.IsEmailExixt(EmailAddress))
            {
                return Json("Sorry, this name already exists", JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }
        


    }
}
