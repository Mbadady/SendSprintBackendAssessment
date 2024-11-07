namespace BackendAssessment.Models.DTOs.Auth
{
    public class LoginResponseDto
    {
        public required UserDto User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
