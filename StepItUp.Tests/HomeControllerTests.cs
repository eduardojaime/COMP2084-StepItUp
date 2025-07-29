using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StepItUp.Controllers;

namespace StepItUp.Tests
{
    // Make class public and sealed
    // Mark it with the TestClass attribute
    [TestClass]
    public sealed class HomeControllerTests
    {
        // You should have at least one test method per action method in the controller
        [TestMethod]
        public void IndexReturnsViewResult()
        {
            // Arrange > initialize controller class
            var controller = new HomeController();

            // Act > call the index() action method
            var result = controller.Index();

            // Assert > verify that result is of type ViewResult
            Assert.IsInstanceOfType<ViewResult>(result);
        }
    }
}
