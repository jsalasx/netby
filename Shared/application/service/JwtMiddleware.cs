using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Shared.Application.Service;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _secret;

    public JwtMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _secret = Environment.GetEnvironmentVariable("JWT_SECRET") 
                  ?? config["Jwt:Secret"] 
                  ?? throw new ArgumentNullException("JWT_SECRET no configurado");
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,  // puedes activar si quieres validar issuer
                    ValidateAudience = false, // puedes activar si quieres validar aud
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // sin tolerancia
                }, out SecurityToken validatedToken);

                // Opcional: guardar claims en HttpContext para usarlos en los controladores
                var jwtToken = (JwtSecurityToken)validatedToken;
                context.Items["User"] = jwtToken.Claims
                    .ToDictionary(c => c.Type, c => c.Value);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("❌ Token inválido o expirado");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("❌ Token requerido");
            return;
        }

        // pasa al siguiente middleware
        await _next(context);
    }
}
