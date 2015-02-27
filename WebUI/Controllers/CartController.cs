using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
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
        public ViewResult Index(Cart cart ,string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Product");
            return View(cart.Lines);
        }


        public RedirectToRouteResult AddToCart(Cart cart,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            cart.AddToCart(product);
            return RedirectToAction("Index", new{returnUrl});
        }

        public RedirectToRouteResult DeleteFromCart(Cart cart, int productId)
        {
            cart.DeleteFromCart(productId);
            return RedirectToAction("Index");
        }

        public PartialViewResult Navbar(Cart cart)
        {
            return PartialView(cart.Lines);
        }

        public ActionResult Order(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            OrderDetails order = new OrderDetails();
            return View(order);
        }

        [HttpPost]
        public ViewResult Order(Cart cart ,OrderDetails order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk nie może być pusty");
            }

            if (ModelState.IsValid)
            {
                return View("Confirmation", new SessionAndOrderDetails{Cart = cart, OrderDetails = order});
            }

            return View(order);
        }

        [HttpPost]
        public ViewResult Confirmation(Cart cart, SessionAndOrderDetails session)
        {
            emailSend.SendEmail(session.OrderDetails, cart);
            Session["cart"] = null;
            return View("SentEmail");
        }
    }
}