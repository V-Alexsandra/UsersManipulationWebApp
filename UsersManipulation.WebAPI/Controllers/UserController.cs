using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UsersManipulation.Business.DTOs;
using UsersManipulation.Business.DTOs.UserDtos;
using UsersManipulation.Business.Services.Contracts;

namespace UsersManipulation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDto model)
        {
            var register = await _userService.RegisterUserAsync(model);

            return Ok(register);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserDto model)
        {
            var login = await _userService.LoginUserAsync(model);
            return Ok(login);
        }

        [HttpPost("block/{userId}")]
        //[Authorize]
        public IActionResult BlockUser(int userId)
        {
            _userService.BlockUser(userId);
            return Ok("User blocked successfully.");
        }

        [HttpPost("unblock/{userId}")]
        //[Authorize]
        public IActionResult UnblockUser(int userId)
        {
            _userService.UnblockUser(userId);
            return Ok("User unblocked successfully.");
        }

        [HttpPost("delete/{userId}")]
        //[Authorize]
        public IActionResult DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
            return Ok("User deleted successfully.");
        }

        [HttpGet("canLogin/{userId}")]
        public IActionResult CanUserLogin(int userId)
        {
            var canLogin = _userService.CanUserLogin(userId);
            return Ok(canLogin);
        }

        [HttpGet("getAll")]
        //[Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
