using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UsersManipulation.Business.DTOs;
using UsersManipulation.Business.DTOs.UserDtos;
using UsersManipulation.Business.Exceptions;
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
            try
            {
                var register = await _userService.RegisterUserAsync(model);
                return Ok(register);
            }
            catch (NotSucceededException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserDto model)
        {
            try
            {
                var login = await _userService.LoginUserAsync(model);
                return Ok(login);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotSucceededException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (LoginException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("block/{userId}")]
        [Authorize]
        public IActionResult BlockUser(int userId)
        {
            try
            {
                _userService.BlockUser(userId);
                return Ok("User blocked successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("unblock/{userId}")]
        [Authorize]
        public IActionResult UnblockUser(int userId)
        {
            try
            {
                _userService.UnblockUser(userId);
                return Ok("User unblocked successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("delete/{userId}")]
        [Authorize]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return Ok("User deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("getAll")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("getName/{userId}")]
        [Authorize]
        public IActionResult GetUserName(int userId)
        {
            try
            {
                var userName = _userService.GetUserName(userId);
                return Ok(userName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

