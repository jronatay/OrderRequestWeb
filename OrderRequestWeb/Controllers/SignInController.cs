using OrderRequestWeb.Filters;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using EntityLibrary;
using BusinessLogicLayer;

namespace OrderRequestWeb.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class SignInController : Controller
    {
        private BusinessLogicLayer.CustomerService CustomerService = new CustomerService();
        //
        // GET: /SignIn/
         [AllowAnonymous]

        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
            
        }
         [HttpPost]
         [AllowAnonymous]
         [ValidateAntiForgeryToken]
         public ActionResult Index(EntityLibrary.CustomerModels.SignInInputModel user)
         {
            if (ModelState.IsValid && CustomerService.IsCustomerSignInExist(user))
             {
               
               
                 FormsAuthentication.SetAuthCookie(CustomerService.LoggedInUser(user), false);

                 return RedirectToAction("Index", "Order");

             }
             return View(user);
         }
         [Authorize]
         public ActionResult signout()
         {
             FormsAuthentication.SignOut();
             return RedirectToAction("Index", "Home");

         }

    }
}
