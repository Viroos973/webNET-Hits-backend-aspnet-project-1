using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        public readonly IOrderService _orderSevise;
        public readonly ITokenService _tokenService;

        public OrderController(IOrderService orderSevise, ITokenService tokenService)
        {
            _orderSevise = orderSevise;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                var orderDto = await _orderSevise.GetOrder(orderId, Guid.Parse(User.Identity.Name));
                return Ok(orderDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
            catch (SecurityException ex)
            {
                return StatusCode(403, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderCreate)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                await _orderSevise.CreateOrder(orderCreate, Guid.Parse(User.Identity.Name));
                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = ex.Message
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
