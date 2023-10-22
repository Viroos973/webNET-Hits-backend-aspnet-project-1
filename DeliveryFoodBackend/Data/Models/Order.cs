using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace DeliveryFoodBackend.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DataType DeliveryTime { get; set; }

        [Required]
        public DataType OrderTime { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [MinLength(1)]
        public string Address { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<Basket> Baskets { get; set; }
    }
}
