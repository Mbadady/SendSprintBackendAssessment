using BackendAssessment.Models.DTOs;
using BackendAssessment.Models.DTOs.Auth;

namespace BackendAssessment.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
