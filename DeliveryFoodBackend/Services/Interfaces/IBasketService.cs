using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IBasketService
    {
        Task<List<DishBasketDto>> GetDishBaskets(Guid userId);
        Task AddDishBaskets(Guid dishId, Guid userId);
        Task DeleteDishBaskets(Guid dishId, Guid userId, bool increase);
    }
}
