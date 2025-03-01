using System.IdentityModel.Tokens.Jwt;

public class TokenInterceptorMiddleware
{
    private readonly RequestDelegate _next;

    public TokenInterceptorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            AttachTokenPayloadToContext(context, token);
        }

        await _next(context);
    }

    private void AttachTokenPayloadToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken != null)
            {
                context.Items["TokenPayload"] = jwtToken.Payload;
            }
        }
        catch
        {
            // Do nothing if token validation fails
        }
    }
}