namespace BackendAssessment.Interfaces.Services
{
    public interface ICheckOutService
    {
        Task<ResponseDto> ProcessCheckoutAsync(CheckOutRequest checkOutRequest, string email, CancellationToken cancellation = default);
    }
}
