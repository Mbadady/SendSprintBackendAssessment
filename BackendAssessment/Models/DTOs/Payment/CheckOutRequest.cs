

namespace BackendAssessment.Models.DTOs.Payment;

public class CheckOutRequest
{
    public List<ProductQuantity> Products { get; set; } = []; // List of products and their quantities
}

public class ProductQuantity
{
    [Required]
    public int ProductId { get; set; } // ID of the product
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } // Quantity of the product
}
