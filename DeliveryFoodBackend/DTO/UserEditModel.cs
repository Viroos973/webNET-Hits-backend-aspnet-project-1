using DeliveryFoodBackend.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class UserEditModel
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public Guid? AddressId { get; set; }

        [Phone]
        [RegularExpression(@"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$", 
            ErrorMessage = "The phone number must match the following mask <+7 (xxx) xxx-xx-xx>")]
        public string? PhoneNumber { get; set; }
    }
}
