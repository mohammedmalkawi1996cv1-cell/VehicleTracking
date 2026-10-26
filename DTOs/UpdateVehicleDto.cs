using System.ComponentModel.DataAnnotations;

namespace VehicleTrackingSystem.API.DTOs
{
    public class UpdateVehicleDto
    {
        [Required(ErrorMessage = "Vehicle name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Vehicle name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100,
            ErrorMessage = "Vehicle model cannot exceed 100 characters.")]
        public string? Model { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "DealerId must be greater than 0.")]
        public int DealerId { get; set; }
    }
}
