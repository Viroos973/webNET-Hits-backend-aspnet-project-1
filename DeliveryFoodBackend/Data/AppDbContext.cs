﻿using Microsoft.EntityFrameworkCore;

namespace DeliveryFoodBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}