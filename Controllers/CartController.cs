using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Extensions;
using Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đọc giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetObject("Cart", cart);
            TempData["Success"] = $"Đã thêm {product.Name} vào giỏ hàng!";
            return RedirectToAction("Index", "Home");
        }

        // Cập nhật số lượng sản phẩm
        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                    TempData["Success"] = "Đã cập nhật số lượng!";
                }
                else
                {
                    cart.Remove(cartItem);
                    TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
                }
                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetObject("Cart", cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            }

            return RedirectToAction("Index");
        }
    }
}
