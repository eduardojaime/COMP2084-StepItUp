using Microsoft.AspNetCore.Mvc;
using StepItUp.Models;

namespace StepItUp.Controllers
{
    public class CategoriesController : Controller
    {
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

            // redirect to category list
            return RedirectToAction("Index");
        }
    }
}
