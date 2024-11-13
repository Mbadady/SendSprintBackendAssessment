namespace BackendAssessment.Models.DTOs.Payment;

public class WebhookRequest
{
    public string Event { get; set; } = string.Empty;// The type of event (e.g., "charge.success", "charge.failed")
    public Data? Data { get; set; }
}
public class Data
{
    public string Reference { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
