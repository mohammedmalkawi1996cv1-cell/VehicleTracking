using System.ComponentModel.DataAnnotations;

namespace VehicleTrackingSystem.API.DTOs
{
    public class UpdateDealerDto
    {
        [Required(ErrorMessage = "Dealer name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Dealer name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200,
            ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }
    }
}
