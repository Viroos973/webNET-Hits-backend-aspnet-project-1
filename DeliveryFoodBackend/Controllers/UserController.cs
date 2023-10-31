using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userSevise;
        public readonly ITokenService _tokenService;

        public UserController(IUserService userSevise, ITokenService tokenService)
        {
            _userSevise = userSevise;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegister(UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TokenResponse token = await _userSevise.RegisterUser(userRegisterModel);
                return Ok(token);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TokenResponse token = await _userSevise.Login(credentials);
                return Ok(token);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                await _userSevise.LogOut(token);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new Response
                {
                    Status = "Error",
                    Message = "User is not authorized"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                await _tokenService.CheckToken(HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length));
                UserDto user = await _userSevise.GetProfile(Guid.Parse(User.Identity.Name));
                return Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new Response
                {
                    Status = "Error",
                    Message = "User is not authorized"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> EditProfile(UserEditModel editModel)
        {
            try
            {
                await _tokenService.CheckToken(HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length));
                await _userSevise.EditProfile(editModel, Guid.Parse(User.Identity.Name));
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new Response
                {
                    Status = "Error",
                    Message = "User is not authorized"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }
    }
}
