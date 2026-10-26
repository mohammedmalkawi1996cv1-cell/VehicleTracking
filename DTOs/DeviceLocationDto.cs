namespace VehicleTrackingSystem.API.DTOs
{
    public class DeviceLocationDto
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime RecordedAt { get; set; }
    }
}