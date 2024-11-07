namespace BackendAssessment.Models.DTOs.Payment
{
    public class PaymentResponse
    {
        public string AuthorizationUrl { get; set; } = string.Empty; // URL to redirect the user for payment
        public string PaymentReference { get; set; } = string.Empty;
    }
}
