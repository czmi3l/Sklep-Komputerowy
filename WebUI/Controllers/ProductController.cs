using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using PagedList;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        private int elementsPerPage = 5;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        // GET: Product
        public ActionResult Index(int page = 1, string category = null)
        {

            ViewBag.OnePageOfProducts  = repository.Products
                .OrderBy(m => m.Category)
                .Where(m => m.Category == category || category == null)
                .ToPagedList(page, elementsPerPage);

            ViewBag.CurrentCategory = category;

            IEnumerable<Product> products = repository.Products
                .OrderBy(m => m.Category)
                .Where(m => m.Category == category || category == null)
                .Skip((page - 1)*elementsPerPage)
                .Take(elementsPerPage);
            return View(products);
        }

        
    }
}