using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang chủ: Hiển thị danh sách sản phẩm, lọc theo từ khóa hoặc danh mục
        public IActionResult Index(string keyword, int? categoryId)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Name.Contains(keyword));
                ViewBag.Keyword = keyword;
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            ViewBag.Categories = _context.Categories.ToList();

            return View(products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
