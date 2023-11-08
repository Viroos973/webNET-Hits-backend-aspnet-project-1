using DeliveryFoodBackend.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class SearchAddressModel
    {
        [Required]
        public long ObjectId { get; set; }

        [Required]
        public Guid ObgectGuid { get; set; }

        public string? Text { get; set; }

        [Required]
        public GarAddressLevel ObjectLevel { get; set; }

        public string? ObjectLevelText { get; set; }
    }
}
