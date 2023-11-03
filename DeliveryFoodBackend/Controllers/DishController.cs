using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        public readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
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
    }
}
