using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }
        public ViewResult List() =>
            View(repository.Orders.Where(o => !o.Shipped));

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public ViewResult CheckOut() => View(new Order());

        [HttpPost]
        public IActionResult CheckOut(Order order)
        {
            if(cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }
            if(ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return Redirect(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}