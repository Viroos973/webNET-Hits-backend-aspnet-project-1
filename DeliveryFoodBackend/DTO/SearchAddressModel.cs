﻿using System.ComponentModel.DataAnnotations;

namespace DeliveryFoodBackend.DTO
{
    public class SearchAddressModel
    {
        [Required]
        public int ObjectId { get; set; }

        [Required]
        public Guid ObgectGuid { get; set; }

        public string? Text { get; set; }

        [Required]
        public string ObjectLevel { get; set; }

        public string? ObjectLevelText { get; set; }
    }
}