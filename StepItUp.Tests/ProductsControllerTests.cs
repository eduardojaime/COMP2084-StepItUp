using Microsoft.EntityFrameworkCore;
using StepItUp.Controllers;
using StepItUp.Data;
using StepItUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepItUp.Tests
{
    // Make class public and sealed
    // Mark with the [TestClass] attribute
    [TestClass]
    public sealed class ProductsControllerTests
    {
        // Shared variables for the all test methods
        private ProductsController _controller;
        private ApplicationDbContext _context;
        // Initialize test class
        // Create an instance of the in-memory database
        // and an instance of the ProductsController
        // This method runs before each test method
        [TestInitialize]
        public void Initialize()
        {
            // create an instance of the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDB")
                .Options;
            _context = new ApplicationDbContext(options);
            // create mock data and add it to the context
            // categories
            var cat1 = new Category { CategoryId = 1, Name = "Running Shoes" };
            var cat2 = new Category { CategoryId = 2, Name = "Soccer Shoes" };
            // add to context as you would in a real database
            _context.Category.Add(cat1);
            _context.Category.Add(cat2);
            _context.SaveChanges();
            // products
            var prod1 = new Product
            {
                ProductId = 1,
                Name = "Nike Air Zoom Pegasus",
                Description = "Lightweight running shoes",
                Size = "10",
                Price = 120.00m,
                Colour = "Black",
                Photo = "nike-air-zoom.jpg",
                Category = cat1
            };
            var prod2 = new Product
            {
                ProductId = 2,
                Name = "Adidas Predator",
                Description = "High-performance soccer shoes",
                Size = "11",
                Price = 150.00m,
                Colour = "White",
                Photo = "adidas-predator.jpg",
                Category = cat2
            };
            _context.Product.Add(prod1);
            _context.Product.Add(prod2);
            _context.SaveChanges();
            // create an instance of the ProductsController passing the in-memory context
            _controller = new ProductsController(_context);
        }
    }
}
