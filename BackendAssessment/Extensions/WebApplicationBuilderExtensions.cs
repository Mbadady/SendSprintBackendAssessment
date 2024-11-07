

namespace BackendAssessment.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;

            var secret = jwtOptions.Secret;

            var issuer = jwtOptions.Issuer;

            var audience = jwtOptions.Audience;


            var key = Encoding.ASCII.GetBytes(secret!);
            Log.Information($"{key}");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true
                };
            });

            return builder;
        }
    }
}
