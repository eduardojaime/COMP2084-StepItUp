using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StepItUp.Data;

namespace StepItUp.Controllers
{
    public class ProductsController : Controller
    {
        // class-level db conn
        private readonly ApplicationDbContext _context;

        // constructor using db conn
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            // fetch list of Categories for parent dropdown list
            var categories = _context.Category.OrderBy(c => c.Name).ToList();
            ViewBag["CategoryId"] = new SelectList(categories, "CategoryId", "Name");

            // show a blank form to enter a new product
            return View();
        }

        public IActionResult Edit()
        {
            // show populated form to edit a product
            return View();
        }

        public IActionResult Delete()
        {
            // show confirmation page before deleting product
            return View();
        }
    }
}
