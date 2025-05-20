using Microsoft.AspNetCore.Mvc;

namespace StepItUp.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
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
