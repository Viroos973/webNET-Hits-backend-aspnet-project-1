using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/basket")]
    public class BasketController : ControllerBase
    {
        public readonly IBasketService _basketSevise;
        public readonly ITokenService _tokenService;

        public BasketController(IBasketService basketSevise, ITokenService tokenService)
        {
            _basketSevise = basketSevise;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Authorize]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> AddDishBasket(Guid dishId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                await _basketSevise.AddDishBaskets(dishId, Guid.Parse(User.Identity.Name));
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response
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

        [HttpDelete]
        [Authorize]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> DeleteDishBasket(Guid dishId, bool increase)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                await _basketSevise.DeleteDishBaskets(dishId, Guid.Parse(User.Identity.Name), increase);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response
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
    }
}
