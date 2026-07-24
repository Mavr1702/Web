using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using System.Linq;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách đơn hàng đã đặt
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
