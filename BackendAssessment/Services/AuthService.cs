namespace BackendAssessment.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IAppDbContext dbContext, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<ResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower().Trim());

            if (user == null)
            {
                return ResponseDto.Failure("Invalid username or password");
            }

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!isValid)
            {
                return ResponseDto.Failure("Invalid username or password");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDTO = new()
            {
                Email = user.Email ?? "",
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber ?? ""
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDTO,
                Token = token
            };

            return ResponseDto.Success(loginResponseDto, "Login successful");
        }

        public async Task<ResponseDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _dbContext.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email ?? "",
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber ?? ""
                    };
                    return ResponseDto.Success(userDto, "Registration Successful");

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ResponseDto.Failure(registrationRequestDto, $"Registration failed: {errors}");

                }

            }
            catch (Exception ex)
            {
                return ResponseDto.Failure(registrationRequestDto, ex.Message, ex);
            }
        }
    }
}
