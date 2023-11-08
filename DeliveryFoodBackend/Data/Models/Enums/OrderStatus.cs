using System.Text.Json.Serialization;

namespace DeliveryFoodBackend.Data.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        InProcess,
        Delivered
    }
}
