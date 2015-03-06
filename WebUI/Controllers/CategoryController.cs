using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private IProductRepository repository;

        public CategoryController(IProductRepository repo)
        {
            repository = repo;
        }
        // GET: Category
        [ChildActionOnly]
        public PartialViewResult Index(string category = null)
        {
            ViewBag.CurrentCategory = category;

            IEnumerable<string> categories = repository.Products
                .Select(m => m.Category)
                .Distinct()
                .OrderBy(m => m);
            

            return PartialView(categories);
        }
    }
}