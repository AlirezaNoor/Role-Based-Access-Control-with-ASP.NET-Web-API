using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RoleBasedAuthSample.Middleware;

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class TokenValidationMiddleware : IMiddleware
{
    private readonly IConfiguration _configuration;

    public TokenValidationMiddleware(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // اگر درخواست برای ساخت یوزر است، Middleware را عبور دهید/
        if (context.Request.Path.StartsWithSegments("/auth/register") ||context.Request.Path.StartsWithSegments("/Auth/Login")  )
        {
            await next(context);
            return;
        }

        // اگر درخواست برای ساخت یوزر نیست، توکن را بررسی کنید
        if (!TokenIsValid(context.Request.Headers["Authorization"]))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        // توکن معتبر است، ادامه دهید
        await next(context);
    }

    private bool TokenIsValid(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        // Assuming JWT token format: "Bearer {token}"
        var parts = token.Split(' ');
        if (parts.Length != 2 || parts[0] != "Bearer")
            return false;

        // Get the token string
        var tokenString = parts[1];

        try
        {
            // Decode and validate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
            tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return true;
        }
        catch (Exception)
        {
            // Token is not valid (malformed, expired, or invalid signature)
            return false;
        }
    }
}
