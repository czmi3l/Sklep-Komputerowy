using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class ProductsDb : IProductRepository
    {
        private EFDbContext _efDbContext = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return _efDbContext.Products; }
        }
    }
}
