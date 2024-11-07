using BackendAssessment.Interfaces.Services;
using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BackendAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto>> Register([FromBody] RegistrationRequestDto model)
        {

            var responseDto = await _authService.Register(model);
            if (!responseDto.IsSuccess)
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (!loginResponse.IsSuccess)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);

        }
    }
}
