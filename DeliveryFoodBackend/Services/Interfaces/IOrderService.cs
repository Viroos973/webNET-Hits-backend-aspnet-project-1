using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrder(Guid orderId, Guid userId);
        Task<List<OrderInfoDto>> GetListOrders(Guid userId);
        Task CreateOrder(OrderCreateDto orderCreate, Guid userId);
    }
}
