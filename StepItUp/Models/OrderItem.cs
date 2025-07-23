namespace StepItUp.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; } // Primary key for the order item
        
        // Navigation property to Order M to 1 relationship
        public int OrderId { get; set; } // Foreign key to Order
        public Order Order { get; set; } // Navigation property to Order

        // Navigation property to Products 1 to 1 relationship
        public int ProductId { get; set; } // Foreign key to Product
        public Product Product { get; set; } // Navigation property to Product
        
        // Price and quantity that user paid for
        public decimal Price { get; set; }
        public int Quantity {  get; set; }
    }
}
