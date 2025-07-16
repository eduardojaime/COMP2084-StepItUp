using Microsoft.AspNetCore.Mvc;
using StepItUp.Data;
using StepItUp.Models;

namespace StepItUp.Controllers
{
    public class ShopController : Controller
    {
        // shared class-level db conn obj
        private readonly ApplicationDbContext _context;

        // constructor w/db dependency
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //// make a mock list of categories & pass to view for display
            //var categories = new List<Category>();

            //// populate mock list
            //for (var i = 1; i < 16; i++)
            //{
            //    categories.Add(new Category { CategoryId = i, Name = "Category " + i.ToString() });
            //}

            // fetch categories from db
            var categories = _context.Category.OrderBy(c => c.Name).ToList();

            // show the view and pass it the data list for display
            return View(categories);
        }

        // GET /Shop/Category/id
        public IActionResult Category(int? id)
        {
            // 1 - Retrieve a list of products filtered by id
            var products = _context.Product     // SELECT * FROM Product AS p
                .Where(p => p.CategoryId == id) // WHERE p.CategoryId = @id
                .OrderBy(p => p.Name)           // ORDER BY p.Name
                .ToList();

            // 2 retrieve category name by id and store in ViewData
            var category = _context.Category.Find(id);
            ViewData["Category"] = category.Name;

            // 3 - Return products list to the view
            return View(products);
        }

        // POST /Shop/AddToCart with qty and product id included in the form
        // Use [FromForm] next to parameter datatype to indicate these values come from the HTML form
        [HttpPost]
        public IActionResult AddToCart([FromForm] int ProductId, [FromForm] int Quantity)
        {
            // Who is buying? get userid or generate GUID for this session
            var customerId = "GetCustomerId()"; // placeholder

            // Get product price
            // Find() returns a product object, access Price property too
            var price = _context.Product.Find(ProductId).Price;

            // Create cart record
            var cartItem = new Cart
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = DateTime.UtcNow, // BEST PRACTICE: Always use UTC time in DB
                CustomerId = customerId
            };
            // Add to carts dbset and save changes
            _context.Cart.Add(cartItem);    // stored in-memory
            _context.SaveChanges();         // persisted in database

            // Redirect to carts page to show list of selected items
            return RedirectToAction("Cart");
        }

        // TODO: GET /Shop/Cart

    }
}
