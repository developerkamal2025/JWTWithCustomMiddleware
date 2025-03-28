using System.Linq;
using System.Security.Claims;
using JWTWithCustomMiddleware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTWithCustomMiddleware.Authentication
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _Roles;
        public AuthorizeAttribute(params Role[] Roles)
        {
            _Roles = Roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = _Roles.ToList()?.FirstOrDefault();

            bool actionDescriptor = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

            if (actionDescriptor)
                return;

            var claims = (List<Claim>)context.HttpContext.Items["Claims"];
            var userRoles = claims?.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

            if (claims == null || (roles != null && !(roles).ToString().Equals(userRoles)))
            {
                context.Result = new JsonResult(new { Message = "Unathorized", StatusCode = StatusCodes.Status401Unauthorized });
            }
        }
    }
}
