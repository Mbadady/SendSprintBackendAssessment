using BackendAssessment.Models;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Payment;

namespace BackendAssessment.Interfaces.Services
{
    public interface ICheckOutService
    {
        Task<ResponseDto> ProcessCheckoutAsync(CheckOutRequest checkOutRequest, string email, CancellationToken cancellation = default);
    }
}
