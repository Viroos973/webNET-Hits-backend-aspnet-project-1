using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.Data.Models;
using DeliveryFoodBackend.Services.Interfaces;

namespace DeliveryFoodBackend.Service
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDishBaskets(Guid dishId, Guid userId)
        {
            var dish = _context.Dishes.Where(x => x.Id == dishId).FirstOrDefault();

            if (dish == null)
            {
                throw new KeyNotFoundException(message: $"Dish with id={dishId} not found");
            }

            var dishInBasket = _context.Baskets.Where(x => x.DishId == dishId && x.UserId == userId).FirstOrDefault();

            if(dishInBasket != null)
            {
                dishInBasket.Amount += 1;
            }
            else
            {
                await _context.Baskets.AddAsync(new Basket
                {
                    Id = Guid.NewGuid(),
                    Amount = 1,
                    DishId = dishId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
