namespace VehicleTrackingSystem.API.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Model { get; set; }

        public int DealerId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
