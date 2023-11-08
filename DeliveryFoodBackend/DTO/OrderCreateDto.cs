using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class OrderCreateDto
    {
        [Required]
        public DateTime DeliveryTime { get; set; }

        [Required]
        public Guid AddressId { get; set; }
    }
}
