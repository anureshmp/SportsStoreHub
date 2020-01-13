using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController:Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService)
        {
            repository = repoService;
           

        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }


        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            cart = GetCart();

            if (cart.Lines().Count() == 0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines().ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));

            }
            else
            {
                return View(order);
            }

            
        }

        public ViewResult Completed()
        {
            cart = GetCart();
            cart.Clear();
            return View();
        }

        public ViewResult List()
        {
            return View(repository.Orders.Where(o => !o.Shipped));
        }

        [HttpPost]
        public IActionResult MarkShipped(int OrderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == OrderID);

            if(order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));

        }


        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
    }
}
