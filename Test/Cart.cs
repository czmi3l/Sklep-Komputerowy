using System;
using System.Linq;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //arrange
            Cart cart = new Cart();
            Product prod1 = new Product{ProductId = 1, Name = "Prod1"};
            Product prod2 = new Product{ProductId = 2, Name = "Prod2"};

            //act
            cart.AddToCart(prod1);
            cart.AddToCart(prod2);
            CartLine[] cartLines = cart.Lines.ToArray();


            //assert
            Assert.AreEqual(cart.Count, 2);
            Assert.AreEqual(cartLines[0].Product.Name, "Prod1");
            Assert.AreEqual(cartLines[1].Product.Name, "Prod2");
        }

        [TestMethod]
        public void Can_Increment_Quantity()
        {
            //arrange
            Cart cart = new Cart();
            Product prod1 = new Product { ProductId = 1, Name = "Prod1" };
            Product prod2 = new Product { ProductId = 1, Name = "Prod1" };

            //act
            cart.AddToCart(prod1);
            cart.AddToCart(prod2);
            CartLine[] cartLines = cart.Lines.ToArray();

            //assert
            Assert.AreEqual(cart.Count, 1);
            Assert.AreEqual(cartLines[0].Product.Name, "Prod1");
            Assert.AreEqual(cartLines[0].Quantity, 2);
        }

        [TestMethod]
        public void Can_Delete_CartLine()
        {
            //arrange
            Cart cart = new Cart();
            Product prod1 = new Product { ProductId = 1, Name = "Prod1" };
            Product prod2 = new Product { ProductId = 2, Name = "Prod2" };

            //act
            cart.AddToCart(prod1);
            cart.AddToCart(prod2);
            cart.DeleteFromCart(prod1.ProductId);
            CartLine[] cartLines = cart.Lines.ToArray();

            //assert
            Assert.AreEqual(cart.Count, 1);
            Assert.AreEqual(cartLines[0].Product.Name, "Prod2");
        }
    }
}
