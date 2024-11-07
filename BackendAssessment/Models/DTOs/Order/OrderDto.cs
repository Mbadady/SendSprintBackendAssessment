using BackendAssessment.Util.Enums;

namespace BackendAssessment.Models.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; } = string.Empty;
        public int? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
