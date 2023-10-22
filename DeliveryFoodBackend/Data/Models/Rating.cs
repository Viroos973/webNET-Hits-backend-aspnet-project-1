using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryFoodBackend.Data.Models
{
    public class Rating
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid DishId { get; set; }
        [ForeignKey("DishId")]
        public Dish Dish { get; set;}

        [Required]
        public double RatingScore { get; set; }
    }
}
