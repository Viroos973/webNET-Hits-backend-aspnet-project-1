using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.Data.Models;
using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace DeliveryFoodBackend.Service
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrder(Guid orderId, Guid userId)
        {
            var order = _context.Orders.Where(x => x.Id == orderId).FirstOrDefault();

            if (order == null)
            {
                throw new KeyNotFoundException(message: $"Order with id={orderId} don't in database");
            }

            if (userId != order.UserId)
            {
                throw new SecurityException(message: "Invalid order owner");
            }

            var dishInBaskets = await _context.Baskets.Where(x => x.OrderId == orderId).Join(_context.Dishes, b => b.DishId, d => d.Id, (b, d) => new DishBasketDto
            {
                Id = b.Id,
                Name = d.Name,
                Price = d.Price,
                TotalPrice = d.Price * b.Amount,
                Amount = b.Amount,
                Image = d.Image
            }).ToListAsync();

            return new OrderDto
            {
                Id= orderId,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = order.Price,
                Dishes = dishInBaskets,
                Address = order.Address
            };
        }

        public async Task CreateOrder(OrderCreateDto orderCreate, Guid userId)
        {
            var objectGuid = _context.AsHouses.Where(x => x.Objectguid == orderCreate.AddressId).FirstOrDefault();

            if(objectGuid == null)
            {
                throw new KeyNotFoundException(message: $"Not found object with ObjectGuid={orderCreate.AddressId}");
            }

            if (orderCreate.DeliveryTime <= DateTime.UtcNow.AddHours(1))
            {
                throw new BadHttpRequestException(message: "Invalid delivery time. Delivery time must be more than current datetime on 60 minutes");
            }

            var dishes = _context.Baskets.Where(x => x.UserId == userId && x.OrderId == null).ToList();

            if (dishes.Count() == 0)
            {
                throw new BadHttpRequestException(message: $"Empty basket for user with id={userId}");
            }

            var orderId = Guid.NewGuid();
            dishes.ForEach(x => x.OrderId = orderId);

            double res = 0;

            for (int i = 0; i < dishes.Count; i++)
            {
                var dish = _context.Dishes.Where(x => x.Id == dishes[i].DishId).FirstOrDefault();
                res += dish.Price * dishes[i].Amount;
            }

            await _context.Orders.AddAsync(new Order
            {
                Id = orderId,
                DeliveryTime = orderCreate.DeliveryTime,
                OrderTime = DateTime.UtcNow,
                Status = OrderStatus.InProcess.ToString(),
                Price = res,
                Address = orderCreate.AddressId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }
    }
}
