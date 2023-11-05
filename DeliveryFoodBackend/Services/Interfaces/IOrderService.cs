﻿using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrder(OrderCreateDto orderCreate, Guid userId);
    }
}
