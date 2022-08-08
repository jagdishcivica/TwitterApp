using Microsoft.AspNetCore.Mvc;
using TwitterApp.Dtos;
using TwitterApp.Repository;

namespace TwitterApp.Controllers
{
    [Route("api/v1.0/tweets/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var response = await _authRepository.Register(
                new Models.User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    LoginId = userDto.LoginId,
                    ContactNumber = userDto.ContactNumber,
                }, userDto.Password);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authRepository.Login(loginDto.LoginId, loginDto.Password);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("users/all")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetAllUser()
        {
            return Ok(await _authRepository.UserList());
        }

        [HttpGet("user/search/{username}")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> SearchUser(string username)
        {
            return Ok(await _authRepository.SearchUser(username));
        }
    }
}
