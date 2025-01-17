﻿using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IDishService
    {
        Task<DishPagedListDto> GetListDishes(DishCategory? category, bool vegetarian, DishSorting? sorting, int page);
        Task<DishDto> GetDishInfo(Guid id);
        Task<bool> CheckRatingUse(Guid dishId, Guid userId);
        Task SetRating(Guid dishId, Guid userId, int score);
    }
}
