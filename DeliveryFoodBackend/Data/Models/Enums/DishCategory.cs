﻿using System.Text.Json.Serialization;

namespace DeliveryFoodBackend.Data.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DishCategory
    {
        Wok,
        Pizza,
        Soup,
        Dessert,
        Drink
    }
}
