using System.Security.Claims;

namespace JWTWithCustomMiddleware.Authentication
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext _context, IJWTToken _jwtToken)
        {
            var token = _context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) {
                List<Claim> claims = await _jwtToken.ValidateJWT(token);

                if (claims!= null && claims.Count() > 0)
                {
                    _context.Items["Claims"] = claims;
                }
            }
            await _next(_context);
        }
    }
}
