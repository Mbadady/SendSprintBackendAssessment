namespace BackendAssessment.Interfaces.Services
{
    public interface IWebhookService
    {
        Task HandleWebhookAsync(WebhookRequest webhookRequest);
    }
}
