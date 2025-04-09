using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.API.Extensions;
using UserService.Application.Interfaces.Public;
using UserService.Application.Requests;

namespace UserService.API.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(
            [FromBody] RegisterUserRequest registerUserRequest, 
            CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterUserAsync(
                registerUserRequest, 
                cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(
            [FromBody] LoginUserRequest loginUserRequest,
            CancellationToken cancellationToken)
        {
            var result = await _userService.LoginUserAsync(
                loginUserRequest,
                cancellationToken);

            if(result.IsFailure)
                return BadRequest(result.Error);

            HttpContext.SetAuthTokens(
                result.Value.RefreshToken,
                result.Value.AccessToken);

            return Ok(result.Value);
        }

        [HttpPost("confirm")]
        public async Task<ActionResult> ConfirmUser(
            [FromBody] ConfirmUserRequest confirmUserRequest,
            CancellationToken cancellationToken)
        {
            var result = await _userService.ConfirmUserAsync(
                confirmUserRequest, 
                cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpPost("tokens/refresh")]
        public async Task<ActionResult> RefreshToken(
            CancellationToken cancellationToken)
        {
            var refreshToken = HttpContext.Request.Cookies["testy-cookies"];

            var result = await _userService.RefreshUserTokenAsync(
                refreshToken, 
                cancellationToken);

            if(result.IsFailure)
                return BadRequest(result.Error);

            HttpContext.SetAuthTokens(
                result.Value.RefreshToken, 
                result.Value.AccessToken);
            
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutUser(
            CancellationToken cancellationToken)
        {
            var refreshToken = HttpContext.Request.Cookies["testy-cookies"];
            var jwtTokenId = User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            HttpContext.DeleteAuthTokens();

            var result = await _userService.LogoutUserAsync(
                jwtTokenId,
                refreshToken,
                cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateUserInfo(
            [FromBody] UpdateUserInfoRequest updateUserInfoRequest,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _userService.UpdateUserInfoAsync(
                Guid.Parse(userId),
                updateUserInfoRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}
