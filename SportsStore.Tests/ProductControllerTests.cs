using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using SportsStore.Models;
using SportsStore.Controllers;
using Moq;
using SportsStore.Models.ViewModels;

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
    }
}
