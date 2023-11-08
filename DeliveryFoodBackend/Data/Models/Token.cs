using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.Data.Models
{
    public class Token
    {
        [Key]
        [Required]
        public string InvalideToken { get; set; }

        [Required]
        public DateTime ExpiredDate { get; set; }
    }
}
