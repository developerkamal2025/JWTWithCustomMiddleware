using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTWithCustomMiddleware.Authentication
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute :Attribute{}
}
