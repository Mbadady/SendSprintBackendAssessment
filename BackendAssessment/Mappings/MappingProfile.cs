using AutoMapper;
using BackendAssessment.Models;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Models.DTOs.Product;
using BackendAssessment.Models.DTOs.Transaction;
using System.Text.Json;

namespace BackendAssessment.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => DeserializeOrderItems(src.OrderItemsJson)));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderItemsJson, opt => opt.MapFrom(src => SerializeOrderItems(src.OrderItems)));
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, CreateTransactionDto>().ReverseMap();
            CreateMap<Transaction, UpdateTransactionDto>().ReverseMap();
        }

        private static List<OrderItem> DeserializeOrderItems(string json)
        {
            return string.IsNullOrEmpty(json)
                ? []
                : JsonSerializer.Deserialize<List<OrderItem>>(json) ?? [];
        }

        private static string SerializeOrderItems(List<OrderItem> orderItems)
        {
            return JsonSerializer.Serialize(orderItems);
        }
    }
}
