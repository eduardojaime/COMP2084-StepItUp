using Microsoft.AspNetCore.Mvc;
using StepItUp.Data;
using StepItUp.Models;

namespace StepItUp.Controllers
{
    public class CategoriesController : Controller
    {
        // db connection instance at class level => available to all methods in controller
        private readonly ApplicationDbContext _context;

        // constructor => when instance of controller created, receive db connection instance
        // DbContext is a dependency of the controller
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // make a mock list of categories & pass to view for display
            var categories = new List<Category>();

            // populate mock list
            for (var i = 1; i < 16; i++)
            {
                categories.Add(new Category { CategoryId = i, Name = "Category " + i.ToString() });
            }

            // show the view and pass it the mock data list for display
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name")] Category category)
        {
            // use db connection to save new category
            _context.Category.Add(category);
            _context.SaveChanges();

            // redirect to category list
            return RedirectToAction("Index");
        }
    }
}
