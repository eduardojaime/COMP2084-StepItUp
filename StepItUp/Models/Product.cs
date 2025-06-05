using System.ComponentModel.DataAnnotations;

namespace StepItUp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [MaxLength(3)]
        public string Size { get; set; }

        [Range(0.01,500)]
        public decimal Price { get; set; }

        public string Colour { get; set; }
        public string? Photo { get; set; }

        // FK => reference to Category a Product belongs to
        public int CategoryId { get; set; }

        // parent reference to Category
        public Category Category { get; set; }
    }
}
