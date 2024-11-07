namespace BackendAssessment.Models.DTOs.Order
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; // Name of the product
        public decimal Price { get; set; } // Price of the product
        public int Quantity { get; set; } // Quantity of the product ordered
    }
}