namespace StepItUp.Models
{
    
    public class Cart
    {
        public int CartId { get; set; }
        // we'll use customerId to identify items that a user wants to buy
        // values can be an email address (username) or GUID (unauthenticated user)
        public string CustomerId { get; set; }
        // Link 1 to 1 to chosen Products by id
        public int ProductId { get; set; } // product id
        public Product Product { get; set; } // navigation property
        // Additional information: price, qty, etc...
        public decimal Price { get; set; } // Unit price
        public int Quantity { get; set; } // Quantity to calculate total for this item
        public DateTime DateCreated { get; set; }
    }
}
