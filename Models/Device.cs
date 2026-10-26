using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Models;

namespace VehicleTrackingSystem.API.Models
{
    public class Device
    {
        public int Id { get; set; }

        public string DeviceId { get; set; } = string.Empty;

        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; } = null!;

        public ICollection<DeviceLocation> Locations { get; set; }
            = new List<DeviceLocation>();
    }
}