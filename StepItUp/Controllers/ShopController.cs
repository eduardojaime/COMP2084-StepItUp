using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepItUp.Data;
using StepItUp.Models;
using StepItUp.Extensions;
using Stripe.Checkout;
using Stripe;

namespace StepItUp.Controllers
{
    public class ShopController : Controller
    {
        // shared class-level db conn obj
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // << this is injected as a service in Program.cs

        // constructor w/db dependency
        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            var customerId = GetCustomerId(); // placeholder

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

        // GET /Shop/Cart
        public IActionResult Cart()
        {
            // retrieve customerId so I can filter cart items
            var customerId = GetCustomerId();
            // retrieve cart items associated to that customerid
            var cartItems = _context.Cart                                   // SELECT * FROM Cart AS c
                                .Include(c => c.Product)                     // JOIN Product AS p ON c.ProductId = p.ProductId
                                .Where(c => c.CustomerId == customerId)     // WHERE c.CustomerId = @customerId
                                .OrderByDescending(c => c.DateCreated)      // ORDER BY DateCreated DESC
                                .ToList();

            // Calculate total amount for the entire transaction, return as a string formatted as currency
            // ViewBag is a dynamic object that allows you to pass data to the view
            // Property names are not strongly typed, and must match in the view
            ViewBag.TotalAmount = cartItems.Sum(c => (c.Price * c.Quantity)).ToString("C");
            // return a view with the list of cart items
            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int id)
        {
            // find cart item
            var cartItem = _context.Cart.Find(id);
            // delete from context collection
            _context.Cart.Remove(cartItem);
            // save changes
            _context.SaveChanges();
            // redirect to cart
            return RedirectToAction("Cart");
        }

        // GET /Shop/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // This reads a hash in the view that the app users to validate it
        public IActionResult Checkout([Bind("FirstName, LastName, Address, Province, City, PostalCode")] Order order)
        {
            // Order already contains the values for the properties from the model binder
            // Programmatically fill in the values for the Date, Total and CustomerId
            order.DateCreated = DateTime.UtcNow;
            order.CustomerId = GetCustomerId();
            // Calculate total from cart items
            var totalAmount = _context.Cart.Sum(c => (c.Price * c.Quantity));
            order.Total = totalAmount;
            // Store Order Object in Session store
            HttpContext.Session.SetObject("Order", order);
            // Send user to Payment page (that's where the stripe script is executed)
            return RedirectToAction("Payment");
        }

        // GET /Shop/Payment
        [Authorize]
        public IActionResult Payment()
        {
            // Retrieve order total from session storage
            var order = HttpContext.Session.GetObject<Order>("Order");
            // Pass to view in ViewBag
            ViewBag.TotalAmount = order.Total;
            // Also pass Stripe key in ViewBag
            // ????
            // Render View
            return View();
        }


        // POST /Shop/ProcessPayment
        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment()
        {
            // Load Order and Secret Key
            var order = HttpContext.Session.GetObject<Order>("Order");
            StripeConfiguration.ApiKey = _configuration["Payments:Stripe:SecretKey"];

            // Copy code from Create() method in Stripe Quickstart guide
            // https://docs.stripe.com/checkout/quickstart

            var domain = "https://" + Request.Host; // Returns whichever domain this app is running on
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Pass total amount as a single item
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = (order.Total * 100),
                        // UnitAmount = (order.Total * 100) as long?,
                        Currency = "cad",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "StepItUp Online Purchase"
                        }
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                PaymentMethodTypes = new List<string> { "card" },
                SuccessUrl = domain + "/Shop/SaveOrder",
                CancelUrl = domain + "/Shop/Cart",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        // TODO: SaveOrder

        // Helper Methods are Private
        private string GetCustomerId()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            // check session store for value associated to this session
            if (String.IsNullOrEmpty(customerId))
            {
                // there's nothing in the session yet
                // determine whether user is authenticated (email) or not (GUID)
                if (User.Identity.IsAuthenticated)
                {
                    customerId = User.Identity.Name; // email address
                }
                else
                {
                    customerId = Guid.NewGuid().ToString();
                }

                // store whichever customerId got created to session store
                HttpContext.Session.SetString("CustomerId", customerId);
            }

            // return session value
            return customerId;
        }
    }
}
