using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Infrastructure;

namespace SportsStore.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
       
        public IViewComponentResult Invoke()
        {
            return View(GetCart());
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

    }
}
