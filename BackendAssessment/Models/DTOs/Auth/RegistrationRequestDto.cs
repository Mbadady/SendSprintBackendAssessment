﻿namespace BackendAssessment.Models.DTOs.Auth
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
