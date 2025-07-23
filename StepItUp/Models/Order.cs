namespace StepItUp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; } // email or GUID
        public decimal Total { get; set; } // total for this order (what's calculated in the cart page)
        public DateTime DateCreated { get; set; }
        // Delivery information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        // Navigation property to Order Details 1 to M relationship
        public List<OrderItem> Items { get; set; }
    }
}
