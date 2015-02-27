using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Binders
{
    public class CartBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = (Cart)controllerContext.HttpContext.Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session["Cart"] = cart;
            }
            return cart;
        }
    }
}