using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebUI.Infrastructure;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IEmailSend emailSend;

        public CartController(IProductRepository repo, IEmailSend email)
        {
            repository = repo;
            emailSend = email;
        }

        // GET: Cart
        public ViewResult Index(Cart cart, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Product");
            return View(cart.Lines);
        }


        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            cart.AddToCart(product);
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult DeleteFromCart(Cart cart, int productId)
        {
            cart.DeleteFromCart(productId);
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public PartialViewResult Navbar(Cart cart)
        {
            return PartialView(cart.Lines);
        }

        public ActionResult Order(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            AppUser user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.GetOwinContext()
                        .GetUserManager<AppUserManager>()
                        .FindByName(HttpContext.User.Identity.Name);
                if (user == null)
                {
                    user = new AppUser();
                }
            }
            else
            {
                user = new AppUser();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Order(Cart cart, OrderDetails order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk nie może być pusty");
            }

            if (ModelState.IsValid)
            {
                return View("Confirmation", new SessionAndOrderDetails { Cart = cart, OrderDetails = order });
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Confirmation(Cart cart, SessionAndOrderDetails session)
        {
            emailSend.SendEmail(session.OrderDetails, cart);
            Session["cart"] = null;
            return View("SentEmail");
        }
    }
}