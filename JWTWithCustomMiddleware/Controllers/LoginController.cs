using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWTWithCustomMiddleware.Authentication;
using JWTWithCustomMiddleware.Services;

namespace WebAPIWithJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTToken _token;

        public LoginController(IUserService userService, IJWTToken token) {
            _userService = userService;
            _token = token;
        }

        [HttpGet("Login")]
        public async Task<ActionResult> Login(string userName, string password)
        {
            try
            {
                var user = await _userService.Login(userName, password);

                if (user != null)
                {
                    var token = await _token.GenerateJWT(user);

                    return Ok(new { Status = 200, Token = token, Message = "Login Successful" });
                }
                else
                {
                    return Ok(new { Status = 409, Message = "Login Failed" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
