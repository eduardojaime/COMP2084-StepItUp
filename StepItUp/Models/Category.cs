namespace StepItUp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // child ref to Product model: 1 category has many products
        public List<Product>? Products { get; set; }
    }
}
