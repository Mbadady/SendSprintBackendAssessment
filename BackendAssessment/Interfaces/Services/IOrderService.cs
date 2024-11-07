﻿using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Order;
using BackendAssessment.Util.Enums;

namespace BackendAssessment.Interfaces.Services
{
    public interface IOrderService
    {
        Task<ResponseDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseDto> GetAllOrdersAsync(string? searchTerm, int? skip, int? take, CancellationToken cancellationToken = default);
        Task<ResponseDto> AddOrderAsync(OrderDto order, CancellationToken cancellationToken = default);
        Task<ResponseDto> UpdateOrderAsync(int id, OrderDto order, CancellationToken cancellationToken = default);
        Task<ResponseDto> UpdateOrderTransactionIdAsync(OrderDto order, CancellationToken cancellationToken = default);
        Task<ResponseDto> DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
    }
}
