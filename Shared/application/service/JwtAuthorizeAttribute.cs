namespace Shared.Application.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new JsonResult(new { message = "❌ Token requerido" }) 
            { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        try
        {
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET") 
                         ?? context.HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Secret"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            context.HttpContext.Items["User"] = jwtToken.Claims
                .ToDictionary(c => c.Type, c => c.Value);
        }
        catch
        {
            context.Result = new JsonResult(new { message = "❌ Token inválido o expirado" }) 
            { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
