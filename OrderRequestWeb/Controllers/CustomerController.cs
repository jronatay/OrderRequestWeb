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
        private OrderRequestWeb.Models.CustomerModel.CountryViewModel Country = new Models.CustomerModel.CountryViewModel();
        
      

        [AllowAnonymous]

        public ActionResult register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Order");
            }
            else
            {
                ViewData["Country"] = Country.LoadCountries();
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(EntityLibrary.Customer Customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CustomerService.IsEmailExixt(Customer))
                    {
                        ModelState.AddModelError("EmailAddress", "Email already exist!");

                    }
                    else
                    {
                        if (CustomerService.IsCustomerDataValid(Customer))
                        {
                            CustomerService.Save(Customer);
                            return RedirectToAction("register");
                        }
                        ModelState.AddModelError("", "Please put valid information");
                    }

                }

                ModelState.AddModelError("", "Please put valid information");
                ViewData["Country"] = Country.LoadCountries();
                return View(Customer);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Please put valid information");
                ViewData["Country"] = Country.LoadCountries();
                return View(Customer);
            }
        }

        
        


    }
}
