﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController:Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }


        public ViewResult List(int ProductPage = 1)
        {
            return View
                (
                new ProductsListViewModel
                {
                    Products = repository.Products
                                .OrderBy(p => p.ProductID)
                                .Skip((ProductPage - 1) * PageSize)
                                .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = ProductPage,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.Products.Count()
                    }
                                

                }
                
                );
        }
    }
}