using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lines = new List<CartLine>();

        public void AddToCart(Product prod)
        {
            CartLine cartLine = lines.FirstOrDefault(m => m.Product.ProductId == prod.ProductId);
            if (cartLine != null)
            {
                cartLine.Quantity++;
            }
            else
            {
                lines.Add(new CartLine{Product = prod, Quantity = 1});
            }
        }

        public void DeleteFromCart(int productId)
        {
            CartLine cartLine = lines.FirstOrDefault(m => m.Product.ProductId == productId);
            if (cartLine != null)
            {
                lines.Remove(cartLine);
            }
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lines; }
        }

        public int Count
        {
            get { return lines.Count; }
        }

        public decimal SumPrice()
        {
            return lines.Sum(p => p.Product.Price * p.Quantity);
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
