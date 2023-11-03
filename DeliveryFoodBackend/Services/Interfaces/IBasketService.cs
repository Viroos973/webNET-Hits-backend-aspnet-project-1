namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddDishBaskets(Guid dishId, Guid userId);
        Task DeleteDishBaskets(Guid dishId, Guid userId, bool increase);
    }
}
