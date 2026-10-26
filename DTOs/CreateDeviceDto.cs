using System.ComponentModel.DataAnnotations;

namespace VehicleTrackingSystem.API.DTOs
{
    public class CreateDeviceDto
    {
        [Required(ErrorMessage = "Device ID is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Device ID must be between 2 and 100 characters.")]
        public string DeviceId { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "VehicleId must be greater than 0.")]
        public int VehicleId { get; set; }
    }
}
