using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class PageInfoModel
    {
        [Required]
        public int Size { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public int Current {  get; set; }
    }
}
