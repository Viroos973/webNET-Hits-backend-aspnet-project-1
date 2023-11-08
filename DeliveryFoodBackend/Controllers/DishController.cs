using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        public readonly IDishService _dishService;
        public readonly ITokenService _tokenService;

        public DishController(IDishService dishService, ITokenService tokenService)
        {
            _dishService = dishService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDishList(DishCategory? category, bool vegetarian, DishSorting? sorting, int page)
        {
            try
            {
                DishPagedListDto dishPagedList = await _dishService.GetListDishes(category, vegetarian, sorting, page);
                return Ok(dishPagedList);
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDishInfo(Guid id)
        {
            try
            {
                DishDto dishInfo = await _dishService.GetDishInfo(id);
                return Ok(dishInfo);
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

        [HttpGet]
        [Authorize]
        [Route("{id}/rating/check")]
        public async Task<IActionResult> CheckRatingUse(Guid id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                bool checkRating = await _dishService.CheckRatingUse(id, Guid.Parse(User.Identity.Name));
                return Ok(checkRating);
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

        [HttpPost]
        [Authorize]
        [Route("{id}/rating")]
        public async Task<IActionResult> SetRating(Guid id,[Range(0, 10)] int score)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                await _tokenService.CheckToken(token);
                await _dishService.SetRating(id, Guid.Parse(User.Identity.Name), score);
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
