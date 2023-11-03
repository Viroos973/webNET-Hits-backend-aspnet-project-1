using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DeliveryFoodBackend.Service
{
    public class DishService : IDishService
    {
        private readonly AppDbContext _context;

        public DishService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DishPagedListDto> GetListDishes(DishCategory? category, bool vegetarian, DishSorting? sorting, int page)
        {
            if (category != null &&
                category != DishCategory.Pizza &&
                category != DishCategory.Drink &&
                category != DishCategory.Dessert &&
                category != DishCategory.Soup &&
                category != DishCategory.Wok)
            {
                throw new BadHttpRequestException(message: $"Dish categoty {category} is not available");
            }

            var dishes = await GetDishes(category, vegetarian);

            dishes = SortingDishes(dishes, sorting);

            if (dishes == null)
            {
                throw new BadHttpRequestException(message: $"Dish sorting {sorting} is not available");
            }

            if (page < 1 || dishes.Count() <= (page - 1) * 5)
            {
                throw new BadHttpRequestException(message: $"Page value must be greater than 0 and less than {(dishes.Count() + 4) / 5 + 1}");
            }

            var listDishes = dishes.Skip((page - 1) * 5).Take(5).ToList();

            var pagination = new PageInfoModel
            {
                Size = listDishes.Count(),
                Count = (dishes.Count() + 4) / 5,
                Current = page
            };

            return new DishPagedListDto
            {
                Dishes = listDishes,
                Pagination = pagination
            };
        }

        public async Task<DishDto> GetDishInfo(Guid id)
        {
            var dish = _context.Dishes.Where(x => x.Id == id).FirstOrDefault();

            if (dish == null)
            {
                throw new KeyNotFoundException(message: $"Dish with id={id} not found");
            }

            return new DishDto
            {
                Id = id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                Vegetarian = dish.Vegetarian,
                Rating = dish.Rating,
                Category = dish.Category
            };
        }

        public async Task<List<DishDto>> GetDishes(DishCategory? category, bool vegetarian)
        {
            if (vegetarian)
            {
                if (category == null)
                {
                    return await _context.Dishes.Where(x => x.Vegetarian == true).Select(x => new DishDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Image = x.Image,
                        Vegetarian = x.Vegetarian,
                        Rating = x.Rating,
                        Category = x.Category
                    }).ToListAsync();
                }

                return await _context.Dishes.Where(x => x.Vegetarian == true && x.Category == category.ToString()).Select(x => new DishDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Image = x.Image,
                    Vegetarian = x.Vegetarian,
                    Rating = x.Rating,
                    Category = x.Category
                }).ToListAsync();
            }
            else
            {
                if (category == null)
                {
                    return await _context.Dishes.Select(x => new DishDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Image = x.Image,
                        Vegetarian = x.Vegetarian,
                        Rating = x.Rating,
                        Category = x.Category
                    }).ToListAsync();
                }

                return await _context.Dishes.Where(x => x.Category == category.ToString()).Select(x => new DishDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Image = x.Image,
                    Vegetarian = x.Vegetarian,
                    Rating = x.Rating,
                    Category = x.Category
                }).ToListAsync();
            }
        }

        public List<DishDto> SortingDishes(List<DishDto> dishes, DishSorting? sorting)
        {
            if (sorting == null || sorting == DishSorting.NameAsc)
                return dishes.OrderBy(x => x.Name).ToList();
            if (sorting == DishSorting.NameDesc)
                return dishes.OrderByDescending(x => x.Name).ToList();
            if (sorting == DishSorting.PriceAsc)
                return dishes.OrderBy(x => x.Price).ToList();
            if (sorting == DishSorting.PriceDesc)
                return dishes.OrderByDescending(x => x.Price).ToList();
            if (sorting == DishSorting.RatingAsc)
                return dishes.OrderBy(x => x.Rating).ToList();
            if (sorting == DishSorting.RatingDesc)
                return dishes.OrderByDescending(x => x.Rating).ToList();
            return null;
        }
    }
}
