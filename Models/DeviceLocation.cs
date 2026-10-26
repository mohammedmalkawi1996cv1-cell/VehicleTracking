using System;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Models;

namespace VehicleTrackingSystem.API.Models
{
    public class DeviceLocation
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime RecordedAt { get; set; }

        public Device Device { get; set; } = null!;
    }
}