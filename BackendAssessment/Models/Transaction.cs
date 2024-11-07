using BackendAssessment.Util.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendAssessment.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public string StatusDesc { get; set; } = string.Empty;
        public PaymentMethod PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
