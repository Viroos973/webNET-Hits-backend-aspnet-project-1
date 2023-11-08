using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryFoodBackend.Data.Models
{
    public class Basket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public Guid DishId { get; set; }
        [ForeignKey("DishId")]
        public Dish Dish { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set;}
    }
}
