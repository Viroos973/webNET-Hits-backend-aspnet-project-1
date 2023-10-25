using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.Data.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(1)]
        [Required]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public string Genders { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        [MinLength (1)]
        [EmailAddress]
        public string EmailAddress { get; set;}

        public string? Address { get; set; }

        [Required]
        [MinLength(1)]
        public string Password { get; set; }

        public List<Rating> Ratings { get; set; }
        public List<Order> Orders { get; set; }
        public List<Basket> Baskets { get; set; }
    }
}
