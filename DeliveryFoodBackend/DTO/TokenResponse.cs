using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class TokenResponse
    {
        [Required]
        [MinLength(1)]
        public string Token { get; set; }
    }
}
