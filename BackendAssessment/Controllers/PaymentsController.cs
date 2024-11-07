

using Newtonsoft.Json;

namespace BackendAssessment.Controllers
{
    [ApiController]
    [Route("/payment/webhook")]
    public class PaymentsController(IWebhookService webhookService, IConfiguration configuration) : ControllerBase
    {
        private readonly string _secretKey = configuration["Payment:PaystackSK"];


        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            // Get the x-paystack-signature header
            var xPaystackSignature = Request.Headers["x-paystack-signature"].ToString();

            // Compute the HMAC hash
            var computedSignature = ComputeHmacSha512(requestBody, _secretKey);

            // Verify the signature
            if (computedSignature.Equals(xPaystackSignature, StringComparison.OrdinalIgnoreCase))
            {
                // Process the webhook request
                var webhookRequest = JsonConvert.DeserializeObject<WebhookRequest>(requestBody);

                await webhookService.HandleWebhookAsync(webhookRequest!);
                return Ok();
            }
            else
            {
                // Invalid signature
                return Unauthorized("Invalid signature.");
            }
        }

        private static string ComputeHmacSha512(string input, string key)
        {
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using var hmac = new HMACSHA512(secretkeyBytes);
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashValue).Replace("-", string.Empty);
        }
    }
}
