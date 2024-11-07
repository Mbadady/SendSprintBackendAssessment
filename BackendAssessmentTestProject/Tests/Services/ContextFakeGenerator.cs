using BackendAssessment.Models;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Tests.Services
{
    public static class ContextFakeGenerator
    {
        public static List<Product> GetProducts()
        {
            return [
                new Product
           {
            ProductId = 1,
            Price = 10,
            Quantity = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "Laptop",
            Name = "Lenovo"
           },
        new Product
        {
            ProductId = 1,
            Price = 10,
            Quantity = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "Laptop",
            Name = "Lenovo"
        },
        new Product
        {
            ProductId = 1,
            Price = 10,
            Quantity = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "Laptop",
            Name = "Lenovo"
        }

            ];
        }

        public static List<Order> GetOrders()
        {
            return [
                new Order
           {
            Id = 1,
            TotalAmount = 10,
            PaymentStatus = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OrderItemsJson = "Lenovo",
            TransactionId = 1
           },
        new Order
        {
            Id = 1,
            TotalAmount = 10,
            PaymentStatus = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OrderItemsJson = "Lenovo",
            TransactionId = 1
        },
        new Order
        {
            Id = 1,
            TotalAmount = 10,
            PaymentStatus = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OrderItemsJson = "Lenovo",
            TransactionId = 1
        }

            ];
        }
        public static List<Transaction> GetTransactions()
        {
            return [
                new Transaction
           {
            Id = 1,
            Amount = 10,
            Currency = "NGN",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PaymentGateway = PaymentMethod.Paystack,
            PaymentReference = Guid.NewGuid().ToString(),
            Status = PaymentStatus.Pending,
            StatusDesc = "Pending",
           },
        new Transaction
        {
            Id = 1,
            Amount = 10,
            Currency = "NGN",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PaymentGateway = PaymentMethod.Paystack,
            PaymentReference = Guid.NewGuid().ToString(),
            Status = PaymentStatus.Pending,
            StatusDesc = "Pending",
        },
        new Transaction
        {
            Id = 1,
            Amount = 10,
            Currency = "NGN",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PaymentGateway = PaymentMethod.Paystack,
            PaymentReference = Guid.NewGuid().ToString(),
            Status = PaymentStatus.Pending,
            StatusDesc = "Pending",
        }

            ];
        }
    }
}
