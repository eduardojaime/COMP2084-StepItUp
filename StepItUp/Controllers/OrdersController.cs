using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StepItUp.Data;
using StepItUp.Models;

namespace StepItUp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Order                       // SELECT * FROM Order
                .Where(o => o.CustomerId == User.Identity.Name)     // WHERE CustomerId = User.Identity.Name
                .OrderByDescending(o => o.DateCreated)              // ORDER BY DateCreated DESC 
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order                        // SELECT * FROM Order
                .Include(o => o.Items)                              // JOIN OrderItem ON Order.OrderId = OrderItem.OrderId
                .ThenInclude(oi => oi.Product)                      // JOIN Product ON OrderItem.ProductId = Product.ProductId
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
