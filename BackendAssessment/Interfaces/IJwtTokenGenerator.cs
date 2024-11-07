using BackendAssessment.Models;

namespace BackendAssessment.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
