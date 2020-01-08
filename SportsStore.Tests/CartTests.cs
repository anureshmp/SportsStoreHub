using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using SportsStore.Models;
using SportsStore.Controllers;
using SportsStore.Components;
using Moq;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] results = target.Lines().ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);

        }

        [Fact]

        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 10);
            target.AddItem(p1, 20);

            CartLine[] results = target.Lines().OrderBy(c => c.Product.ProductID).ToArray();


            Assert.Equal(2, results.Length);
            Assert.Equal(21, results[0].Quantity);
            Assert.Equal(10, results[1].Quantity);

        }

        [Fact]
        public void Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            Product p4 = new Product { ProductID = 4, Name = "P4" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            Assert.Equal(0, target.Lines().Where(c => c.Product == p2).Count());
            Assert.Equal(2, target.Lines().Count());


        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart Target = new Cart();

            Target.AddItem(p1, 1);
            Target.AddItem(p2, 1);
            Target.AddItem(p1, 3);

            decimal result = Target.ComputeTotalValue();

            Assert.Equal(450M, result);

        }

        [Fact]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();

            Assert.Equal(0, target.Lines().Count());

        }

    }
}
