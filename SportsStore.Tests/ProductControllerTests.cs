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
    public class UnitTest1
    {
        [Fact]
        public void ProductControllerTests()
        {
         

        }

        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}

            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            IEnumerable<Product> result =
                controller.List(null,2).ViewData.Model as IEnumerable<Product>;

            Product[] prodArray = result.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);


        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                (new Product[]
                {
                    new Product {ProductID = 1,Name = "P1"},
                    new Product {ProductID = 1,Name = "P2"},
                    new Product {ProductID = 1,Name = "P3"},
                    new Product {ProductID = 1,Name = "P4"},
                    new Product {ProductID = 1,Name = "P5"}
                }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            PagingInfo pageinfo = result.PagingInfo;

            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.TotalItems);
            Assert.Equal(2, pageinfo.TotalPages());

        }

        [Fact]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products)
                .Returns((new Product[]
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "P4", Category = "Cat2"}

                }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Product[] result = (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel)
                                .Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");

        }

        [Fact]
        public void Can_Select_Categories()
        {

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"}

            }).AsQueryable<Product>());

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            string[] results = ((IEnumerable<string>)(target.Invoke()
            as ViewComponentResult).ViewData.Model).ToArray();


            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples",
            "Oranges", "Plums" }, results));



        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            string CategoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"}
            }

                ).AsQueryable<Product>());

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = CategoryToSelect;
            // Action
            string result = (string)(target.Invoke() as
            ViewViewComponentResult).ViewData["SelectedCategory"];
            // Assert
            Assert.Equal(CategoryToSelect, result);


        }
    }
}
