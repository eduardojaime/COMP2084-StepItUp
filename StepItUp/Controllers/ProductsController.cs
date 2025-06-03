using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StepItUp.Data;
using StepItUp.Models;

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
            // fetch product list from db. include join to parent Category
            var products = _context.Product.OrderBy(p => p.Name).Include(p => p.Category).ToList();

            // load view and pass product list
            return View(products);
        }

        // GET: load empty product form including dropdown of categories
        public IActionResult Create()
        {
            // fetch list of Categories for parent dropdown list
            var categories = _context.Category.OrderBy(c => c.Name).ToList();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");

            // show a blank form to enter a new product
            return View();
        }

        // POST: process create form submission to save new product
        [HttpPost]
        public IActionResult Create([Bind("Name,Description,Size,Price,Colour,Photo,CategoryId")] Product product)
        {
            // save new product to db
            _context.Product.Add(product);
            _context.SaveChanges();

            // redirect to products Index to see updated list
            return RedirectToAction("Index");
        }

        // GET: populate edit form so we can modify a product
        public IActionResult Edit(int id)
        {
            // fetch selected product from db
            var product = _context.Product.Find(id);

            // fetch category list & select right category for dropdown
            ViewData["CategoryId"] = new SelectList(_context.Category.OrderBy(c => c.Name).ToList(),
                "CategoryId", "Name", product.CategoryId);

            // show populated form to edit a product
            return View(product);
        }

        // POST: update selected product w/changes from form
        [HttpPost]
        public IActionResult Edit(int id, [Bind("ProductId,Name,Description,Size,Price,Colour,Photo,CategoryId")] Product product)
        {
            // save new product to db
            _context.Product.Update(product);
            _context.SaveChanges();

            // redirect to products Index to see updated list
            return RedirectToAction("Index");
        }

        // GET: // show delete confirmation page w/product details
        public IActionResult Delete(int id)
        {
            // fetch selected product
            var product = _context.Product.Find(id);

            // show confirmation page before deleting product
            return View(product);
        }

        // POST: remove selected product
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // fetch selected product
            var product = _context.Product.Find(id);

            // remove from
            _context.Product.Remove(product);
            _context.SaveChanges();

            // redirect to products Index to see updated list
            return RedirectToAction("Index");
        }
    }
}
