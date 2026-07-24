using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Extensions;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        // GET: Hiển thị trang nhập thông tin giao hàng
        public IActionResult Index()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

        // POST: Lưu thông tin đơn hàng và chi tiết đơn hàng vào DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Order orderForm)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValid)
            {
                // 1. Tạo đơn hàng mới
                var order = new Order
                {
                    CustomerName = orderForm.CustomerName,
                    Phone = orderForm.Phone,
                    Address = orderForm.Address,
                    TotalAmount = cart.Sum(c => c.Total),
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
                _context.SaveChanges(); // Lưu Order trước để tự động tạo Order.Id

                // 2. Tạo danh sách OrderDetail từ giỏ hàng
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    };
                    _context.OrderDetails.Add(orderDetail);
                }

                _context.SaveChanges(); // Lưu các OrderDetails

                // 3. Xóa Session giỏ hàng sau khi đặt hàng thành công
                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Success");
            }

            return View(orderForm);
        }

        // Trang thông báo đặt hàng thành công
        public IActionResult Success()
        {
            return View();
        }
    }
}
