using Microsoft.AspNetCore.Mvc;
using JWTWithCustomMiddleware.Services;
using JWTWithCustomMiddleware.Authentication;
using JWTWithCustomMiddleware.Models;

namespace WebAPIWithJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService = userService;
        }

        [Authorize(Roles : Role.SuperAdmin)]
        [HttpGet("GetUserList")]
        public async Task<ActionResult> GetUserList()
        {
            try
            {
                var users = await _userService.GetUserList();

                return Ok(new {Status = 200, Users = users});
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetUserById")]
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                return Ok(new { Status = 200, User = user });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
