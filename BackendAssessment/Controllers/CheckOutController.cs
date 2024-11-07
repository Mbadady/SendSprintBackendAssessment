using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BackendAssessment.Controllers
{
    [ApiController]
    [Route("checkout")]
    public class CheckOutController : ControllerBase
    {
        private readonly ICheckOutService _checkoutService;

        public CheckOutController(ICheckOutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // POST /checkout
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckOutRequest checkoutRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User email not found.");
            }

            var paymentResponse = await _checkoutService.ProcessCheckoutAsync(checkoutRequest, email);
            return Ok(paymentResponse);
        }
    }
}
