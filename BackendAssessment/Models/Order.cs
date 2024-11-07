using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Util.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BackendAssessment.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Json string of the list of orderItems
        /// </summary>
        public string OrderItemsJson { get; set; } = string.Empty;

        // This property is used to hold the deserialized order items
        [NotMapped]
        public List<OrderItem> OrderItems
        {
            get => string.IsNullOrEmpty(OrderItemsJson)
                ? []
                : JsonSerializer.Deserialize<List<OrderItem>>(OrderItemsJson) ?? [];
            set => OrderItemsJson = JsonSerializer.Serialize(value);
        }
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; } = string.Empty;
        public int? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
