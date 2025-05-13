using Microsoft.AspNetCore.Mvc;

namespace StepItUp.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            // make a mock list of categories & pass to view for display


            return View();
        }
    }
}
